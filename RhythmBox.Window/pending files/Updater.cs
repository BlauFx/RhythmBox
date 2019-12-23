using osu.Framework.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace RhythmBox.Window.pending_files
{
    public class Updater
    {
        private HttpClient client = new HttpClient();
        private HttpResponseMessage response;
        private string responseStr = string.Empty;

        private const string URL = "";

        public async Task<bool> SearchAsyncForUpdates()
        {
            try
            {
                //response = await client.GetAsync("URL");
                //response.EnsureSuccessStatusCode();
                //responseStr = await response.Content.ReadAsStringAsync();
            }
            finally
            {
                //response.Dispose();
                //client.Dispose();
            }

            return CheckNewVersionAvailable();
        }

        private bool CheckNewVersionAvailable()
        {
            if (GetCurrentVersion() != "0.0.0")
            {
                return true;
            }

            return false;
        }

        private string GetCurrentVersion()
        {
            return "0.0.0";
        }

        public bool DownloadUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                if (!Directory.Exists(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp"))
                {
                    Directory.CreateDirectory(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp");
                }

                wc.Proxy = null;
                wc.Headers.Add("user-agent", " ");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    wc.DownloadFile(new Uri(URL), @$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.zip");
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message, LoggingTarget.Network, LogLevel.Error);
                    return false;
                }
            }

            if (File.Exists(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.zip"))
            {
                using StreamWriter strmWriter = new StreamWriter(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.txt");
                strmWriter.WriteLine("Done");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ApplyUpdate()
        {
            if (File.Exists(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.txt"))
            {
                using StreamReader reader = new StreamReader(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.txt");
                if (!(reader.ReadLine() == "Done"))
                {
                    return;
                }
            }
            if (!Directory.Exists(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/files"))
            {
                Directory.CreateDirectory(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/files");
            }
            else
            {
                Directory.Delete(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file");
                Directory.CreateDirectory(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/files");
            }

            ZipFile.ExtractToDirectory(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.zip", @$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/files");

            var proc = new Process();

            proc.StartInfo.FileName = "updater.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;

            proc.Start();
            var sw = proc.StandardInput;

            int PID = Process.GetCurrentProcess().Id;
            string File_Location = @$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/files";

            sw.Write($"{PID} {File_Location}");
            sw.Close();
            
            Environment.Exit(0);
        }

        private void RemoveOldUpdate()
        {

        }
    }
}
