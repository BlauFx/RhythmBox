using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Maps
{
    public static class CurrentSongsAvailable
    {
        public static string GetRandomAudio()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\";

            int GetAmountOfFolder = osu.Framework.Utils.RNG.Next(0, Directory.GetDirectories(path).Length);
            string GetFolder = Directory.GetDirectories(path)[GetAmountOfFolder];

            string[] GetAudioFile = Directory.GetFiles(GetFolder, "**.mp3");
            return GetAudioFile[0];
        }
    }
}
