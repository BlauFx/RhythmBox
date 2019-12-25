using Newtonsoft.Json;
using osu.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static System.IO.Directory;
using FileWebRequest = osu.Framework.IO.Network.FileWebRequest;

namespace RhythmBox.Window.Updater
{
    public class Update
    {
        public Update()
        {
            if (Exists(@$"{ExePath}/temp"))
                Delete(@$"{ExePath}/temp", true);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private const string URL = "https://api.github.com/repos/BlauFx/RhythmBox/releases";

        private string ExePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private List<Github_Releases> github_Releases;

        public async Task<bool> SearchAsyncForUpdates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response;

                client.DefaultRequestHeaders.Add("user-agent", ".");

                response = await client.GetAsync(URL);
                response.EnsureSuccessStatusCode();

                string responseStr = await response.Content.ReadAsStringAsync();
                github_Releases = JsonConvert.DeserializeObject<List<Github_Releases>>(responseStr);
            }
            catch
            {
                //It can fail due to no internet connection or being rate-limited
                return false;
            }

            return CheckNewVersionAvailable();
        }

        private bool CheckNewVersionAvailable()
        {
            var x = github_Releases.First(x => x.assets == x.assets);

            if (!(GetCurrentVersion().Equals(x.tag_name)))
            {
                return true;
            }

            return false;
        }

        //TODO:
        private string GetCurrentVersion()
        {
            return "v0.0";
        }

        public async void DownloadUpdate()
        {
            var x = github_Releases.First(x => x.assets == x.assets);
            Assets win_x64 = x.assets.FirstOrDefault(y => y.name.Equals("win-x64.zip"));
            Assets Updater = x.assets.FirstOrDefault(y => y.name.Equals("Updater.exe"));

            if (!Exists(@$"{ExePath}/temp"))
                CreateDirectory(@$"{ExePath}/temp");

            try
            {
                await new FileWebRequest(@$"{ExePath}/temp/Updater.exe", Updater.browser_download_url).PerformAsync();
                await new FileWebRequest(@$"{ExePath}/temp/win-x64.zip", win_x64.browser_download_url).PerformAsync();
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, LoggingTarget.Network, LogLevel.Error);
                return;
            }

            if (File.Exists(@$"{ExePath}/temp/win-x64.zip"))
            {
                using StreamWriter strmWriter = new StreamWriter(@$"{ExePath}/temp/win-x64.txt");
                strmWriter.WriteLine("Done");
            }
        }

        public async void ApplyUpdate()
        {
            if (File.Exists($"{ExePath}\\temp\\win-x64.txt"))
            {
                using StreamReader reader = new StreamReader($"{ExePath}\\temp\\win-x64.txt");
                if (!(reader.ReadLine() == "Done"))
                {
                    return;
                }
            }

            void CreateDir(string path)
            {
                if (!Exists(path))
                    CreateDirectory(path);
                else
                {
                    Delete(path, true);
                    CreateDirectory(path);
                }
            }

            CreateDir($"{ExePath}\\temp\\files");
            CreateDir($"{ExePath}\\temp\\old");
            
            int PID = Process.GetCurrentProcess().Id;
            string File_Location = ExePath;

            ZipFile.ExtractToDirectory($"{ExePath}\\temp\\win-x64.zip", $"{ExePath}\\temp\\files");

            new Process()
            {
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    Arguments = $"/C start {ExePath}\\temp\\Updater.exe {PID} {File_Location}",
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                },
            }.Start();

            await Task.Delay(2000); //Just wait a lil bit until the Process has started
            Environment.Exit(0);
        }
    }

    internal class Github_Releases
    {
        public string tag_name { get; set; }
        public List<Assets> assets { get; set; }
    }

    internal class Assets
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }

        public List<Assets> assets { get; set; }
    }
}
