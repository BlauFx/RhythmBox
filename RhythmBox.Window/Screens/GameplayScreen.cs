using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.Screens.SongSelection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RhythmBox.Window.Mods;
using RhythmBox.Window.Screens.Playfield;
using RhythmBox.Window.Screens.Result;

namespace RhythmBox.Window.Screens
{
    public class GameplayScreen : Screen
    {
        private double Accuracy { get; set; } = 100; //TODO:

        private TextFlowContainer dispayCombo, dispayScore;

        private readonly Map map;

        public Playfield.Playfield RbPlayfield;
        
        public HPBar Hpbar { get; private set; }

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private readonly BindableDouble userPlaybackRate = new(1) { Default = 1, MinValue = 0.1, MaxValue = 3, Precision = 0.1 };

        private readonly BindableBool isPaused = new();

        private readonly BindableBool resuming = new(true);
        private bool HasFailed { get; set; }

        private BreakOverlay breakOverlay;

        public Track Track;
        private BindableBool ReturntoSongSelectionAfterFail { get; } = new BindableBool();

        public readonly BindableBool GameStarted = new BindableBool();

        private GameplayScreenLoader gameplayScreenLoader;

        private readonly List<Mod> toApplyMods;

        private readonly Sprite[] keyPress = new Sprite[4];

        private readonly Key[] keys = new Key[4];

        [Resolved]
        private AudioManager audio { get; set; }
        
        [Resolved]
        private Gameini Gameini { get; set; }
        
        [Resolved]
        private TextureStore textureStore { get; set; }

        private Volume volume;

        private const float SCREEN_ENTERING_DURATION = 500;

        public GameplayScreen(string path, List<Mod> ToApplyMods)
        {
            this.toApplyMods = ToApplyMods;
            map = new Map(path);
        }

        [BackgroundDependencyLoader]
        private void Load(GameHost gameHost, CachedMap cachedMap)
        {
            IResourceStore<byte[]> store = new StorageBackedResourceStore(gameHost.Storage);
            ITrackStore trackStore = audio.GetTrackStore(store);

            var audioFile = $"{Path.GetDirectoryName(map.Path)}{Path.DirectorySeparatorChar}{map.AFileName}";
            Track = trackStore.Get(audioFile);
            
            if (Track != null)
                Track.Volume.Value = Gameini.Get<double>(SettingsConfig.Volume);

            InternalChildren = new Drawable[]
            {
                gameplayScreenLoader = new GameplayScreenLoader
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                rhythmBoxClockContainer = new RhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f)
                },
                breakOverlay = new BreakOverlay(new Action[] { () => breakOverlay.State.Value = Visibility.Hidden, () => ReturntoSongSelectionAfterFail.Value = true })
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 0f,
                },
                keyPress[0] = GetSprite("Skin/K1", 0.3f),
                keyPress[1] = GetSprite("Skin/K2", 0.4f),
                keyPress[2] = GetSprite("Skin/K3", 0.5f),
                keyPress[3] = GetSprite("Skin/K4", 0.6f),
                volume = new Volume(new Bindable<Track>(Track))
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
            
            for (int i = 0; i < 4; i++)
            {
                Enum.TryParse(Gameini.Get<string>((SettingsConfig)i), out Key key);
                keys[i] = key;
            }
            
            breakOverlay.State.Value = Visibility.Hidden;
            breakOverlay.State.ValueChanged += async (e) =>
            {
                if (e.NewValue == Visibility.Hidden)
                {
                    breakOverlay.AnimationOut();
                    await Task.Delay(1500);
                    resuming.Value = true;
                    rhythmBoxClockContainer.Start();
                    Track.Start();
                    RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                RbPlayfield = new Playfield.Playfield(toApplyMods)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both, 
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 0.9f),
                    Map = map,
                    Y = 0.02f
                },
                Hpbar = new HPBar(toApplyMods)
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft, 
                    RelativeSizeAxes = Axes.Both, 
                    Size = new Vector2(0.8f, 1f),
                    Colour = Color4.AliceBlue
                },
                dispayCombo = new TextFlowContainer
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.BottomLeft,
                },
                dispayScore = new TextFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.TopRight,
                    X = -0.01f
                },
            };

            rhythmBoxClockContainer.IsPaused.BindTo(isPaused);
            rhythmBoxClockContainer.UserPlaybackRate.BindTo(userPlaybackRate);

            RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            dispayScore.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            dispayCombo.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            Hpbar.Clock = rhythmBoxClockContainer.RhythmBoxClock;

            RbPlayfield.Resuming.BindTo(resuming);

            dispayCombo.AddText("0x", x => x.Font = new FontUsage("Roboto", 40));
            dispayScore.AddText("000000", x => x.Font = new FontUsage("Roboto", 40));
            
            Score.Combo.ComboInt.ValueChanged += (e) => Hpbar.CurrentValue.Value += Hpbar.CalcValue(Score.Combo.CurrentHit);
            RbPlayfield.HasFinished.ValueChanged += (e) =>
            {
                if (!e.NewValue) 
                    return;

                rhythmBoxClockContainer.Stop();

                var currentTime = Track?.CurrentTime;
                Track?.Stop();

                RbPlayfield.HasFinished.UnbindEvents();

                cachedMap.Map = map;
                cachedMap.LoadTrackFile();
                cachedMap.Seek(currentTime.GetValueOrDefault());

                Scheduler.AddDelayed(() => this.Expire(), 1000);
                LoadComponentAsync(new ResultScreen(), this.Push);
            };

            ReturntoSongSelectionAfterFail.ValueChanged += (e) =>
            {
                cachedMap.Map = map;
                cachedMap.LoadTrackFile();
                cachedMap.Seek(Track.CurrentTime);

                Selection songSelction;
                LoadComponent(songSelction = new Selection());
                Schedule(() => this.Push(songSelction));
            };

            RbPlayfield.CanStart.ValueChanged += (e) =>
            {
                if (e.NewValue) 
                    Load(1000);
            };
        }

        private async void Load(int time)
        {
            await gameplayScreenLoader.Rotate(time);
            Schedule(() => gameplayScreenLoader.FadeOut(time, Easing.In).Delay(time).Finally((action) =>
            { 
                gameplayScreenLoader.Expire();
                RemoveInternal(gameplayScreenLoader);
                
                gameplayScreenLoader = null;
            }));

            await Task.Delay(time);
            GameStarted.Value = true;

            rhythmBoxClockContainer.Start();
            Track?.Start();
            
            rhythmBoxClockContainer.Seek(map.StartTime);
            Track?.Seek(map.StartTime);
            
            if (!Hpbar.Enabled.Value) return;
            Hpbar.Drain(false);
        }

        protected override void Update()
        {
            if (Hpbar.Enabled.Value && Hpbar.CurrentValue.Value <= 0)
                if (!HasFailed)
                    Fail();

            dispayCombo.Text = dispayScore.Text = string.Empty;

            dispayCombo.AddText($"{Score.Combo.ComboInt}x", x => x.Font = new FontUsage("Roboto", 40));
            dispayScore.AddText(Score.Score.ScoreInt.ToString(), x => x.Font = new FontUsage("Roboto", 40));

            base.Update();
        }

        private async void Fail()
        {
            HasFailed = true;
            RbPlayfield.Failed = true;

            Box box;

            AddInternal(box = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Colour = Color4.Red,
                Alpha = 0f,
            });

            box.FadeTo(0.7f, 500, Easing.In);

            foreach (var x in this.RbPlayfield)
            {
                if (x is DrawPlayfield playfield)
                {
                    foreach (var y in playfield)
                    {
                        y.TransformTo(nameof(Shear), new Vector2(osu.Framework.Utils.RNG.NextSingle(-0.15f, 0.15f)), 5000, Easing.OutBack);
                        y.TransformTo(nameof(Scale), new Vector2(osu.Framework.Utils.RNG.NextSingle(1.1f, 2f)), 5000, Easing.OutBack);
                    }
                }
                else
                {
                    x.TransformTo(nameof(Shear), new Vector2(osu.Framework.Utils.RNG.NextSingle(-0.15f, 0.15f)), 1000, Easing.OutBack);
                    x.TransformTo(nameof(Scale), new Vector2(osu.Framework.Utils.RNG.NextSingle(0.6f, 2f)), 1000, Easing.OutBack);
                    x.MoveToOffset(new Vector2(osu.Framework.Utils.RNG.NextSingle(0.1f, 0.4f)), 1000, Easing.OutBack);
                }
            }

            rhythmBoxClockContainer.StopWithDelay();
            
            if (Track == null)
                return;

            for (double i = Track.Frequency.Value; i > 0; i -= 0.1d)
            {
                try
                {
                    Track.Frequency.Value = i;
                }
                catch { }
                await Task.Delay(500);
            }

            //TODO:
            //ReturntoSongSelectionAfterFail.Value = true;
        }
        
        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
            {
                if (resuming.Value && gameplayScreenLoader == null)
                {
                    resuming.Value = false;
                    rhythmBoxClockContainer.Stop();
                        
                    Track?.Stop(); 
                    breakOverlay.ToggleVisibility();
                }

                if (HasFailed)
                    ReturntoSongSelectionAfterFail.Value = true;

                RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            }

            CheckKey(e.Key, true);

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e) => CheckKey(e.Key, false);

        private void CheckKey(Key e, bool Down = true)
        {
            int i;

            if (e == keys[0])
                i = 0;
            else if (e == keys[1])
                i = 1;
            else if (e == keys[2])
                i = 2;
            else if (e == keys[3])
                i = 3;
            else
                return;

            keyPress[i].FadeTo(Down ? 0.5f : 1f, 50);
        }
        
        protected override bool OnScroll(ScrollEvent e)
        {
            volume.ChangeVolume(e);
            volume.Fade(100, 100, 1000);

            return base.OnScroll(e);
        }

        Sprite GetSprite(string texture, float YPos)
        {
            return new Sprite
            {
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.CentreRight,
                Texture = textureStore.Get(texture),
                Y = YPos,
            };
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(SCREEN_ENTERING_DURATION, Easing.In);
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            Track?.Stop();
            Schedule(this.Exit);

            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            Track?.Stop();
            return base.OnExiting(next);
        }

        protected override void Dispose(bool isDisposing)
        {
            Track?.Stop();
            base.Dispose(isDisposing);
        }
    }
}
