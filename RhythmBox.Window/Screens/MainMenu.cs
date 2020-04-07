using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;
using RhythmBox.Window.Screens.SongSelection;
using RhythmBox.Window.Updater;
using System;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Screens
{
    public class MainMenu : Screen
    {
        public Sprite Background { get; set; }

        private NotificationOverlay _overlay;

        private SongSelcetion songSelction;

        private Box box;

        private SpriteText CurrentPlaying;

        [Resolved]
        private CurrentMap currentMap { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        [Resolved]
        private GameHost Host { get; set; }

        private Track track = null;

        private Volume volume;

        private const float Font_Size = 40f;

        public virtual Action GetAction(int pos)
        {
            Action[] clickAction = new Action[4];

            clickAction[0] = () =>
            {
                if (songSelction.ValidForPush)
                    this.Push(songSelction);
            };

            clickAction[1] = () =>
            {
                currentMap.Stop();
                this.Push(new Settings());
            };

            clickAction[2] = () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\TestMap\\Difficulty1.ini";

                if (!File.Exists(path))
                    _ = new DefaultFolder();

                currentMap.Stop();
                this.Push(new EditorDefault(path));
            };

            clickAction[3] = () => Environment.Exit(0);

            return clickAction[pos];
        }

        [BackgroundDependencyLoader]
        private async void Load(LargeTextureStore store, Gameini gameini)
        {
            track = Audio.GetTrackStore(new StorageBackedResourceStore(Host.Storage)).Get(CurrentSongsAvailable.GetRandomAudio());

            if (track != null)
                track.Volume.Value = gameini.Get<double>(SettingsConfig.Volume);

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
                new MusicVisualizationLinear(3f, 120, 0f, new Bindable<Track>(track))
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
                    //Text = new LocalisedString($"Currently playing: {currentMap?.Map.Title}"),
                },
                volume = new Volume(new Bindable<Track>(track))
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

            track?.Start();

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
            var Durationn = 100;
            var easing = Easing.OutQuart;

            //TODO: I currently don't like the way how it's done
            if (e.LastMousePosition.Y >= e.MousePosition.Y)
            {
                Background.MoveToOffset(new Vector2(0f, -0.05f), Durationn, easing);
            }

            if (e.LastMousePosition.Y <= e.MousePosition.Y)
            {
                Background.MoveToOffset(new Vector2(0f, 0.05f), Durationn, easing);
            }

            if (e.LastMousePosition.X >= e.MousePosition.X)
            {
                Background.MoveToOffset(new Vector2(-0.05f, 0), Durationn, easing);
            }

            if (e.LastMousePosition.X <= e.MousePosition.X)
            {
                Background.MoveToOffset(new Vector2(0.05f, 0), Durationn, easing);
            }

            return base.OnMouseMove(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            volume.ChangeVolume(true, e);
            volume.FadeIn(100).OnComplete(x => x.Delay(1000).FadeOut(100));

            return base.OnScroll(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) => Background.MoveTo(new Vector2(0), 2000);

        protected override void UpdateAfterChildren() => CurrentPlaying.Text = new LocalisedString($"Currently playing: {currentMap?.Map?.Title}");

        private void LimitFPS()
        {
            Host.MaximumDrawHz = 200;
            Host.MaximumUpdateHz = 200;
        }

        private void UnlockFPS()
        {
            Host.MaximumDrawHz = int.MaxValue;
            Host.MaximumUpdateHz = int.MaxValue;
        }

        public override void OnEntering(IScreen last)
        {
            Schedule(async () => await LoadComponentAsync(songSelction = new SongSelcetion()));

            LimitFPS();

            this.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);
            this.FadeInFromZero<MainMenu>(250, Easing.In);
        }

        public override void OnResuming(IScreen last)
        {
            Schedule(async () => await LoadComponentAsync(songSelction = new SongSelcetion()));
            track?.Start();

            LimitFPS();

            this.FadeInFromZero<MainMenu>(175, Easing.In);

            currentMap?.Play(Audio, 0);

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
            track?.Stop();

            UnlockFPS();

            this.FadeOutFromOne<MainMenu>(0, Easing.In);
            base.OnSuspending(next);
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();

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
