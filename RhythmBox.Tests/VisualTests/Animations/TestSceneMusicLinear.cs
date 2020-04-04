using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneMusicLinear : TestScene
    {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Resolved]
        private GameHost Host { get; set; }

        private Track track;

        private const float barWidth = 3f;

        private const int AmountOfBars = 120;

        private const float Spacing = 0f;

        [BackgroundDependencyLoader]
        private void Load()
        {
            track = Audio.GetTrackStore(new StorageBackedResourceStore(Host.Storage)).Get(CurrentSongsAvailable.GetRandomAudio());
            track.Volume.Value = 0.1d;

            Child = new MusicVisualizationLinear(barWidth, AmountOfBars, Spacing, new Bindable<Track>(track))
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(.4f),  
            };

            AddAssert("Check if audio has been loaded", () => { return (this.Child as MusicVisualizationLinear).AudioHasLoaded; });

            AddSliderStep("Adjust Spacing", 0, 8f, Spacing, (x) => (this.Child as MusicVisualizationLinear).Spacing = x);
            AddSliderStep("Adjust AmountOfBars", 0, 256, AmountOfBars, (x) => (this.Child as MusicVisualizationLinear).AmountOfBars = x);
            AddSliderStep("Adjust BarWidth", 1f, 15f, 4f, (x) => (this.Child as MusicVisualizationLinear).BarWidth = x);
            AddSliderStep("Adjust Intensivity", 0, 1000, 400, (x) => (this.Child as MusicVisualizationLinear).Intensivity = x);

            AddStep("Start", () => track.Start());
            AddStep("Reverse", () => (this.Child as MusicVisualizationLinear).IsReversed = !(this.Child as MusicVisualizationLinear).IsReversed);
            AddStep("Stop", () => track.Stop());
        }

        protected override void Dispose(bool isDisposing) => track?.Stop();
    }
}
