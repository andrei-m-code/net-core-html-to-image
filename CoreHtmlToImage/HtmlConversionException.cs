namespace CoreHtmlToImage;

/// <summary>
/// Exception thrown when HTML to image conversion fails.
/// </summary>
public class HtmlConversionException : Exception
{
    public HtmlConversionException(string message) : base(message) { }

    public HtmlConversionException(string message, Exception innerException) : base(message, innerException) { }
}
