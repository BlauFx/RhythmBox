using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using osu.Framework.Logging;

namespace RhythmBox.Tests.pending_files
{
    public class Check_Licenses
    {
        private static readonly string[] license = new string[3];
        private static readonly string[] license_in_folder = new string[license.Length];
        private static readonly string[] missing_licenses = new string[license.Length];
        private static int number;

        public static void License()
        {
            license[0] = "RhythmBox";
            license[1] = "osu-framework";
            license[2] = "Roboto-Font";

            number = Count_Licenses();

            if (!(license.Length.ToString() == number.ToString()))
            {
                Get_Missing_Licenses();
                Task.Run(() => Download_Licenses());
            }
        }
        private static int Count_Licenses()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Licenses");
            DirectoryInfo dir = new DirectoryInfo(path);

            if (dir.Exists)
            {
                int i = 0;
                if (dir.GetFiles("*.txt").Length > 0)
                {
                    foreach (var obj in dir.GetFiles("*.txt"))
                    {
                        for (int i2 = 0; i2 < license.Length; i2++)
                        {
                            if (obj.ToString().Contains(license[i2] + ".txt"))
                            {
                                license_in_folder[i2] = license[i2];
                            }
                            else
                            {
                                license_in_folder[i2] = string.Empty;
                            }
                        }
                        i++;
                    }
                }
                else
                {
                    for (int i2 = 0; i2 < license.Length; i2++)
                    {
                        license_in_folder[i2] = string.Empty;
                    }
                }
                return i;
            }
            return 0;
        }

        private static void Get_Missing_Licenses()
        {
            try
            {
                for (int i = 0; i < license.Length; i++)
                {
                    if (!(license_in_folder[i].Contains(license[i])))
                    {
                        missing_licenses[i] = license[i];
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString(), LoggingTarget.Runtime, LogLevel.Error);
            }
        }

        private static void Download_Licenses()
        {
            string download_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Licenses");
            Directory.CreateDirectory(download_path);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;

                int download_number = 0;
                Logger.Log("Start downloading missing licenses...", LoggingTarget.Information, LogLevel.Verbose);
                while (download_number < missing_licenses.Length)
                {
                    try
                    {
                        string file = wc.DownloadString(new Uri(string.Format("https://raw.githubusercontent.com/BlauFx/RhythmBox/master/Licenses/{0}.txt", missing_licenses[download_number])));
                        Logger.Log(string.Format(Path.Combine(download_path, "RhythmBox\\Licenses\\{0}.txt"), missing_licenses[download_number]));
                        using (StreamWriter strm = new StreamWriter(string.Format(Path.Combine(download_path, "{0}.txt"), missing_licenses[download_number])))
                        {
                            strm.WriteLine(file);
                        }
                    }
                    catch { }

                    download_number++;
                }
            }
        }
    }
}
