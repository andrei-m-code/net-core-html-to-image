using FluentAssertions;
using Xunit;

namespace CoreHtmlToImage.Tests;

[Trait("Category", "Integration")]
public class IntegrationTests : IAsyncLifetime
{
    private HtmlConverter _converter = null!;

    public async Task InitializeAsync()
    {
        _converter = new HtmlConverter();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _converter.DisposeAsync();
    }

    [Fact]
    public async Task FromHtmlStringAsync_SimpleHtml_ReturnsJpgBytes()
    {
        var html = "<div><strong>Hello</strong> World!</div>";

        var bytes = await _converter.FromHtmlStringAsync(html);

        bytes.Should().NotBeEmpty();
        // JPEG magic bytes: FF D8
        bytes[0].Should().Be(0xFF);
        bytes[1].Should().Be(0xD8);
    }

    [Fact]
    public async Task FromHtmlStringAsync_PngFormat_ReturnsPngBytes()
    {
        var html = "<div>PNG Test</div>";
        var options = new HtmlConverterOptions { Format = ImageFormat.Png };

        var bytes = await _converter.FromHtmlStringAsync(html, options);

        bytes.Should().NotBeEmpty();
        // PNG magic bytes: 89 50 4E 47
        bytes[0].Should().Be(0x89);
        bytes[1].Should().Be(0x50);
        bytes[2].Should().Be(0x4E);
        bytes[3].Should().Be(0x47);
    }

    [Fact]
    public async Task FromHtmlStringAsync_WebPFormat_ReturnsWebPBytes()
    {
        var html = "<div>WebP Test</div>";
        var options = new HtmlConverterOptions { Format = ImageFormat.WebP };

        var bytes = await _converter.FromHtmlStringAsync(html, options);

        bytes.Should().NotBeEmpty();
        // WebP: starts with RIFF....WEBP
        bytes[0].Should().Be((byte)'R');
        bytes[1].Should().Be((byte)'I');
        bytes[2].Should().Be((byte)'F');
        bytes[3].Should().Be((byte)'F');
    }

    [Fact]
    public async Task FromHtmlStringAsync_CustomWidth_ProducesImage()
    {
        var html = "<div>Width Test</div>";
        var options = new HtmlConverterOptions { Width = 640 };

        var bytes = await _converter.FromHtmlStringAsync(html, options);

        bytes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FromHtmlStringAsync_FullPage_ProducesImage()
    {
        var html = "<div style='height:2000px'>Tall content</div>";
        var options = new HtmlConverterOptions { FullPage = true };

        var bytes = await _converter.FromHtmlStringAsync(html, options);

        bytes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FromUrlAsync_ValidUrl_ReturnsImageBytes()
    {
        var bytes = await _converter.FromUrlAsync("https://example.com");

        bytes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FromHtmlString_SyncWrapper_Works()
    {
        var html = "<div>Sync Test</div>";

        var bytes = _converter.FromHtmlString(html);

        bytes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FromUrl_SyncWrapper_Works()
    {
        var bytes = _converter.FromUrl("https://example.com");

        bytes.Should().NotBeEmpty();
        await Task.CompletedTask;
    }
}
