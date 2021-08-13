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
    public class BreakOverlay : FocusedOverlayContainer
    {
        private TextFlowContainer text;

        private Box box;

        private SpriteButton @continue, @return;

        private readonly Action[] actions;

        public BreakOverlay(Action[] actions) => this.actions = actions;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
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
                text = new TextFlowContainer
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextAnchor = Anchor.Centre,
                    Spacing = new Vector2(2f),
                    AutoSizeAxes = Axes.Both,
                    Alpha = 1f,
                },
                @continue = new SpriteButton
                {
                    Alpha = 0f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    Texture = store.Get("Skin/Continue"),
                    ClickAction = () => actions[0]?.Invoke(),
                },
                @return = new SpriteButton
                {
                    Alpha = 0f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    Texture = store.Get("Skin/Return"),
                    ClickAction = () => actions[1]?.Invoke(),
                }
            };

            @return.Y += @continue.Height + 0.05f;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != osuTK.Input.Key.T)
                return true;

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


        private void AnimationIn()
        {
            text.Text = string.Empty;
            text.MoveTo(new Vector2(0f));

            this.FadeInFromZero(100, Easing.In);
            box.FadeInFromZero(100, Easing.In);

            text.AddText("You've paused the game!", x => x.Font = new FontUsage("Roboto", 100));
            text.Scale = new Vector2(0f);

            text.FadeInFromZero(500, Easing.InBack);
            text.ScaleTo(1f, 2000, Easing.OutElastic);

            Scheduler.AddDelayed(() =>
            {
                text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In);

                @continue.FadeInFromZero(500, Easing.In);
                @return.FadeInFromZero(500, Easing.In);
            }, 1000);
        }

        public void AnimationOut()
        {
            this.FadeOutFromOne(1000, Easing.In);
            box.FadeInFromZero(0, Easing.In);

            text.FadeOutFromOne(500, Easing.OutBack);
            text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In);
            
            @continue.FadeOutFromOne(500, Easing.In);
            @return.FadeOutFromOne(500, Easing.In);
        }
    }
}
