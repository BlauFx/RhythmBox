using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Animations;
using System.Threading.Tasks;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHpBar : TestScene
    {
        private HpBar hpBar;

        private const float Duration = 100f;

        private const Easing easing = Easing.None;

        private bool CanContinue = false;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            SetupGameHost();

            Child = hpBar = new HpBar(1f)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                colour = Color4.AliceBlue,
            };

            AddStep("Set HP to 100%", () => ResizeHPBar(hpBar.BoxMaxValue, Duration, easing));

            AddUntilStep("Wait until HP is 100%", () => CanContinue);
            AddStep("Set HP to 50%", () => ResizeHPBar(hpBar.BoxMaxValue / 2, Duration, easing));

            AddUntilStep("Wait until HP is 50%", () => CanContinue);
            AddStep("Set HP to 0%", () => ResizeHPBar(0f, 1000, Easing.InOutSine));

            AddUntilStep("Wait until HP reduced by 10%", () => CanContinue);
            AddStep("Increase Hp by 10%", () => ResizeHPBar(hpBar.CurrentValue + (hpBar.BoxMaxValue / 10), Duration, easing));

            AddUntilStep("Wait until HP is 0%", () => CanContinue);
            AddStep("Reduce Hp by 10%", () => ResizeHPBar(hpBar.CurrentValue - (hpBar.BoxMaxValue / 10), Duration, easing));

            AddUntilStep("Wait until HP increased by 10%", () => CanContinue);
            AddStep("Set HP to 100%", () => ResizeHPBar(hpBar.BoxMaxValue, Duration, easing));

            AddUntilStep("Wait until HP is 100%", () => CanContinue);
            AddStep("Start to drain HP", () => DrainHP(hpBar.HP_Update, Easing.OutCirc));

            AddSliderStep("Adjust the value of the HPBar", 0f, 1f, 1f, (x) =>
            {
                hpBar.ResizeBox(x, Duration, easing);
            });
        }

        private async void ResizeHPBar(float value, float duration, Easing easing)
        {
            CanContinue = false;

            hpBar.ResizeBox(value, duration, easing);

            await Task.Delay((int)duration);
            CanContinue = true;
        }

        private void DrainHP(float duration, Easing easing)
        {
            Scheduler.AddDelayed(() =>
            {
                ResizeHPBar(hpBar.CalcHpBarValue(hpBar.CurrentValue, hpBar.BoxMaxValue, 0f, Hit.Hit100, true), duration, easing);
            }, duration, true);
        }
    }
}
