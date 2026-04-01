using CoreHtmlToImage;

await using var converter = new HtmlConverter();

// From HTML string (async)
var html = "<div><strong>Hello</strong> World!</div>";
var htmlBytes = await converter.FromHtmlStringAsync(html);
File.WriteAllBytes("hello.jpg", htmlBytes);
Console.WriteLine("Saved hello.jpg");

// From URL with options
var options = new HtmlConverterOptions
{
    Width = 800,
    Format = ImageFormat.Png,
    Quality = 90,
    FullPage = true,
    NavigationTimeout = 120000
};
var urlBytes = await converter.FromUrlAsync("https://google.com", options);
File.WriteAllBytes("example.png", urlBytes);
Console.WriteLine("Saved example.png");

// Backward-compatible sync API still works
var syncBytes = converter.FromHtmlString("<p>Sync API</p>", width: 640);
File.WriteAllBytes("sync.jpg", syncBytes);
Console.WriteLine("Saved sync.jpg");
