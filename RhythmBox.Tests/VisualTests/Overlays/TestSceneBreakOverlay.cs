using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Overlays;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    [TestFixture]
    public class TestSceneBreakOverlay : TestScene
    {
        private BreakOverlay breakOverlay;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                breakOverlay = new BreakOverlay
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

            breakOverlay.State.Value = Visibility.Hidden;

            AddWaitStep("Load", 1);
            AddStep("FadeIn", () => breakOverlay.State.Value = Visibility.Visible);

            AddWaitStep("wait for complete", 10);
            AddStep("FadeOut", () => breakOverlay.State.Value = Visibility.Hidden);
        }
    }
}
