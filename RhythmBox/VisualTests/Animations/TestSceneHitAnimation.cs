using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using RhythmBox.Window.Animation;

namespace RhythmBox.Window.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHitAnimation : TestScene
    {
        private HitAnimation hitAnimation { get; set; }

        private bool CanContinue;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = hitAnimation = new HitAnimation(Hit.Hit300, true)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
            };
            
            AddStep("Start hit 300", () => Wait(Hit.Hit300));
            AddUntilStep("Wait until hit 300 finished", () => CanContinue);
            AddStep("Start hit 100", () => Wait(Hit.Hit100));
            AddUntilStep("Wait until hit 100 finished", () => CanContinue);
            AddStep("Start hit x", () => Wait(Hit.Hitx));
        }

        private async void Wait(Hit hit)
        {
            CanContinue = false;

            hitAnimation.Hit = hit;
            hitAnimation.LoadAndPrepareHitSprite();

            await Task.Delay(hitAnimation.WaitTime);
            CanContinue = true;
        }
    }
}
