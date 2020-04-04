using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Maps
{
    public static class CurrentSongsAvailable
    {
        public static string GetRandomAudio()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\";

            int GetDirectoriesLength = Directory.GetDirectories(path).Length;

            int GetAmountOfFolder = osu.Framework.Utils.RNG.Next(0, GetDirectoriesLength);

            if (GetDirectoriesLength == 0)
                return null;

            string GetFolder = Directory.GetDirectories(path)[GetAmountOfFolder];

            string[] GetAudioFile = Directory.GetFiles(GetFolder, "**.mp3");

            if (GetAudioFile.Length == 0)
                return null;

            return GetAudioFile[0];
        }
    }
}
