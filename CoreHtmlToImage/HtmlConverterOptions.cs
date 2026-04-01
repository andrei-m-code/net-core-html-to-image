namespace CoreHtmlToImage;

/// <summary>
/// Configuration options for <see cref="HtmlConverter"/>.
/// </summary>
public sealed class HtmlConverterOptions
{
    /// <summary>
    /// Output document viewport width in pixels. Default is 1024.
    /// </summary>
    public int Width { get; set; } = 1024;

    /// <summary>
    /// Output document viewport height in pixels. When null, the page height is determined automatically.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Output image format. Default is Jpg.
    /// </summary>
    public ImageFormat Format { get; set; } = ImageFormat.Jpg;

    /// <summary>
    /// Output image quality from 1 to 100. Applies to Jpg and WebP only. Default is 100.
    /// </summary>
    public int Quality { get; set; } = 100;

    /// <summary>
    /// Whether to capture the full scrollable page. Default is false.
    /// </summary>
    public bool FullPage { get; set; }

    /// <summary>
    /// Whether to hide the default white background and allow transparency (PNG/WebP only). Default is false.
    /// </summary>
    public bool OmitBackground { get; set; }

    /// <summary>
    /// Maximum navigation timeout in milliseconds. Default is 30000 (30 seconds). Set to 0 for no timeout.
    /// </summary>
    public int NavigationTimeout { get; set; } = 30_000;
}
