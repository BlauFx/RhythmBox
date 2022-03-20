using osu.Framework.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace RhythmBox.Window
{

    public static class License
    {
        public static void Licenses(string url, IEnumerable<string> licenses)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Licenses");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using HttpClient httpClient = new HttpClient();

            foreach (var t in licenses)
            {
                if (File.Exists($"{path}\\{t}.txt"))
                    continue;

                try
                {
                    using var fs = new FileStream($"{path}\\{t}.txt", FileMode.CreateNew);
                    httpClient.GetStreamAsync($"{url}/{t}.txt").Result.CopyTo(fs);
                }
                catch (Exception e)
                {
                    Logger.Log($"Class: License; Method: Licenses; Msg: {e.Message}", LoggingTarget.Runtime, LogLevel.Important);
                }
            }
        }
    }
}
