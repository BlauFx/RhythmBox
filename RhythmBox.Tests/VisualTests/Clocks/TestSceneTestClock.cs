using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Tests.Clock;
using RhythmBox.Tests.pending_files;

namespace RhythmBox.Tests.VisualTests.Clock
{
    [TestFixture]
    public class TestSceneTestClock : TestScene
    {
        private SpriteText sptText;

        private TestSceneMap map;

        private TestSceneRbPlayfield playfield;

        private TestSceneRhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        public readonly BindableBool IsPaused = new BindableBool();

        private bool Resuming { get; set; } = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            map = new TestSceneMap
            {
                AFileName = "null",
                BGFile = "none",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Objects = 10,
                AutoMap = false,
                Mode = RhythmBox.Mode.Std.Tests.Interfaces.GameMode.STD,
                Title = "Test Title",
                Artist = "Test Artist",
                Creator = "Test Creator",
                DifficultyName = "Test DifficultyName",
                StartTime = 100,
                EndTime = 1000
            };

            map.HitObjects = new Mode.Std.Tests.Interfaces.HitObjects[1];
            map.HitObjects[0] = new RhythmBox.Mode.Std.Tests.Interfaces.HitObjects();
            map.HitObjects[0]._direction = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects.Direction.Up;
            map.HitObjects[0].Speed = 2f;
            map.HitObjects[0].Time = 200;

            Children = new Drawable[]
            {
                sptText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "0",
                },
                rhythmBoxClockContainer = new TestSceneRhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f)
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                playfield = new TestSceneRbPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = map,
                },
            };

            rhythmBoxClockContainer.IsPaused.BindTo(IsPaused);
            rhythmBoxClockContainer.UserPlaybackRate.BindTo(UserPlaybackRate);

            playfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            rhythmBoxClockContainer.Start();
        }

        protected override void Update()
        {
            sptText.Text = rhythmBoxClockContainer.IsPaused.ToString();
            base.Update();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (Resuming)
            {
                Resuming = false;
                rhythmBoxClockContainer.Stop();
            }
            else
            {
                Resuming = true;
                rhythmBoxClockContainer.Start();
            }

            playfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            return base.OnKeyDown(e);
        }
    }
}
