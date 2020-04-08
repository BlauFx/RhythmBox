using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneMusicCircular : TestScene
    {
        [Resolved]
        private CachedMap cachedMap { get; set; }

        private const int barWidth = 3;

        private const int Degree = 360;

        private const int DefaultRadius = 360;

        private const int AmountOfBars = 200;

        [BackgroundDependencyLoader]
        private void Load()
        {
            cachedMap.Map = CurrentSongsAvailable.GetRandomMap();
            cachedMap.LoadTrackFile();

            Child = new MusicVisualizationCircular(Degree, barWidth, AmountOfBars, DefaultRadius, cachedMap.BindableTrack);

            AddAssert("Check if audio has been loaded", () => { return (this.Child as MusicVisualizationCircular).AudioHasLoaded; });

            AddSliderStep("Adjust Radius", 0, 600, DefaultRadius, (x) => (this.Child as MusicVisualizationCircular).CircleSize = x);
            AddSliderStep("Adjust Degree", 0, 600, Degree, (x) => (this.Child as MusicVisualizationCircular).Degree = x);
            AddSliderStep("Adjust AmountOfBars", 0, 600, AmountOfBars, (x) => (this.Child as MusicVisualizationCircular).AmountOfBars = x);
            AddSliderStep("Adjust BarWidth", 0, 15f, 4f, (x) => (this.Child as MusicVisualizationCircular).BarWidth = x);
            AddSliderStep("Adjust Intensivity", 0, 1000, 400, (x) => (this.Child as MusicVisualizationCircular).Intensivity = x);

            AddStep("Start", () => cachedMap.Play());
            AddStep("Reverse", () => (this.Child as MusicVisualizationCircular).IsReversed = !(this.Child as MusicVisualizationCircular).IsReversed);
            AddStep("Stop", () => cachedMap.Stop());
        }

        protected override void Dispose(bool isDisposing) => cachedMap.Stop();
    }
}
