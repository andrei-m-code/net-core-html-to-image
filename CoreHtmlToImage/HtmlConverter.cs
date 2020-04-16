using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CoreHtmlToImage
{
    /// <summary>
    /// Html Converter. Converts HTML string and URLs to image bytes
    /// </summary>
    public class HtmlConverter
    {
        private const string toolFilename = "wkhtmltoimage.exe";
        private static readonly string directory;
        private static readonly string toolFilepath;

        static HtmlConverter()
        {
            directory = AppContext.BaseDirectory;
            toolFilepath = Path.Combine(directory, toolFilename);

            if (!File.Exists(toolFilepath))
            {
                var assembly = typeof(HtmlConverter).GetTypeInfo().Assembly;
                var type = typeof(HtmlConverter);
                var ns = type.Namespace;

                using (var resourceStream = assembly.GetManifestResourceStream($"{ns}.{toolFilename}"))
                using (var fileStream = File.OpenWrite(toolFilepath))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }
        }

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
            var imageFormat = format.ToString().ToLower();
            var filename = Path.Combine(directory, $"{Guid.NewGuid().ToString()}.{imageFormat}");

            string args;

            if (IsLocalPath(url))
            {
                args = $"--quality {quality} --width {width} -f {imageFormat} \"{url}\" \"{filename}\"";
            }
            else
            {
                args = $"--quality {quality} --width {width} -f {imageFormat} {url} \"{filename}\"";
            }

            Process process = Process.Start(new ProcessStartInfo(toolFilepath, args)
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

        private static bool IsLocalPath(string path)
        {
            if (path.StartsWith("http"))
            {
                return false;
            }

            return new Uri(path).IsFile;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception(e.Data);
        }
    }
}