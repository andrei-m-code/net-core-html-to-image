using FluentAssertions;
using Xunit;

namespace CoreHtmlToImage.Tests;

public class HtmlConverterOptionsTests
{
    [Fact]
    public void Defaults_AreCorrect()
    {
        var options = new HtmlConverterOptions();

        options.Width.Should().Be(1024);
        options.Height.Should().BeNull();
        options.Format.Should().Be(ImageFormat.Jpg);
        options.Quality.Should().Be(100);
        options.FullPage.Should().BeFalse();
        options.OmitBackground.Should().BeFalse();
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        var options = new HtmlConverterOptions
        {
            Width = 800,
            Height = 600,
            Format = ImageFormat.Png,
            Quality = 50,
            FullPage = true,
            OmitBackground = true
        };

        options.Width.Should().Be(800);
        options.Height.Should().Be(600);
        options.Format.Should().Be(ImageFormat.Png);
        options.Quality.Should().Be(50);
        options.FullPage.Should().BeTrue();
        options.OmitBackground.Should().BeTrue();
    }
}
