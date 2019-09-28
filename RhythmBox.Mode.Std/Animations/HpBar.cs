using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Mode.Std.Animations
{
    public class HpBar : Container
    {
        private Box _box;

        public readonly float BoxMaxValue;

        public float CurrentValue { get; set; } = 0f;

        public Color4 colour;

        public HpBar(float BoxMaxValue)
        {
            this.BoxMaxValue = BoxMaxValue;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = _box = new Box
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativePositionAxes = Axes.Both,
                Alpha = 1f,
                Colour = colour,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(BoxMaxValue, 0.04f),
                X = 0f,
                Y = 0f,
            };
        }

        public void ResizeBox(float value, double duration, Easing easing)
        {
            if (value > BoxMaxValue || value < -0.0001f)
            {
                return;
            }

            CurrentValue = value;

            _box.ResizeWidthTo(value, duration, easing);
        }
    }
}
