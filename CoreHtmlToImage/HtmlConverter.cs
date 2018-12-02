using System;
using System.Diagnostics;
using System.IO;

namespace CoreHtmlToImage
{
    /// <summary>
    /// Html Converter. Converts HTML string and URLs to image bytes
    /// </summary>
    public class HtmlConverter
    {
        /// <summary>
        /// Converts HTML string to image
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <param name="width">Output document width</param>
        /// <param name="format">Output image format</param>
        /// <param name="quality">Output image quality 1-100</param>
        /// <returns></returns>
        public byte[] FromHtmlString(string html, int width = 1024, ImageFormat format = ImageFormat.Jpg, int quality = 100)
        {
            var directory = AppContext.BaseDirectory;
            var filename = Path.Combine(directory, $"{Guid.NewGuid()}.html");
            File.WriteAllText(filename, html);
            var bytes = FromUrl(filename, width, format, quality);
            File.Delete(filename);
            return bytes;
        }

        /// <summary>
        /// Converts HTML page to image
        /// </summary>
        /// <param name="url">Valid http(s):// URL</param>
        /// <param name="width">Output document width</param>
        /// <param name="format">Output image format</param>
        /// <param name="quality">Output image quality 1-100</param>
        /// <returns></returns>
        public byte[] FromUrl(string url, int width = 1024, ImageFormat format = ImageFormat.Jpg, int quality = 100)
        {
            var directory = AppContext.BaseDirectory;
            var imageFormat = format.ToString().ToLower();
            var filename = Path.Combine(directory, $"{Guid.NewGuid().ToString()}.{imageFormat}");
            var toolpath = Path.Combine(directory, "wkhtmltoimage.exe");
            Process process = Process.Start(new ProcessStartInfo(toolpath, $"--quality {quality} --width {width} -f {imageFormat} {url} {filename}")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = directory,
                RedirectStandardError = true
            });

            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.WaitForExit();

            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                File.Delete(filename);
                return bytes;
            }

            throw new Exception("Something went wrong. Please check input parameters");
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception(e.Data);
        }
    }
}
