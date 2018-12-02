# .NET Core HTML to Image converter

This is a .NET Core HTML to Image converter. It helps converting HTML strings or URLs to image bytes. Please see the examples:

# Convert HTML string to image bytes
```
var converter = new HtmlConverter();
var html = "<div><strong>Hello</strong> World!</div>";
var htmlStringBytes = converter.FromHtmlString(html);
```
            
# Convert URL to image bytes
```
var urlImageBytes = converter.FromUrl("http://google.com", 800, format: ImageFormat.Png, quality: 1);
File.WriteAllBytes("D:\\lowquo.png", urlImageBytes);
```

# Optional parameters
1. width - output document width. Default is 1024
2. format - output image format. Default is Jpg
3. quality - output image quality from 1 to 100. Default is 100

# More about this library
Because I couldn't find a free implementation of .NET Core HTML to Image library, I've developed my own and hope it will help other people as well. This library wraps very nice wkhtmltoimage tool (https://wkhtmltopdf.org/). I have wkhtmltoimage.exe embedded into my library. This is a very simple implementation. Your contributions are very welcome!
