using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osu.Framework.Timing;

namespace RhythmBox.Tests.VisualTests.Clock
{
    [TestFixture]
    public class TestSceneTestClock : TestScene
    {
        private SpriteText sptText;
        StopwatchClock clock = new StopwatchClock();

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                sptText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "0",
                }
            };
            clock.Start();

            AddStep("Seek 10sec forward", () =>
            {
                clock.Seek(clock.CurrentTime + 10000);
            });
        }

        protected override void Update()
        {
            sptText.Text = clock.CurrentTime.ToString();
            base.Update();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (clock.IsRunning)
            {
                clock.Stop();
            }
            else
            {
                clock.Start();
            }

            return base.OnKeyDown(e);
        }
    }
}
