using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Tests.Objects;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    [TestFixture]
    public class TestSceneTestModOverlay : TestScene
    {
        private TestSceneModOverlay testSceneModOverlay;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                testSceneModOverlay = new TestSceneModOverlay
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
                new Box
                {
                    Depth = 0,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                }
            };

            testSceneModOverlay.State.Value = Visibility.Hidden;

            AddWaitStep("Load", 1);

            AddStep("Animation 1 FadeIn", () =>
            {
                testSceneModOverlay.State.Value = Visibility.Visible;
            });

            AddWaitStep("wait for complete", 10);

            AddStep("Animation 1 FadeOut", () =>
            {
                testSceneModOverlay.State.Value = Visibility.Hidden;
            });
        }
    }

    public class TestSceneModOverlay : FocusedOverlayContainer
    {
        public TextFlowContainer _text;

        private Box box;

        public TestMods modBox;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                box = new Box
                {
                    Depth = 0,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Black.Opacity(0.3f),
                    Alpha = 0f,
                },
                _text = new TextFlowContainer
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextAnchor = Anchor.Centre,
                    Spacing = new Vector2(2f),
                    AutoSizeAxes = Axes.Both,
                    Alpha = 1f,
                },
                modBox = new TestMods
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f, 0.3f),
                    Colour = Color4.Green.Opacity(0.8f),
                    Alpha = 0f,
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.G)
            {
                this.State.Value = Visibility.Hidden;
            }

            return base.OnKeyDown(e);
        }

        protected override void PopIn()
        {
            AnimationIn();
            base.PopIn();
        }

        protected override void PopOut()
        {
            AnimationOut();
            base.PopOut();
        }

        public void Reset()
        {
            _text.Text = string.Empty;
            _text.MoveTo(new Vector2(0f));
        }

        public void AnimationIn()
        {
            Reset();
            this.FadeInFromZero(100, Easing.In);
            box.FadeInFromZero(100, Easing.In);
            _text.AddText("Here are some mods!", x => x.Font = new FontUsage("Roboto", 100));
            _text.Scale = new Vector2(0f);
            _text.FadeInFromZero(500, Easing.InBack);
            _text.ScaleTo(1f, 2000, Easing.OutElastic);
            Scheduler.AddDelayed(() => _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In), 700);

            Scheduler.AddDelayed(() => modBox.FadeInFromZero(100, Easing.In), 1200);
        }

        public void AnimationOut()
        {
            this.FadeOutFromOne(1000, Easing.In);
            box.FadeInFromZero(0, Easing.In);
            _text.FadeOutFromOne(500, Easing.OutBack);
            _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In);
        }
    }
}
