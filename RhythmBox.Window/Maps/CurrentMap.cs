using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using RhythmBox.Mode.Std.Maps;

namespace RhythmBox.Window.Maps
{
    public class CurrentMap
    {
        private AudioManager audio { get; set; }

        private ITrackStore trackStore;

        private IResourceStore<byte[]> store;

        public Track track { get; set; }

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
