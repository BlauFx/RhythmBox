using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RhythmBox.Window.Maps;

namespace RhythmBox.Window
{
    public static class Songs
    {
        public static string SongPath => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + $"{Path.DirectorySeparatorChar}Songs{Path.DirectorySeparatorChar}";

        private static bool didRun;

        private static readonly List<MapPack> MapPack = new List<MapPack>();
        
        public static List<MapPack> GetMapPacks()
        {
            if (didRun)
                return MapPack;
            
            var directories = Directory.GetDirectories(SongPath);
            
            for (int i = 0; i < directories.Length; i++)
            {
                var Files  = Directory.GetFiles(directories[i], "*.ini", SearchOption.TopDirectoryOnly);
               
                if (Files.Length == 0)
                    continue;

                Map[] Maps = new Map[Files.Length];

                for (int j = 0; j < Files.Length; j++)
                    Maps[j] = new Map(Files[j]);

                MapPack.Add(new MapPack(Maps));
            }

            if (MapPack.Count > 0)
                didRun = true;
            
            return MapPack;
        }
        
        public static Map GetRandomMap()
        {
            var mapPacks = GetMapPacks();

            if (mapPacks.Count == 0)
                return null;

            var getRandomMapPack = mapPacks[osu.Framework.Utils.RNG.Next(0, mapPacks.Count)];
            return getRandomMapPack.Maps[osu.Framework.Utils.RNG.Next(0, getRandomMapPack.Maps.Length)];
        }
    }
}
