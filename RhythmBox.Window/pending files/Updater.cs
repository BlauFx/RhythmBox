using osu.Framework.Logging;
using System;
using System.IO;
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

        public bool PrepareUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                if (!Directory.Exists(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp"))
                {
                    Directory.CreateDirectory(@$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp");
                }

                wc.Proxy = null;
                wc.Headers.Add("user-agent", "Only a test!");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    //wc.DownloadFile(new Uri(@"URL"), @$"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/temp/file.zip");
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

        }

        private void RemoveOldUpdate()
        {

        }
    }
}
