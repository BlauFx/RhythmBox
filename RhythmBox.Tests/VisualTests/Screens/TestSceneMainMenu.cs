﻿using NUnit.Framework;
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
using RhythmBox.Window.pending_files;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneMainMenu : TestScene
    {
        public static Sprite background;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new Logo
                {
                    Depth = 0,
                    Size = new Vector2(0.4f,0.55f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    texture = store.Get("Game/Logo.png")
                },
                background = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Texture = store.Get("Game/menu-background.png"),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1.1f),
                }
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
