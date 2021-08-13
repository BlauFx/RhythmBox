using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
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
        private Sprite Background { get; set; }

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

        [BackgroundDependencyLoader]
        private void Load(LargeTextureStore store)
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
                GetMainMenuBox("Play", -0.2f, -0.25f, 0),
                GetMainMenuBox("Settings", -0.21f, -0.05f, 1),
                GetMainMenuBox("Editor", -0.25f, 0.15f, 2),
                GetMainMenuBox("Exit", -0.3f, 0.35f, 3),
                
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

            bool newUpdate = false; //await updater.SearchAsyncForUpdates();

            if (newUpdate)
            {
                //TODO:
                _overlay.Text.Text = "A new update is available!                                      " +
                    "Do you want to install it now?";

                box.Size = new Vector2(3f);
                box.FadeIn(0d, Easing.OutCirc);
                _overlay.State.Value = Visibility.Visible;
                _overlay.State.ValueChanged += (e) => box.FadeOut(250d);
            }

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
            volume.ChangeVolume(e);
            volume.Fade(100, 100, 1000);

            return base.OnScroll(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) => Background.MoveTo(new Vector2(0f), 500);

        protected override void UpdateAfterChildren() => CurrentPlaying.Text = $"Currently playing: {cachedMap?.Map?.Title}";

        private void LimitFPS(bool Limit)
        {
            Host.MaximumDrawHz = Limit ? 200 : short.MaxValue;
            Host.MaximumUpdateHz = Limit ? 200 : short.MaxValue;;
        }

        public override void OnEntering(IScreen last)
        {
            LimitFPS(Limit: true);

            this.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);
            this.FadeInFromZero(250, Easing.In);
        }

        public override void OnResuming(IScreen last)
        {
            LimitFPS(Limit: true);

            this.FadeInFromZero(175, Easing.In);
            cachedMap.Play();
            
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
        
        protected virtual Action GetAction(int pos)
        {
            Action[] clickAction = new Action[4];

            clickAction[0] = () =>
            {
                if (songSelection != null && songSelection.ValidForPush)
                    this.Push(songSelection);
                else
                    LoadComponentAsync(songSelection = new Selection(), this.Push);
            };

            clickAction[1] = () =>
            {
                cachedMap.Stop();
                this.Push(new Settings());
            };

            clickAction[2] = () =>
            {
                string path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}{Path.DirectorySeparatorChar}Songs{Path.DirectorySeparatorChar}TestMap{Path.DirectorySeparatorChar}Difficulty1.ini";

                if (!File.Exists(path))
                    _ = new DefaultFolder();

                cachedMap.Stop();
                this.Push(new EditorDefault(path));
            };

            clickAction[3] = () => Environment.Exit(0);

            return clickAction[pos];
        }

        private MainMenuBox GetMainMenuBox(string text, float XPos, float YPos, int GetActionPos) =>
            new MainMenuBox
            {
                Depth = 1,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.2f, 0.1f),
                Text = text,
                FontSize = Font_Size,
                X = XPos,
                Y = YPos,
                Alpha = 1f,
                ClickAction = GetAction(GetActionPos)
            };
    }

    public class MainMenuBox : Container
    {
        public string Text { get; set; } = string.Empty;

        public float FontSize { get; set; } = 20f;

        public Action ClickAction { get; set; }

        private Box box;

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
                new SpriteText
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
