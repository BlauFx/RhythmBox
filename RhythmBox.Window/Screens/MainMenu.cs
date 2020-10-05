using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.Screens.SongSelection;
using RhythmBox.Window.Updater;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RhythmBox.Window.Screens
{
    public class MainMenu : Screen
    {
        public Sprite Background { get; set; }

        private NotificationOverlay _overlay;

        private Selection songSelection;

        private Box box;

        private SpriteText CurrentPlaying;

        [Resolved]
        private CachedMap cachedMap { get; set; }

        [Resolved]
        private GameHost Host { get; set; }

        private Volume volume;

        private const float Font_Size = 40f;

        public virtual Action GetAction(int pos)
        {
            Action[] clickAction = new Action[4];

            clickAction[0] = () =>
            {
                if (songSelection.ValidForPush)
                    this.Push(songSelection);
            };

            clickAction[1] = () =>
            {
                cachedMap.Stop();
                this.Push(new Settings());
            };

            clickAction[2] = () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\TestMap\\Difficulty1.ini";

                if (!File.Exists(path))
                    _ = new DefaultFolder();

                cachedMap.Stop();
                this.Push(new EditorDefault(path));
            };

            clickAction[3] = () => Environment.Exit(0);

            return clickAction[pos];
        }

        [BackgroundDependencyLoader]
        private async void Load(LargeTextureStore store)
        {
            cachedMap.Map = Songs.GetRandomMap();
            cachedMap.LoadTrackFile();

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
                new Box
                {
                    Depth = Background.Depth - 0.1f,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.15f, 1.1f),
                    Position = new Vector2(0.05f, 0f),
                    Shear = new Vector2(.15f, 0f),
                    EdgeSmoothness = new Vector2(2f)
                },
                new MusicVisualizationLinear(3f, 120, 0f, cachedMap.BindableTrack)
                {
                    Depth = Background.Depth - 0.2f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.CentreLeft,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(.25f, 0f),
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f, 0.1f),
                    Text = "Play",
                    FontSize = Font_Size,
                    Y = -0.25f,
                    X = -0.2f,
                    Alpha = 1f,
                    ClickAction = GetAction(0)
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f, 0.1f),
                    Text = "Settings",
                    FontSize = Font_Size,
                    Y = -0.05f,
                    X = -0.21f,
                    Alpha = 1f,
                    ClickAction = GetAction(1)
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f, 0.1f),
                    Text = "Editor",
                    FontSize = Font_Size,
                    Y = 0.15f,
                    X = -0.25f,
                    Alpha = 1f,
                    ClickAction = GetAction(2)
                },
                new MainMenuBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.2f, 0.1f),
                    Text = "Exit",
                    FontSize = Font_Size,
                    Y = 0.35f,
                    X = -0.3f,
                    Alpha = 1f,
                    ClickAction = GetAction(3)
                },
                CurrentPlaying = new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    RelativePositionAxes = Axes.Both,
                    Y = 0.1f,
                },
                volume = new Volume(cachedMap.BindableTrack)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(1f, 0.3f),
                    X = 0.4f,
                    Y = 0.2f,
                    Alpha = 0f,
                },
            };

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
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        updater.DownloadUpdate();
                        updater.ApplyUpdate();
                    }
                    else
                    {
                        //TODO:
                    }
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

        protected override void LoadAsyncComplete()
        {
            cachedMap.Play(0);

            base.LoadAsyncComplete();
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Background.MoveTo(new Vector2(0f));
            float Intensity = .1f;

            Vector2 offset = ToLocalSpace(e.ScreenSpaceMousePosition) - Background.DrawSize / (Background.Size.X * 2);
            Background.MoveToOffset(offset * Intensity);

            return base.OnMouseMove(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            volume.ChangeVolume(true, e);
            volume.FadeIn(100).OnComplete(x => x.Delay(1000).FadeOut(100));

            return base.OnScroll(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) => Background.MoveTo(new Vector2(0f), 500);

        protected override void UpdateAfterChildren() => CurrentPlaying.Text = new LocalisedString($"Currently playing: {cachedMap?.Map?.Title}");

        private void LimitFPS(bool Limit)
        {
            Host.MaximumDrawHz = Limit ? 200 : int.MaxValue;
            Host.MaximumUpdateHz = Limit ? 200 : int.MaxValue;;
        }

        public override void OnEntering(IScreen last)
        {
            Schedule(async () => await LoadComponentAsync(songSelection = new Selection()));

            LimitFPS(Limit: true);

            this.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);
            this.FadeInFromZero<MainMenu>(250, Easing.In);
        }

        public override void OnResuming(IScreen last)
        {
            Schedule(async () => await LoadComponentAsync(songSelection = new Selection()));
            LimitFPS(Limit: true);

            this.FadeInFromZero<MainMenu>(175, Easing.In);

            cachedMap.Play();

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

            base.OnResuming(last);
        }

        public override void OnSuspending(IScreen next)
        {
            cachedMap.Stop();

            LimitFPS(Limit: false);

            this.FadeOutFromOne<MainMenu>(0, Easing.In);
            base.OnSuspending(next);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing) return;
            cachedMap.Stop();

            base.Dispose(isDisposing);
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
