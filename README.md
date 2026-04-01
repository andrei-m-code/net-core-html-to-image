[![CI](https://github.com/andrei-m-code/net-core-html-to-image/actions/workflows/ci.yml/badge.svg)](https://github.com/andrei-m-code/net-core-html-to-image/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/CoreHtmlToImage.svg)](https://www.nuget.org/packages/CoreHtmlToImage/)

# .NET Core HTML to Image Converter

Cross-platform .NET library that converts HTML strings or URLs to image bytes using headless Chromium (via [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp)). Supports Windows, Linux, and macOS.

## Installation

NuGet Package Manager:
```
Install-Package CoreHtmlToImage
```

dotnet CLI:
```
dotnet add package CoreHtmlToImage
```

**Requirements:**
- .NET 10.0 or later
- On first use, PuppeteerSharp automatically downloads a compatible Chromium binary (~200MB, cached for subsequent runs)

## Quick Start

```csharp
await using var converter = new HtmlConverter();

// Convert HTML string to image
var bytes = await converter.FromHtmlStringAsync("<div><strong>Hello</strong> World!</div>");
File.WriteAllBytes("image.jpg", bytes);
```

## Usage

### Convert HTML String to Image

```csharp
await using var converter = new HtmlConverter();

var html = @"
<html>
<body>
    <h1>Hello World</h1>
    <p>This is rendered with headless Chromium.</p>
</body>
</html>";

var bytes = await converter.FromHtmlStringAsync(html);
File.WriteAllBytes("output.jpg", bytes);
```

### Convert URL to Image

```csharp
await using var converter = new HtmlConverter();

var bytes = await converter.FromUrlAsync("https://example.com");
File.WriteAllBytes("screenshot.jpg", bytes);
```

### Using Options

All conversion options are configured via `HtmlConverterOptions`:

```csharp
await using var converter = new HtmlConverter();

var options = new HtmlConverterOptions
{
    Width = 800,                  // Viewport width in pixels (default: 1024)
    Height = 600,                 // Viewport height in pixels (default: auto)
    Format = ImageFormat.Png,     // Jpg, Png, or WebP (default: Jpg)
    Quality = 90,                 // 1-100, applies to Jpg only (default: 100)
    FullPage = true,              // Capture full scrollable page (default: false)
    OmitBackground = true,        // Transparent background for PNG/WebP (default: false)
    NavigationTimeout = 60000     // Navigation timeout in ms (default: 30000, 0 = no timeout)
};

var bytes = await converter.FromHtmlStringAsync(html, options);
```

### Options Reference

| Property | Type | Default | Description |
|---|---|---|---|
| `Width` | `int` | `1024` | Viewport width in pixels |
| `Height` | `int?` | `null` (auto) | Viewport height in pixels. When null, height is determined automatically |
| `Format` | `ImageFormat` | `Jpg` | Output format: `Jpg`, `Png`, or `WebP` |
| `Quality` | `int` | `100` | Image quality from 1 to 100. Applies to Jpg only |
| `FullPage` | `bool` | `false` | When true, captures the full scrollable page instead of just the viewport |
| `OmitBackground` | `bool` | `false` | When true, hides the default white background for transparent PNG/WebP output |
| `NavigationTimeout` | `int` | `30000` | Maximum time in milliseconds to wait for page navigation. Set to 0 for no timeout |

### Cancellation Support

Async methods accept a `CancellationToken`:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

var bytes = await converter.FromUrlAsync("https://example.com", options, cts.Token);
```

### Sync API (backward-compatible with v1.x)

The synchronous API from v1.x is still available with the same method signatures:

```csharp
var converter = new HtmlConverter();

// Same signatures as v1.x
var bytes = converter.FromHtmlString("<p>Hello</p>");
var bytes = converter.FromUrl("https://example.com", width: 800, format: ImageFormat.Png, quality: 90);
```

> **Note:** The async API is recommended for new code. The sync methods block the calling thread while waiting for Chromium.

### Disposal

`HtmlConverter` manages a headless Chromium browser process internally. Always dispose it when done:

```csharp
// Async (preferred)
await using var converter = new HtmlConverter();

// Sync
using var converter = new HtmlConverter();
```

The browser instance is reused across multiple conversions for performance. A single `HtmlConverter` instance is safe to use from multiple threads concurrently.

### Error Handling

Conversion errors throw `HtmlConversionException`:

```csharp
try
{
    var bytes = await converter.FromUrlAsync("https://invalid-url-example.test");
}
catch (HtmlConversionException ex)
{
    Console.WriteLine($"Conversion failed: {ex.Message}");
    // ex.InnerException contains the underlying PuppeteerSharp exception
}
```

## Migrating from v1.x

v2.0 replaces the abandoned wkhtmltoimage engine with headless Chromium via PuppeteerSharp.

**What changed:**

| | v1.x | v2.0 |
|---|---|---|
| Rendering engine | wkhtmltoimage (QtWebKit) | Chromium (PuppeteerSharp) |
| Target framework | .NET Standard 2.0 | .NET 10.0 |
| Primary API | Sync only | Async (sync wrappers available) |
| Image formats | Jpg, Png | Jpg, Png, WebP |
| Platform support | Windows, Linux | Windows, Linux, macOS |
| Package size | ~41MB (embedded exe) | ~100KB (Chromium downloaded on first use) |
| Disposal | Not required | `IAsyncDisposable` / `IDisposable` |
| Error type | `System.Exception` | `HtmlConversionException` |

**Source-compatible changes** (existing code compiles without modification):
- `new HtmlConverter()` still works
- `FromHtmlString()` and `FromUrl()` keep the same signatures and defaults
- `ImageFormat.Jpg` and `ImageFormat.Png` enum values unchanged

**Behavioral changes:**
- First call downloads Chromium (~200MB, cached after that)
- Rendering uses Chromium instead of QtWebKit (minor visual differences, generally better CSS/JS support)
- `HtmlConverter` should be disposed to clean up the browser process

## License

MIT License - see [LICENSE.md](LICENSE.md) for details.
