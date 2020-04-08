using RhythmBox.Mode.Std.Maps;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Maps
{
    public static class CurrentSongsAvailable
    {
        private static string SongPath => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs";

        private static int GetDirectoriesLength => Directory.GetDirectories(SongPath).Length;

        //TODO:
        public static Map GetRandomMap()
        {
            List<string> MapsAvailable = new List<string>();

            foreach (var dirs in Directory.GetDirectories(SongPath))
            {
                int count = Directory.GetFiles(dirs, "*.ini", SearchOption.TopDirectoryOnly).Length;
                if (count > 0)
                    MapsAvailable.Add(dirs);
            }

            int[] MapsCount2 = new int[MapsAvailable.Count];
            Map[,] Maps = new Map[MapsCount2.Length, 5]; //TODO: For now let's allow only 5 maps per pack

            for (int i = 0; i < MapsAvailable.Count; i++)
            {
                MapsCount2[i] = Directory.GetFiles(MapsAvailable[i], "*.ini", SearchOption.TopDirectoryOnly).Length;

                if (MapsCount2[i] > 0)
                {
                    DirectoryInfo d = new DirectoryInfo(MapsAvailable[i]);
                    FileInfo[] Files = d.GetFiles("*.ini");

                    for (int j = 0; j < Files.Length; j++)
                    {
                        Maps[i, j] = new Map(Files[j].FullName);
                    }
                }
            }

            int GetRandomMap = osu.Framework.Utils.RNG.Next(0, new DirectoryInfo(MapsAvailable[osu.Framework.Utils.RNG.Next(0, MapsAvailable.Count)]).GetFiles("*.ini").Length);

            return Maps[GetRandomMap, 0];
        }
    }
}
