[![Build Status](https://travis-ci.org/andrei-m-code/net-core-html-to-image.svg?branch=master)](https://travis-ci.org/andrei-m-code/net-core-html-to-image) [![NuGet](https://img.shields.io/nuget/v/CoreHtmlToImage.svg)](https://www.nuget.org/packages/CoreHtmlToImage/)

# .NET Core HTML to Image Converter

This is a .NET Core HTML to Image converter. It helps converting HTML strings or URLs to image bytes. Please see the examples:

## Installation Instructions
Nuget package available (https://www.nuget.org/packages/CoreHtmlToImage/)
```
Install-Package CoreHtmlToImage
```
dotnet cli:
```
dotnet add package CoreHtmlToImage
```
## Convert HTML string to image bytes
```
var converter = new HtmlConverter();
var html = "<div><strong>Hello</strong> World!</div>";
var bytes = converter.FromHtmlString(html);
File.WriteAllBytes("image.jpg", bytes);
```
            
## Convert URL to image bytes
```
var converter = new HtmlConverter();
var bytes = converter.FromUrl("http://google.com");
File.WriteAllBytes("image.jpg", bytes);
```

## Optional parameters
1. width - output document width. Default is 1024.
2. format - output image format. Default is Jpg.
3. quality - output image quality from 1 to 100. Default is 100.

## Roadmap
* Async methods
* Non-Windows support

## More about this library
Because I couldn't find a free implementation of .NET Core HTML to Image library, I've developed my own and hope it will help other people as well. This library wraps very nice wkhtmltoimage tool (https://wkhtmltopdf.org/). I have wkhtmltoimage.exe embedded into my library. This is a very simple implementation. Your contributions are very welcome!

## MIT License

Copyright (c) 2020 Andrei M

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
