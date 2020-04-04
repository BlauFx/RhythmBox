using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;
using System.IO;
using System.Reflection;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneMusicCircular : TestScene
    {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Resolved]
        private GameHost Host { get; set; }

        private Track track;

        private const float barWidth = 3f;

        private const int Degree = 360;

        private const int DefaultRadius = 360;

        private const int AmountOfBars = 200;

        [BackgroundDependencyLoader]
        private void Load()
        {
            track = Audio.GetTrackStore(new StorageBackedResourceStore(Host.Storage)).Get(CurrentSongsAvailable.GetRandomAudio());
            track.Volume.Value = 0.1d;

            Child = new MusicVisualizationCircular(Degree, barWidth, AmountOfBars, DefaultRadius, new Bindable<Track>(track));

            AddAssert("Check if audio has been loaded", () => { return (this.Child as MusicVisualizationCircular).AudioHasLoaded; });

            AddSliderStep("Adjust Radius", 0, 600, DefaultRadius, (x) => (this.Child as MusicVisualizationCircular).CircleSize = x);
            AddSliderStep("Adjust Degree", 0, 600, Degree, (x) => (this.Child as MusicVisualizationCircular).Degree = x);
            AddSliderStep("Adjust AmountOfBars", 0, 600, AmountOfBars, (x) => (this.Child as MusicVisualizationCircular).AmountOfBars = x);
            AddSliderStep("Adjust BarWidth", 0, 15f, 4f, (x) => (this.Child as MusicVisualizationCircular).BarWidth = x);
            AddSliderStep("Adjust Intensivity", 0, 1000, 400, (x) => (this.Child as MusicVisualizationCircular).Intensivity = x);

            AddStep("Start", () => track.Start());
            AddStep("Reverse", () => (this.Child as MusicVisualizationCircular).IsReversed = !(this.Child as MusicVisualizationCircular).IsReversed);
            AddStep("Stop", () => track.Stop());
        }

        protected override void Dispose(bool isDisposing) => track?.Stop();
    }
}
