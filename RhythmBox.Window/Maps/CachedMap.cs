using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace RhythmBox.Window.Maps
{
    public class CachedMap : Component
    {
        private ITrackStore trackStore;

        public Track Track { get; private set; }

        public Bindable<Track> BindableTrack { get; } = new Bindable<Track>();

        public Map Map { get; set; }

        private readonly Storage storage;

        public CachedMap(Storage storage) => this.storage = storage;

        [Resolved]
        private Gameini gameini { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager Audio) => trackStore = Audio?.GetTrackStore(new StorageBackedResourceStore(storage));

        public void Seek(double time)
        {
            if (Map == null) 
                return;

            var IsRunning = Track?.IsRunning;

            Track?.Stop();
            Track?.Seek(time);

            if (IsRunning.GetValueOrDefault())
                Track?.Start();
            else
                Track?.Stop();

            BindableTrack.Value = Track;
        }

        public void Play(double? time)
        {
            Seek(time.GetValueOrDefault());
            Track?.Start();
        }

        public void Play() => Play(Track?.CurrentTime);

        public void LoadTrackFile()
        {
            if (Map is null)
                return;

            string folder = Path.GetDirectoryName(Map.Path);
            Track = trackStore?.Get($"{folder}{Path.DirectorySeparatorChar}{Map.AFileName}");

            if (Track != null)
                Track.Volume.Value = gameini.Get<double>(SettingsConfig.Volume);
        }

        public void Stop() => Track?.Stop();
    }
}
