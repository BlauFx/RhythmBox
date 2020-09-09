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
        private HPBar hpbar;

        private const float Duration = 100f;

        private const Easing easing = Easing.None;

        private bool CanContinue = false;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            SetupGameHost();

            Child = hpbar = new HPBar(1f)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                colour = Color4.AliceBlue,
            };

            AddStep("Set HP to 100%", () => ResizeHPBar(hpbar.BoxMaxValue, Duration, easing));

            AddUntilStep("Wait until HP is 100%", () => CanContinue);
            AddStep("Set HP to 50%", () => ResizeHPBar(hpbar.BoxMaxValue / 2, Duration, easing));

            AddUntilStep("Wait until HP is 50%", () => CanContinue);
            AddStep("Set HP to 0%", () => ResizeHPBar(0f, 1000, Easing.InOutSine));

            AddUntilStep("Wait until HP reduced by 10%", () => CanContinue);
            AddStep("Increase Hp by 10%", () => ResizeHPBar(hpbar.CurrentValue.Value + (hpbar.BoxMaxValue / 10), Duration, easing));

            AddUntilStep("Wait until HP is 0%", () => CanContinue);
            AddStep("Reduce Hp by 10%", () => ResizeHPBar(hpbar.CurrentValue.Value - (hpbar.BoxMaxValue / 10), Duration, easing));

            AddUntilStep("Wait until HP increased by 10%", () => CanContinue);
            AddStep("Set HP to 100%", () => ResizeHPBar(hpbar.BoxMaxValue, Duration, easing));

            AddUntilStep("Wait until HP is 100%", () => CanContinue);
            AddStep("Start to drain HP", () => hpbar.DrainHP());

            AddSliderStep("Adjust the value of the HPBar", 0f, 1f, 1f, (x) => hpbar.ResizeBox(x, Duration, easing));
        }

        private async void ResizeHPBar(float value, float duration, Easing easing)
        {
            CanContinue = false;

            hpbar.ResizeBox(value, duration, easing);

            await Task.Delay((int)duration);
            CanContinue = true;
        }
    }
}
