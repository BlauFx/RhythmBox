using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneMainMenu : TestScene
    {
        private ScreenStack stack = null;

        private TestMainMenu testMainMenu;

        private bool Can_new_TestSceneMainMenu = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestSceneGameplayScreen", () =>
            {
                if (Can_new_TestSceneMainMenu)
                {
                    Can_new_TestSceneMainMenu = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(testMainMenu = new TestMainMenu()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0f)
                    });
                    testMainMenu.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);
                    stack.Push(testMainMenu);
                }
            });

            AddStep("Remove TestSceneGameplayScreen", () =>
            {
                this.stack?.Expire();
                this.testMainMenu?.Exit();
                this.testMainMenu?.Expire();
                this.testMainMenu = null;

                Can_new_TestSceneMainMenu = true;
            });
        }
    }
    public class TestMainMenu : Screen
    { 
        public static Sprite background;

        [BackgroundDependencyLoader] 
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
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
                new TestSceneMainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    text = "Play",
                    FontSize = 60f,
                    Y = 0f,
                    X = -0.375f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                    }
                },
                new TestSceneMainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    text = "Settings",
                    FontSize = 60f,
                    Y = 0f,
                    X = -0.125f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                    }
                },
                new TestSceneMainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    text = "Editor",
                    FontSize = 60f,
                    Y = 0f,
                    X = 0.125f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                    }
                },
                new TestSceneMainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    text = "Exit",
                    FontSize = 60f,
                    Y = 0f,
                    X = 0.375f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                        Environment.Exit(0);
                    }
                },
            };

            new DefaultFolder();
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

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero<TestMainMenu>(250, Easing.In);
            base.OnEntering(last);
        }

        public override void OnResuming(IScreen last)
        {
            this.FadeInFromZero<TestMainMenu>(175, Easing.In);
            base.OnResuming(last);
        }

        public override void OnSuspending(IScreen next)
        {
            this.FadeOutFromOne<TestMainMenu>(0, Easing.In);
            base.OnSuspending(next);
        }
    }

    internal class TestSceneMainMenuBox : Container
    {
        public string text { get; set; } = string.Empty;

        public float FontSize { get; set; } = 20f;

        public Action ClickAction;

        protected SpriteText sprite;

        protected Box box;

        private Color4 color { get; set; } = Color4.White;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                box = new Box
                {
                    Depth = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Colour = color,
                },
                sprite = new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Black,
                    RelativePositionAxes = Axes.Both,
                    Text = text,
                    X = 0,
                    //Y = (0.25f/2f),
                    Font = new FontUsage("Roboto",FontSize),
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            box.Colour = color.Opacity(0.7f);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            box.Colour = color.Opacity(1f);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            ClickAction?.Invoke();

            return base.OnClick(e);
        }
    }
}
