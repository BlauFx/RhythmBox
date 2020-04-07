using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
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
        private AudioManager Audio { get; set; }

        [Resolved]
        private GameHost Host { get; set; }

        private Track track;

        [BackgroundDependencyLoader]
        private void Load()
        {
            track = Audio.GetTrackStore(new StorageBackedResourceStore(Host.Storage)).Get(CurrentSongsAvailable.GetRandomAudio());
            track.Volume.Value = 1d;

            Child = new Volume(new Bindable<Track>(track))
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
            };

            AddStep("Start", () => track.Start());
            AddStep("Stop", () => track.Stop());
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            (Child as Volume).ChangeVolume(true, e);

            return base.OnScroll(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();

            base.Dispose(isDisposing);
        }
    }
}
