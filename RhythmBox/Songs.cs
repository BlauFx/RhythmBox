using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using RhythmBox.Window.Maps;
using osu.Framework.Graphics;

namespace RhythmBox.Window
{
    public static class Songs
    {
        public static string SongPath => $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}{Path.DirectorySeparatorChar}Songs{Path.DirectorySeparatorChar}";

        private static bool didRun;

        private static readonly List<MapPack> MapPack = new();
        
        public static List<MapPack> GetMapPacks()
        {
            if (didRun)
                return MapPack;
            
            var directories = Directory.GetDirectories(SongPath);
            
            for (int i = 0; i < directories.Length; i++)
            {
                var Files = Directory.GetFiles(directories[i], "*.ini", SearchOption.TopDirectoryOnly);
               
                if (Files.Length == 0)
                    continue;

                Map[] maps = new Map[Files.Length];

                for (int j = 0; j < Files.Length; j++)
                    maps[j] = new Map(Files[j]);

                MapPack.Add(new MapPack(maps));
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
        
        

        public static void GenerateSong(String song, GameHost host, AudioManager audio)
        {
            Assembly assembly = Assembly.LoadFrom("RhythmBox.Resources.dll");
            
            if (assembly == null)
                throw new Exception($"{nameof(assembly)} can not be null!");

            var lines = DefaultFolder.ReadLines(() => assembly.GetManifestResourceStream("RhythmBox.Resources.Template.ini"), Encoding.UTF8).ToList();

            lines[2] = "AFileName: " + Path.GetFileName(song);
            lines[8] = "BPM: 97"; //BPM

            IResourceStore<byte[]> store = new StorageBackedResourceStore(host.Storage);
            ITrackStore trackStore = audio.GetTrackStore(store);

            var track = trackStore.Get(song);
            track.Frequency.Value = 100;
            track.Looping = false;
            //This is really terrible done, I know, sigh. TODO: Find a better solution.
            track.Start();
            Task.Delay(100).GetAwaiter().GetResult();
            track.Stop();
            lines[15] = $"Timings: 0,{(int)track.Length}";
            int bpm_stuff = 97 * 4;
            for (int i = 0; i < ((int) track.Length) / bpm_stuff; i++)
            {
                var direction = (HitObject.DirectionEnum)osu.Framework.Utils.RNG.Next(0, 4);
                lines.Add($"{direction}, {i * bpm_stuff}, 1f");
            }
            
            new Map(lines).WriteToNewMap($"{song}.ini");
        }
    }
}
