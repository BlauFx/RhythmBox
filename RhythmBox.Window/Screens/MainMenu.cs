using System;
using System.IO;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;
using RhythmBox.Window.Updater;

namespace RhythmBox.Window.Screens
{
    public class MainMenu : Screen
    {
        public Sprite Background { get; set; }

        private NotificationOverlay _overlay;

        private SongSelction songSelction;

        private Box box;

        private SpriteText CurrentPlaying;

        protected bool Disable_buttons = false;

        private const float Font_Size = 40f;

        [BackgroundDependencyLoader]
        private async void Load(LargeTextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                Background = new Sprite
                {
                    Depth = 2,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Texture = store.Get("Skin/menu-background.png"),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1.1f),
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    Text = "Play",
                    FontSize = Font_Size,
                    Y = 0f,
                    X = -0.375f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                        if (Disable_buttons) return;

                        if (songSelction.ValidForPush)
                            this.Push(songSelction);
                    }
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    Text = "Settings",
                    FontSize = Font_Size,
                    Y = 0f,
                    X = -0.125f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                        if (Disable_buttons) return;
                        this.Push(new Settings());
                    }
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    Text = "Editor",
                    FontSize = Font_Size,
                    Y = 0f,
                    X = 0.125f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {                
                        if (Disable_buttons) return;
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                        if (!File.Exists(path))
                        {
                            new DefaultFolder();
                        }
                        
                        this.Push(new EditorDefault(path));
                    }
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f),
                    Text = "Exit",
                    FontSize = Font_Size,
                    Y = 0f,
                    X = 0.375f,
                    Alpha = 1f,
                    ClickAction = () =>
                    {
                        if (Disable_buttons) return;
                        Environment.Exit(0);
                    }
                },
                CurrentPlaying = new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    RelativePositionAxes = Axes.Both,
                    Y = 0.1f,
                    Text = new LocalisedString($"Currently playing: {null}"),
                },
            };

            Schedule(async () =>
            {
                await LoadComponentAsync(songSelction = new SongSelction());
            });

            new DefaultFolder();

            AddInternal(box = new Box
            {
                Depth = float.MinValue + 1,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Color4.Black.Opacity(0.8f),
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Alpha = 0f,
            });

            var updater = new Update();
            
            AddInternal(_overlay = new NotificationOverlay
            {
                Depth = float.MinValue,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.3f),
                typeOfOverlay = NotificationOverlay.TypeOfOverlay.YesNo,
                ActionYes = () =>
                {
                    updater.DownloadUpdate();
                    updater.ApplyUpdate();
                }
            });

            bool NewUpdate = await updater.SearchAsyncForUpdates();

            if (NewUpdate)
            {
                //TODO:
                _overlay._text.Text = "A new update is available!                                      " +
                    "Do you want to install it now?";

                box.Size = new Vector2(3f);
                box.FadeIn(0d, Easing.OutCirc);
                _overlay.State.Value = Visibility.Visible;
                _overlay.State.ValueChanged += (e) => box.FadeOut(250d);
            }
            
            Discord.DiscordRichPresence.ctor();

            Discord.DiscordRichPresence.UpdateRPC(
                new DiscordRPC.RichPresence()
                {
                    Details = "MainMenu",
                    State = " ",
                    Assets = new DiscordRPC.Assets()
                    {
                        LargeImageKey = "three",
                    }
                });
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (e.LastMousePosition.Y >= e.MousePosition.Y)
            {
                Background.MoveToOffset(new Vector2(0f, -0.05f), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.Y <= e.MousePosition.Y)
            {
                Background.MoveToOffset(new Vector2(0f, 0.05f), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.X >= e.MousePosition.X)
            {
                Background.MoveToOffset(new Vector2(-0.05f, 0), 100, Easing.OutQuart);
            }

            if (e.LastMousePosition.X <= e.MousePosition.X)
            {
                Background.MoveToOffset(new Vector2(0.05f, 0), 100, Easing.OutQuart);
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Background.MoveTo(new Vector2(0), 1000, Easing.InBack);

            base.OnHoverLost(e);
        }

        public override void OnEntering(IScreen last) => this.FadeInFromZero<MainMenu>(250, Easing.In);

        public override void OnResuming(IScreen last)
        {
            this.FadeInFromZero<MainMenu>(175, Easing.In);

            Schedule(async () =>
            {
                await LoadComponentAsync(songSelction = new SongSelction());
            });

            base.OnResuming(last);

            Discord.DiscordRichPresence.UpdateRPC(
                new DiscordRPC.RichPresence()
                {
                    Details = "MainMenu",
                    State = " ",
                    Assets = new DiscordRPC.Assets()
                    {
                        LargeImageKey = "three",
                    }
                });
        }

        public override void OnSuspending(IScreen next)
        {
            this.FadeOutFromOne<MainMenu>(0, Easing.In);
            base.OnSuspending(next);
        }
    }

    public class MainMenuBox : Container
    {
        public string Text { get; set; } = string.Empty;

        public float FontSize { get; set; } = 20f;

        public Action ClickAction { get; set; }

        protected SpriteText sprite;

        protected Box box;

        private Color4 Color { get; set; } = Color4.White;

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
                    Colour = Color,
                },
                sprite = new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Black,
                    RelativePositionAxes = Axes.Both,
                    Text = Text,
                    X = 0,
                    //Y = (0.25f/2f),
                    Font = new FontUsage("Roboto", FontSize),
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            box.Colour = Color.Opacity(0.7f);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            box.Colour = Color.Opacity(1f);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            ClickAction?.Invoke();

            return base.OnClick(e);
        }
    }
}
