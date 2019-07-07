using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;

namespace RhythmBox.Window.pending_files
{
    public class Logo : Container
    {
        private Sprite logo;
        public Texture texture;
        protected const float scale = 0.05f;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                logo = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Texture = texture,
                    Size = new osuTK.Vector2(1f),
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ScaleTo(this.Scale.X + scale, 500, Easing.Out);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ScaleTo(this.Scale.X - scale, 500, Easing.Out);
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            this.ScaleTo(this.Scale.X - (scale * 1.1f), 100, Easing.Out);
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            this.ScaleTo(this.Scale.X + (scale * 1.1f), 100, Easing.Out);
            return base.OnMouseUp(e);
        }
    }
}
