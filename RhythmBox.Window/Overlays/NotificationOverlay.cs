using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Overlays
{
    public class NotificationOverlay : OverlayContainer
    {
        private Box _box;

        private SpriteText _text;

        private const float Duration = 1000;
        
        public Action Action { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                _box = new Box
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.White.Opacity(0.6f),
                    Alpha = 0f,
                },
                new SpriteButton
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    Texture = store.Get("Game/X"),
                    ClickAction = () =>
                    {
                        this.State.Value = Visibility.Hidden;
                    },
                },
                _text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    Font = new FontUsage("Roboto, 30f"),
                    Text = "Test Notification!",
                    Colour = Color4.Black,
                },
            };
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Action?.Invoke();
            this.State.Value = Visibility.Hidden;

            return base.OnMouseDown(e);
        }

        protected override void PopIn()
        {
            this.MoveTo(new Vector2(0f, -0.1f), 0, Easing.None);
            this.MoveToOffset(new Vector2(0, 0.1f), Duration, Easing.InOutQuint);
            this.FadeInFromZero(Duration, Easing.InOutQuint);
            _box.FadeInFromZero(Duration, Easing.InOutQuint);
        }

        protected override void PopOut()
        {
            this.MoveToOffset(new Vector2(0, -0.1f), Duration, Easing.InOutQuint);
            this.FadeOutFromOne(Duration, Easing.InOutQuint);
            _box.FadeOutFromOne(Duration, Easing.InOutQuint);
        }
    }
}
