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
using RhythmBox.Window.Objects;
using RhythmBox.Window.Overlays;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    [TestFixture]
    public class TestSceneModOverlay : TestScene
    {
        private ModOverlay modOverlay;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                modOverlay = new ModOverlay
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

            modOverlay.State.Value = Visibility.Hidden;

            AddStep("Animation 1 FadeIn", () =>
            {
                modOverlay.State.Value = Visibility.Visible;
            });

            AddWaitStep("wait for complete", 10);

            AddStep("Animation 1 FadeOut", () =>
            {
                modOverlay.State.Value = Visibility.Hidden;
            });
        }
    }
}
