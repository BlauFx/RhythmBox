using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Overlays
{
    public class NotificationOverlay : OverlayContainer
    {
        private Box box;

        public SpriteText Text { get; set; }

        private const float Duration = 1000;

        public TypeOfOverlay typeOfOverlay = TypeOfOverlay.Default;

        public Action ActionYes { get; set; }
        public Action ActionNo { get; set; }
        public Action ActionCancel { get; set; }

        private SpriteTextButton[] spriteTextButtons = new SpriteTextButton[2];

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Depth = float.MaxValue,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Black.Opacity(1f),
                    Alpha = 1f,
                },
                box = new Box
                {
                    Depth = float.MaxValue - 1,
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
                Text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.9f, .2f),
                    Font = new FontUsage("Roboto, 30f"),
                    Text = "Test Notification!",
                    Colour = Color4.Black,
                    AllowMultiline = true,
                },
                spriteTextButtons[0] = new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    Y = 0.25f,
                    X = -0.125f,
                    Text = "Yes",
                    ClickAction = () =>
                    {
                        ActionYes?.Invoke();
                        this.State.Value = Visibility.Hidden;
                    },
                },
                spriteTextButtons[1] = new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.15f, 0.1f),
                    Text = "No",
                    Y = 0.25f,
                    X = 0.125f,
                    ClickAction = () =>
                    {
                        switch (typeOfOverlay)
                        {
                            case TypeOfOverlay.Default:
                                spriteTextButtons[0].Alpha = 1f;
                                spriteTextButtons[1].Alpha = 0f;
                                break;
                            case TypeOfOverlay.YesNo:
                                ActionNo?.Invoke();
                                break;
                            case TypeOfOverlay.YesCancel:
                                ActionCancel?.Invoke();
                                break;
                        }
                        this.State.Value = Visibility.Hidden;
                    },
                },
            };
        }

        protected override void PopIn()
        {
            switch (typeOfOverlay)
            {
                case TypeOfOverlay.Default:
                    spriteTextButtons[0].Alpha = 1f;
                    spriteTextButtons[1].Alpha = 0f;
                    break;
                case TypeOfOverlay.YesNo:
                    spriteTextButtons[0].Alpha = 1f;
                    spriteTextButtons[1].Alpha = 1f;
                    spriteTextButtons[1].Text = "No";
                    break;
                case TypeOfOverlay.YesCancel:
                    spriteTextButtons[0].Alpha = 1f;
                    spriteTextButtons[1].Alpha = 1f;
                    spriteTextButtons[1].Text = "Cancel";
                    break;
            }

            this.MoveTo(new Vector2(0f, -0.1f), 0, Easing.None);
            this.MoveToOffset(new Vector2(0, 0.1f), Duration, Easing.InOutQuint);
            this.FadeInFromZero(Duration, Easing.InOutQuint);
            box.FadeInFromZero(Duration, Easing.InOutQuint);
        }

        protected override void PopOut()
        {
            this.MoveToOffset(new Vector2(0, -0.1f), Duration, Easing.InOutQuint);
            this.FadeOutFromOne(Duration, Easing.InOutQuint);
            box.FadeOutFromOne(Duration, Easing.InOutQuint);
        }

        public enum TypeOfOverlay
        {
            Default = 0x1,
            YesNo = 0x2,
            YesCancel = 0x3,
        }
    }
}
