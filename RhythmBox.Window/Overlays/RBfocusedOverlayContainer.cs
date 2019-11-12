using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window.Overlays
{
    public class RBfocusedOverlayContainer : OverlayContainer
    {
        private Box box;

        public RBfocusedOverlayContainer(Color4 color4, bool InstantVis = false)
        {
            Add(new Box
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Colour = color4,
                Alpha = 1f,
            });
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            box = new Box
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.4f),
                Colour = Color4.White.Opacity(0.2f),
                Alpha = 1f,
            };

            Add(box);
        }


        protected override void PopIn()
        {
            this.State.Value = Visibility.Visible;
            this.FadeIn(100, Easing.In);
        }

        protected override void PopOut()
        {
            this.State.Value = Visibility.Hidden;
            this.FadeOut(100, Easing.In);
        }
    }
}
