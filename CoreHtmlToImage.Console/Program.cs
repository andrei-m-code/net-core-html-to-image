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
            var htmlStringBytes = converter.FromHtmlString(html);
            
            // From URL
            var urlImageBytes = converter.FromUrl("http://google.com", 800, format: ImageFormat.Png, quality: 1);
            File.WriteAllBytes("D:\\lowquo.png", urlImageBytes);
        }
    }
}
