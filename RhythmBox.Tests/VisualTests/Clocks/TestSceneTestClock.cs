using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Playfield;

namespace RhythmBox.Tests.VisualTests.Clock
{
    [TestFixture]
    public class TestSceneTestClock : TestScene
    {
        private SpriteText sptText;

        private Map map;

        private Playfield playfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        public readonly BindableBool IsPaused = new BindableBool();

        private bool Resuming { get; set; } = true;

        private TextFlowContainer DispayCombo;

        [BackgroundDependencyLoader]
        private void Load()
        {
            map = new Map(null)
            {
                AFileName = "null",
                BGFile = "none",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Mode = GameMode.STD,
                Title = "Test Title",
                Artist = "Test Artist",
                Creator = "Test Creator",
                DifficultyName = "Test DifficultyName",
                StartTime = 100,
                EndTime = 1000
            };

            map.HitObjects = new HitObjects[1];
            map.HitObjects[0] = new HitObjects();
            map.HitObjects[0]._direction = HitObjects.Direction.Up;
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
                rhythmBoxClockContainer = new RhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                DispayCombo = new TextFlowContainer
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.BottomLeft,
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                playfield = new Playfield(null)
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
            DispayCombo.Text = rhythmBoxClockContainer.RhythmBoxClock.CurrentTime.ToString();
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
