using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;
using System;
using System.Collections.Generic;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneVolume : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            { typeof(Volume) };

        [Resolved]
        private CachedMap cachedMap { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            cachedMap.Map = CurrentSongsAvailable.GetRandomMap();
            cachedMap.LoadTrackFile();

            Child = new Volume(cachedMap.BindableTrack)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
            };

            AddStep("Start", () => cachedMap.Play());
            AddStep("Stop", () => cachedMap.Stop());
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            (Child as Volume).ChangeVolume(true, e);

            return base.OnScroll(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            cachedMap.Stop();

            base.Dispose(isDisposing);
        }
    }
}
