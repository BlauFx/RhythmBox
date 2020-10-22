using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Overlays
{
    public class ModOverlay : FocusedOverlayContainer
    {
        public TextFlowContainer _text;

        public Mods modBox;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 0,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Black.Opacity(0.75f),
                },
                modBox = new Mods
                {
                    Depth = -2,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.6f, 0.3f),
                    Colour = Color4.Green.Opacity(0.8f),
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
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
                this.State.Value = Visibility.Hidden;

            return true;
        }

        protected override void PopIn()
        {
            _text.Scale = new Vector2(0f);
            _text.MoveTo(new Vector2(0f));
            _text.Text = string.Empty;
            _text.AddText("Here are some mods!", x => x.Font = new FontUsage("Roboto", 100));

            this.FadeInFromZero(1000, Easing.In);

            _text.FadeInFromZero(1000, Easing.InBack);
            _text.ScaleTo(1f, 1250, Easing.OutElastic);
            _text.MoveToOffset(new Vector2(0f, -0.25f), 1000, Easing.OutElastic); 
            
            base.PopIn();
        }

        protected override void PopOut()
        {
            this.FadeOutFromOne(400, Easing.In);
            _text.FadeOutFromOne(400, Easing.OutBack);
            _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In);
            
            base.PopOut();
        }
    }
}
