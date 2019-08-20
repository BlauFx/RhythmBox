using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;


namespace RhythmBox.Window.Overlays
{
    public class BreakOverlay : FocusedOverlayContainer
    {
        public TextFlowContainer _text;

        private Box box;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                box = new Box
                {
                    Depth = 0,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Black.Opacity(0.8f),
                    Alpha = 0f,
                },
                _text = new TextFlowContainer
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextAnchor = Anchor.Centre,
                    Spacing = new Vector2(2f),
                    AutoSizeAxes = Axes.Both,
                    Alpha = 1f,
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != osuTK.Input.Key.T)
            {
                return true;
            }
            this.State.Value = Visibility.Hidden;

            return base.OnKeyDown(e);
        }

        protected override void PopIn()
        {
            AnimationIn();
            base.PopIn();
        }

        protected override void PopOut()
        {
            AnimationOut();
            base.PopOut();
        }

        public void Reset()
        {
            _text.Text = string.Empty;
            _text.MoveTo(new Vector2(0f));
        }

        public void AnimationIn()
        {
            Reset();
            this.FadeInFromZero(100, Easing.In);
            box.FadeInFromZero(100, Easing.In);
            _text.AddText("You've paused the game!", x => x.Font = new FontUsage("Roboto", 100));
            _text.Scale = new Vector2(0f);
            _text.FadeInFromZero(500, Easing.InBack);
            _text.ScaleTo(1f, 2000, Easing.OutElastic);
            Scheduler.AddDelayed(() => _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In), 1000);
        }

        public void AnimationOut()
        {
            this.FadeOutFromOne(1000, Easing.In);
            box.FadeInFromZero(0, Easing.In);
            _text.FadeOutFromOne(500, Easing.OutBack);
            _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In);
        }
    }
}
