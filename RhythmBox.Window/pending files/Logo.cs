using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace RhythmBox.Window.pending_files
{
    public class Logo : Container
    {
        private Sprite logo;
        public Texture texture;
        protected const float scale = 0.05f;

        private bool scaling = false;

        private bool logoMoved = false;

        public Action ClickAction;

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
            if (!scaling)
            {
                this.ScaleTo(this.Scale.X + scale, 100, Easing.Out);
                Scheduler.AddDelayed(() => scaling = true, 100);
            }
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (scaling)
            {
                this.ScaleTo(this.Scale.X - scale, 100, Easing.Out);
                Scheduler.AddDelayed(() => scaling = false, 100);
            }
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            ClickAction?.Invoke();

            if (!logoMoved)
            {
                logoMoved = true;
                this.MoveToOffset(new Vector2(-0.13f, 0), 500, Easing.In);
            }
            this.ScaleTo(0.92f, 100, Easing.Out);
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            this.ScaleTo(1f, 100, Easing.Out);
            return base.OnMouseUp(e);
        }
    }
}
