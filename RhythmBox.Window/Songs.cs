using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Maps;

namespace RhythmBox.Window
{
    public static class Songs
    {
        public static string SongPath => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + $"{Path.DirectorySeparatorChar}Songs{Path.DirectorySeparatorChar}";

        private static bool didRun;

        private static List<MapPack> MapPack = new List<MapPack>();
        
        public static List<MapPack> GetMapPacks()
        {
            if (didRun)
                return MapPack;
            
            var dirs = Directory.GetDirectories(SongPath);
            var dirlength = dirs.Length;
            
            for (int i = 0; i < dirlength; i++)
            {
                var Files  = Directory.GetFiles(dirs[i], "*.ini", SearchOption.TopDirectoryOnly);
                var FLength = Files.Length;
               
                if (FLength == 0)
                    continue;

                Map[] Maps = new Map[FLength];

                for (int j = 0; j < FLength; j++)
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
