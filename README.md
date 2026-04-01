[![CI](https://github.com/andrei-m-code/net-core-html-to-image/actions/workflows/ci.yml/badge.svg)](https://github.com/andrei-m-code/net-core-html-to-image/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/CoreHtmlToImage.svg)](https://www.nuget.org/packages/CoreHtmlToImage/)

# .NET Core HTML to Image Converter

Cross-platform .NET library that converts HTML strings or URLs to image bytes using headless Chromium (via [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp)). Supports Windows, Linux, and macOS.

## Installation

```
Install-Package CoreHtmlToImage
```

dotnet CLI:
```
dotnet add package CoreHtmlToImage
```

> **Note:** On first use, PuppeteerSharp automatically downloads a compatible Chromium binary (~200MB, cached for subsequent runs).

## Usage

### Async API (recommended)

```csharp
await using var converter = new HtmlConverter();

// From HTML string
var bytes = await converter.FromHtmlStringAsync("<div><strong>Hello</strong> World!</div>");
File.WriteAllBytes("image.jpg", bytes);

// From URL
var bytes = await converter.FromUrlAsync("https://example.com");
File.WriteAllBytes("image.jpg", bytes);
```

### Options

```csharp
var options = new HtmlConverterOptions
{
    Width = 800,           // Viewport width (default: 1024)
    Height = 600,          // Viewport height (default: auto)
    Format = ImageFormat.Png,  // Jpg, Png, or WebP (default: Jpg)
    Quality = 90,          // 1-100, for Jpg/WebP (default: 100)
    FullPage = true,       // Capture full scrollable page (default: false)
    OmitBackground = true  // Transparent background for PNG/WebP (default: false)
};

var bytes = await converter.FromHtmlStringAsync(html, options);
```

### Sync API (backward-compatible)

```csharp
var converter = new HtmlConverter();
var bytes = converter.FromHtmlString(html);
var bytes = converter.FromUrl("https://example.com", width: 800, format: ImageFormat.Png, quality: 90);
```

## Migrating from v1.x

v2.0 replaces wkhtmltoimage with headless Chromium. Key changes:

- **`HtmlConverter` now implements `IAsyncDisposable`/`IDisposable`** — use `await using` or `using` for proper cleanup
- **New async methods** — `FromHtmlStringAsync` and `FromUrlAsync` are the primary API
- **Sync methods still work** with the same signatures as v1.x
- **New `ImageFormat.WebP`** format option
- **macOS is now supported**
- **No more embedded exe** — Chromium is downloaded automatically on first use
- **Requires .NET 10.0+** (v1.x targeted .NET Standard 2.0)

## MIT License

Copyright (c) 2020 Andrei M

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
