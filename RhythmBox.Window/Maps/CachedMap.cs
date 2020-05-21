using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Maps
{
    public class CachedMap : Component
    {
        private ITrackStore trackStore;

        public Track track { get; set; }

        public Bindable<Track> BindableTrack { get; } = new Bindable<Track>();

        public Map Map { get; set; } = null;

        private Storage storage;

        public CachedMap(Storage storage) => this.storage = storage;

        [Resolved]
        private Gameini gameini { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager Audio)
        {
            trackStore = Audio?.GetTrackStore(new StorageBackedResourceStore(storage));
        }

        public void Seek(double time)
        {
            if (Map == null) return;

            var IsRunning = track.IsRunning;

            track?.Stop();
            track?.Seek(time);

            if (IsRunning)
                track?.Start();
            else
                track?.Stop();

            BindableTrack.Value = track;
        }

        public void Play(double time)
        {
            Seek(time);
            track?.Start();
        }

        public void Play() => Play(track.CurrentTime);

        public void LoadTrackFile()
        {
            if (Map is null)
                return;

            track = trackStore?.Get($"{Map.Path.Substring(0, Map.Path.LastIndexOf(@"\"))}\\{Map.AFileName}");

            if (track != null)
                track.Volume.Value = gameini.Get<double>(SettingsConfig.Volume);
        }

        public void Stop() => track?.Stop();
    }
}
