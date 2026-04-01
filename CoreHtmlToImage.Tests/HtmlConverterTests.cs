using FluentAssertions;
using Xunit;

namespace CoreHtmlToImage.Tests;

public class HtmlConverterTests
{
    [Fact]
    public async Task FromHtmlStringAsync_NullHtml_ThrowsArgumentException()
    {
        await using var converter = new HtmlConverter();

        var act = () => converter.FromHtmlStringAsync(null!);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task FromHtmlStringAsync_EmptyHtml_ThrowsArgumentException()
    {
        await using var converter = new HtmlConverter();

        var act = () => converter.FromHtmlStringAsync("");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task FromUrlAsync_NullUrl_ThrowsArgumentException()
    {
        await using var converter = new HtmlConverter();

        var act = () => converter.FromUrlAsync(null!);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task FromUrlAsync_EmptyUrl_ThrowsArgumentException()
    {
        await using var converter = new HtmlConverter();

        var act = () => converter.FromUrlAsync("");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task FromHtmlStringAsync_AfterDispose_ThrowsObjectDisposedException()
    {
        var converter = new HtmlConverter();
        await converter.DisposeAsync();

        var act = () => converter.FromHtmlStringAsync("<p>test</p>");

        await act.Should().ThrowAsync<ObjectDisposedException>();
    }

    [Fact]
    public async Task FromUrlAsync_AfterDispose_ThrowsObjectDisposedException()
    {
        var converter = new HtmlConverter();
        await converter.DisposeAsync();

        var act = () => converter.FromUrlAsync("https://example.com");

        await act.Should().ThrowAsync<ObjectDisposedException>();
    }

    [Fact]
    public async Task DisposeAsync_CanBeCalledMultipleTimes()
    {
        var converter = new HtmlConverter();

        await converter.DisposeAsync();
        await converter.DisposeAsync();
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var converter = new HtmlConverter();

        converter.Dispose();
        converter.Dispose();
    }
}
