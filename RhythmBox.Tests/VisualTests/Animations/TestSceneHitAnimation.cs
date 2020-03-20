using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using RhythmBox.Mode.Std.Animations;
using System.Threading.Tasks;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHitAnimation : TestScene
    {
        private HitAnimation hitAnimation { get; set; }

        private bool CanContinue = false;

        [BackgroundDependencyLoader]
        private async void Load()
        {
            Child = hitAnimation = new HitAnimation(Hit.Hit300, true)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
            };
            
            AddStep("Start hit300", async () => Wait(Hit.Hit300));

            AddUntilStep("Wait until hit300 finished", () => CanContinue);
            AddStep("Start hit100", async () => Wait(Hit.Hit100));

            AddUntilStep("Wait until hit100 finished", () => CanContinue);
            AddStep("Start hit50", async () => Wait(Hit.Hit50));

            AddUntilStep("Wait until hit50 finished", () => CanContinue);
            AddStep("Start hitx", async () => Wait(Hit.Hitx));
        }

        private async void Wait(Hit hit)
        {
            CanContinue = false;

            hitAnimation.Hit = hit;
            hitAnimation.LoadAndPrepareHitSpirte();

            await Task.Delay(hitAnimation.WaitTime);
            CanContinue = true;
        }
    }
}
