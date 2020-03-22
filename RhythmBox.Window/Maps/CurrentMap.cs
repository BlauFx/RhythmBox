using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using RhythmBox.Mode.Std.Maps;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RhythmBox.Window.Maps
{
    public class CurrentMap
    {
        private AudioManager audio { get; set; }

        private ITrackStore trackStore;

        private IResourceStore<byte[]> store;

        private static Track track;

        public Map Map { get; set; } = null;

        public CurrentMap(Storage storage)
        {
            store = new StorageBackedResourceStore(storage);
        }

        public void Seek(double time)
        {
            if (Map is null) return;

            trackStore = audio?.GetTrackStore(store);

            track?.Stop();
            track = trackStore?.Get($"{Map.Path.Substring(0, Map.Path.LastIndexOf(@"\"))}\\{Map.AFileName}");
            track.Volume.Value = 0.2d;

            track?.Seek(time);
        }

        public void Play(AudioManager audio, double time) 
        {
            this.audio = audio;

            Seek(time);
            track?.Start();
        }

        public void Stop() => track?.Stop();
    }
}
