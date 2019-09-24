using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Mode.Std.Tests.Animations
{
    public class HPBar : CompositeDrawable
    {
        public Box _box;

        public float BoxMaxValue = 0.4f;

        public float CurrentValue => _box.Width;

        public Color4 colour;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            InternalChild = _box = new Box
            {
                Depth = 0,
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
            _box.ResizeWidthTo(value, duration, easing);
        }
    }
}
