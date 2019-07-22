using System;
using NUnit.Framework;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Objects;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class MainMenu : Screen
    {
        public static Sprite background;

        private Logo logo;

        private SettingsOverlay settings;

        private SelectButton[] startButtons = new SelectButton[3];

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                logo = new Logo
                {
                    Depth = 0,
                    Size = new Vector2(0.4f,0.55f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    texture = store.Get("Game/Logo.png"),
                    ClickAction = () =>
                    {
                        startButtons[0].Alpha = 1f;
                        startButtons[1].Alpha = 1f;
                        startButtons[2].Alpha = 1f;
                    }
                },
                background = new Sprite
                {
                    Depth = 2,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Texture = store.Get("Skin/menu-background.png"),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1.1f),
                },
                startButtons[0] = new SelectButton
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.3f,0.08f),
                    text = "Play",
                    FontSize = 60f,
                    Y = -0.2f,
                    X = 0.2f,
                    Alpha = 0f,
                    ClickAction = () =>
                    {
                        this.Push(new GameplayScreen());
                    }
                },
                startButtons[1] = new SelectButton
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.3f,0.08f),
                    text = "Settings",
                    FontSize = 60f,
                    Y = 0f,
                    X = 0.2f,
                    Alpha = 0f,
                    ClickAction = () =>
                    {
                        settings.Show();
                    },
                },
                startButtons[2] = new SelectButton
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.3f,0.08f),
                    text = "Exit",
                    FontSize = 60f,
                    Y = 0.2f,
                    X = 0.2f,
                    Alpha = 0f,
                    ClickAction = () =>
                    {
                        Environment.Exit(0);
                    },
                },
                settings = new SettingsOverlay
                {
                    Depth = int.MinValue,
                },
            };
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (e.LastMousePosition.Y >= e.MousePosition.Y)
            {
                background.MoveToOffset(new Vector2(0f, -0.05f), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.Y <= e.MousePosition.Y)
            {
                background.MoveToOffset(new Vector2(0f, 0.05f), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.X >= e.MousePosition.X)
            {
                background.MoveToOffset(new Vector2(-0.05f, 0), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.X <= e.MousePosition.X)
            {
                background.MoveToOffset(new Vector2(0.05f, 0), 100, Easing.OutQuart);
            }

            return base.OnMouseMove(e);
        }
    }
}
