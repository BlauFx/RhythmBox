using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneMusicLinear : TestScene
    {
        [Resolved]
        private CachedMap cachedMap { get; set; }

        private const int barWidth = 3;

        private const int AmountOfBars = 120;

        private const float Spacing = 0f;

        [BackgroundDependencyLoader]
        private void Load()
        {
            cachedMap.Map = CurrentSongsAvailable.GetRandomMap();
            cachedMap.LoadTrackFile();

            Child = new MusicVisualizationLinear(barWidth, AmountOfBars, Spacing, cachedMap.BindableTrack)
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

            AddStep("Start", () => cachedMap.Play());
            AddStep("Reverse", () => (this.Child as MusicVisualizationLinear).IsReversed = !(this.Child as MusicVisualizationLinear).IsReversed);
            AddStep("Stop", () => cachedMap.Stop());
        }

        protected override void Dispose(bool isDisposing) => cachedMap.Stop();
    }
}
