using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window
{
    public class ScrollContainer : ScrollContainer<Drawable>
    {
        public ScrollContainer(Direction scrollDirection = Direction.Vertical)
            : base(scrollDirection)
        {
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new MyScrollbar(direction);

        private class MyScrollbar : ScrollbarContainer
        {
            private const float dim_size = 10;

            private readonly Color4 hoverColour = Color4.White;
            private readonly Color4 defaultColour = Color4.Gray;
            private readonly Color4 highlightColour = Color4.Gray; //Color4.Black;

            private readonly Box box;

            public MyScrollbar(Direction scrollDir)
                : base(scrollDir)
            {
                Colour = defaultColour;

                Blending = BlendingParameters.Additive;

                CornerRadius = 5;

                const float margin = 3;

                Margin = new MarginPadding
                {
                    Left = scrollDir == Direction.Vertical ? margin : 0,
                    Right = scrollDir == Direction.Vertical ? margin : 0,
                    Top = scrollDir == Direction.Horizontal ? margin : 0,
                    Bottom = scrollDir == Direction.Horizontal ? margin : 0,
                };

                Masking = true;
                Child = box = new Box { RelativeSizeAxes = Axes.Both };

                ResizeTo(1);
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                Vector2 size = new Vector2(dim_size)
                {
                    [(int)ScrollDirection] = val
                };
                this.ResizeTo(size, duration, easing);
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(hoverColour, 100);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(defaultColour, 100);
            }

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if (!base.OnMouseDown(e)) return false;

                //note that we are changing the colour of the box here as to not interfere with the hover effect.
                box.FadeColour(highlightColour, 100);
                return true;
            }

            protected override void OnDrag(DragEvent e)
            {
                //    //Logger.Log(e.MousePosition.Y.ToString());

                base.OnDrag(e);
            }

            protected override void OnMouseUp(MouseUpEvent e)
            {
                //    if (e.Button != MouseButton.Left) return false;

                //    box.FadeColour(Color4.White, 100);

                base.OnMouseUp(e);
            }
        }
    }
}
