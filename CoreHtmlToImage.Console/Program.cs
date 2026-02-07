using System.IO;

namespace CoreHtmlToImage.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // From HTML string
            var converter = new HtmlConverter();
            var html = "<div><strong>Hello</strong> World!</div>";
            var htmlBytes = converter.FromHtmlString(html);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "image_from_html_string.png"), htmlBytes);

            // From URL
            var urlBytes = converter.FromUrl("http://google.com", 800, format: ImageFormat.Png, quality: 90);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "image_from_url.png"), urlBytes);
        }
    }
}
