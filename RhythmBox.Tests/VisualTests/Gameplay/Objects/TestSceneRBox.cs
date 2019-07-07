using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace RhythmBox.Tests.VisualTests.Gameplay.Objects
{
    [TestFixture]
    public class TestSceneRBox : TestScene
    {
        private Box bx;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1f,1f),
                    Y = 0.05f,
                    RelativePositionAxes = Axes.Both,
                }
            };

            AddStep("Start", () =>
            {
                Add(bx = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 0.01f),
                    RelativePositionAxes = Axes.Both,
                });

                bx.FadeIn(100);
                bx.MoveToY(0f, 1500, Easing.InCirc);

                bx.MoveToY(-0.5f, 1500, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), 1500, Easing.InCirc);
                Scheduler.AddDelayed(() => Rip(0, 0), 1800);
            });
        }

        private void Rip(int clear, int expire)
        {
            bx.ClearTransformsAfter(clear);
            Scheduler.AddDelayed(() => bx.Expire(), expire);

            bx.Colour = Color4.Red;

            Scheduler.AddDelayed(() => bx.Colour = Color4.White, 50);

            bx.FadeOut(100);
            bx.ScaleTo(1.1f, 100, Easing.OutCirc);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.W)
            {
                if (bx.Y <= -0.5 + 0.05f)
                {
                    Logger.Log(bx.Y.ToString());
                }

                Rip(1500, 500);

            }
            return base.OnKeyDown(e);
        }
    }
}
