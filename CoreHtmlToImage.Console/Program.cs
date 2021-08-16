using System.IO;
using System.Text;

namespace CoreHtmlToImage.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // From HTML string
            var converter = new HtmlConverter();
            var html = "<div><strong>Hello</strong> World!</div>";
            var htmlBytes = converter.FromHtmlString(html,Encoding.UTF8);

            // From URL
            var urlBytes = converter.FromUrl("http://baidu.com", 800, format: ImageFormat.Png, quality: 90);
            File.WriteAllBytes("/Users/traceless/image.png", urlBytes);
        }
    }
}
