using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Animations;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHpBar : TestScene
    {
        private HpBar hpBar;

        private const float duration = 1000f;

        [BackgroundDependencyLoader]
        private async void Load()
        {
            Child = hpBar = new HpBar(0.4f)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                colour = Color4.AliceBlue,
            };

            AddStep("Set HPBar to 100%", () =>
            {
                hpBar.ResizeBox(0.4f, 1000, Easing.InOutSine);
            });

            AddStep("Set HPBar to 0%", () =>
            {
                hpBar.ResizeBox(0.0001f, 1000, Easing.InOutSine);
            });

            AddStep("Reduce Hp by 10%", () =>
            {
                hpBar.ResizeBox(hpBar.GetCurrentValue - (hpBar.BoxMaxValue / 10), duration, Easing.OutCirc);
            });

            AddStep("Increase Hp by 10%", () =>
            {
                hpBar.ResizeBox(hpBar.CurrentValue + (hpBar.BoxMaxValue / 10), duration, Easing.OutCirc);
            });
        }
    }
}
