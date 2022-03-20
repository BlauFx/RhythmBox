using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneVolume : TestScene
    {
        [Resolved]
        private CachedMap cachedMap { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            cachedMap.Map = Songs.GetRandomMap();
            cachedMap.LoadTrackFile();

            Child = new Volume(cachedMap.BindableTrack)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.7f)
            };

            AddStep("Start", () => cachedMap.Play());
            AddStep("Stop", () => cachedMap.Stop());
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            (Child as Volume)?.ChangeVolume(e);
            return base.OnScroll(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            cachedMap.Stop();
            base.Dispose(isDisposing);
        }
    }
}
