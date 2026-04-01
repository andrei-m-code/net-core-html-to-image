using PuppeteerSharp;

namespace CoreHtmlToImage;

/// <summary>
/// Converts HTML strings and URLs to image bytes using headless Chromium.
/// Implements <see cref="IAsyncDisposable"/> for proper browser cleanup.
/// </summary>
public sealed class HtmlConverter : IAsyncDisposable, IDisposable
{
    private IBrowser? _browser;
    private readonly SemaphoreSlim _browserLock = new(1, 1);
    private bool _disposed;

    /// <summary>
    /// Converts an HTML string to image bytes.
    /// </summary>
    /// <param name="html">HTML string to convert.</param>
    /// <param name="options">Conversion options. Uses defaults if null.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Image bytes in the specified format.</returns>
    public async Task<byte[]> FromHtmlStringAsync(string html, HtmlConverterOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(html);
        ObjectDisposedException.ThrowIf(_disposed, this);

        options ??= new HtmlConverterOptions();

        try
        {
            var browser = await GetBrowserAsync(cancellationToken).ConfigureAwait(false);
            await using var page = await browser.NewPageAsync().ConfigureAwait(false);
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = options.Width,
                Height = options.Height ?? 768
            }).ConfigureAwait(false);
            await page.SetContentAsync(html, new NavigationOptions
            {
                Timeout = options.NavigationTimeout,
                WaitUntil = [WaitUntilNavigation.Networkidle0]
            }).ConfigureAwait(false);

            if (!options.OmitBackground)
                await page.EvaluateExpressionAsync("document.body.style.backgroundColor = document.body.style.backgroundColor || 'white'").ConfigureAwait(false);

            return await CaptureScreenshotAsync(page, options).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not HtmlConversionException and not ArgumentException and not ObjectDisposedException)
        {
            throw new HtmlConversionException($"Failed to convert HTML string to image: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Converts a URL to image bytes.
    /// </summary>
    /// <param name="url">URL to capture.</param>
    /// <param name="options">Conversion options. Uses defaults if null.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Image bytes in the specified format.</returns>
    public async Task<byte[]> FromUrlAsync(string url, HtmlConverterOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(url);
        ObjectDisposedException.ThrowIf(_disposed, this);

        options ??= new HtmlConverterOptions();

        try
        {
            var browser = await GetBrowserAsync(cancellationToken).ConfigureAwait(false);
            await using var page = await browser.NewPageAsync().ConfigureAwait(false);
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = options.Width,
                Height = options.Height ?? 768
            }).ConfigureAwait(false);
            await page.GoToAsync(url, new NavigationOptions
            {
                Timeout = options.NavigationTimeout,
                WaitUntil = [WaitUntilNavigation.Networkidle0]
            }).ConfigureAwait(false);

            if (!options.OmitBackground)
                await page.EvaluateExpressionAsync("document.body.style.backgroundColor = document.body.style.backgroundColor || 'white'").ConfigureAwait(false);

            return await CaptureScreenshotAsync(page, options).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not HtmlConversionException and not ArgumentException and not ObjectDisposedException)
        {
            throw new HtmlConversionException($"Failed to convert URL '{url}' to image: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Converts an HTML string to image bytes (synchronous backward-compatible wrapper).
    /// </summary>
    public byte[] FromHtmlString(string html, int width = 1024, ImageFormat format = ImageFormat.Jpg, int quality = 100)
    {
        var options = new HtmlConverterOptions { Width = width, Format = format, Quality = quality };
        return FromHtmlStringAsync(html, options).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Converts a URL to image bytes (synchronous backward-compatible wrapper).
    /// </summary>
    public byte[] FromUrl(string url, int width = 1024, ImageFormat format = ImageFormat.Jpg, int quality = 100)
    {
        var options = new HtmlConverterOptions { Width = width, Format = format, Quality = quality };
        return FromUrlAsync(url, options).GetAwaiter().GetResult();
    }

    private async Task<IBrowser> GetBrowserAsync(CancellationToken cancellationToken)
    {
        if (_browser is { IsClosed: false })
            return _browser;

        await _browserLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_browser is { IsClosed: false })
                return _browser;

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync().ConfigureAwait(false);

            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = ["--no-sandbox", "--disable-setuid-sandbox"]
            }).ConfigureAwait(false);

            return _browser;
        }
        finally
        {
            _browserLock.Release();
        }
    }

    private static async Task<byte[]> CaptureScreenshotAsync(IPage page, HtmlConverterOptions options)
    {
        var screenshotType = MapFormat(options.Format);

        return await page.ScreenshotDataAsync(new ScreenshotOptions
        {
            Type = screenshotType,
            Quality = screenshotType == ScreenshotType.Jpeg ? options.Quality : null,
            FullPage = options.FullPage,
            OmitBackground = options.OmitBackground
        }).ConfigureAwait(false);
    }

    private static ScreenshotType MapFormat(ImageFormat format) => format switch
    {
        ImageFormat.Jpg => ScreenshotType.Jpeg,
        ImageFormat.Png => ScreenshotType.Png,
        ImageFormat.WebP => ScreenshotType.Webp,
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported image format.")
    };

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (_browser is not null)
        {
            try
            {
                await _browser.CloseAsync().ConfigureAwait(false);
            }
            catch
            {
                // Best-effort cleanup
            }
            _browser.Dispose();
            _browser = null;
        }

        _browserLock.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_browser is not null)
        {
            try
            {
                _browser.CloseAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // Best-effort cleanup
            }
            _browser.Dispose();
            _browser = null;
        }

        _browserLock.Dispose();
    }
}
