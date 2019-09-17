using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneScrollContainer : ScrollContainer<Drawable>
    {
        private float Offset { get; set; }

        public TestSceneScrollContainer(Direction scrollDirection = Direction.Vertical)
            : base(scrollDirection)
        {
        }

        protected override void LoadComplete()
        {
            base.DistanceDecayScroll = 0.01d;
            base.ScrollDistance = 80f;
            base.ClampExtension = 0f;

            base.LoadComplete();
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            Vector2 scrollDelta = e.ScrollDelta;
            float scrollDeltaFloat = scrollDelta.Y;

            if (ScrollDirection == Direction.Horizontal && scrollDelta.X != 0)
            {
                scrollDeltaFloat = scrollDelta.X;
            }

            offset(base.ScrollDistance * -scrollDeltaFloat, true, base.DistanceDecayScroll);
            return true;
        }

        private void offset(float value, bool animated, double distanceDecay)
        {
            Offset += value;
            ScrollTo(Offset + value, animated, distanceDecay);
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new MyScrollbar(direction);

        protected class MyScrollbar : ScrollbarContainer
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

            protected override bool OnDrag(DragEvent e)
            {
                //Logger.Log(e.MousePosition.Y.ToString());

                return base.OnDrag(e);
            }

            protected override bool OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != MouseButton.Left) return false;

                box.FadeColour(Color4.White, 100);

                return base.OnMouseUp(e);
            }
        }
    }
}
