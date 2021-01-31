using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Mode.Standard.Animations;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHpBar : TestScene
    {
        private HPBar hpbar;
        private BindableFloat OldValue { get; set; } = new BindableFloat();

        private float oldValue;

        [BackgroundDependencyLoader]
        private void Load()
        {
            SetupGameHost();

            Child = hpbar = new HPBar
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Colour = Color4.AliceBlue,
            };

            OldValue.BindTo(hpbar.CurrentValue);
            OldValue.ValueChanged += (e) => oldValue = e.OldValue;

            AddStep("Set HP to 100%", () => hpbar.CurrentValue.Value = 1f); 
            AddUntilStep("Ensure HP is 100%", () => hpbar.CurrentValue.Value.Equals(1f));

            AddStep("Set HP to 50%", () => hpbar.CurrentValue.Value = 0.5f);
            AddUntilStep("Ensure HP is 50%", () => hpbar.CurrentValue.Value.Equals(0.5f));

            AddStep("Set HP to 0%", () => hpbar.CurrentValue.Value = 0f); 
            AddUntilStep("Ensure HP is 0%", () => hpbar.CurrentValue.Value.Equals(0f));

            AddStep("Increase HP by 10%", () => hpbar.CurrentValue.Value += 0.1f);
            AddUntilStep("Ensure HP is +10%", () => hpbar.CurrentValue.Value.Equals(oldValue += 0.1f));

            AddStep("Reduce HP by 10%", () => hpbar.CurrentValue.Value -= 0.1f);
            AddUntilStep("Ensure HP is -10%", () => hpbar.CurrentValue.Value.Equals(oldValue -= 0.1f));

            AddStep("Set HP to 100%", () => hpbar.CurrentValue.Value = 1f);
            AddUntilStep("Ensure HP is 100%", () => hpbar.CurrentValue.Value.Equals(1f));

            AddStep("Start to drain HP", () => hpbar.Drain(false));
            AddSliderStep("Adjust the value of the HPBar", 0f, 1f, 1f, (x) => hpbar.CurrentValue.Value = x);
            
            AddStep("Stop draining", () => hpbar.Drain(true));
        }
    }
}
