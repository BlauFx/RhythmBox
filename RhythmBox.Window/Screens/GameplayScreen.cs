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
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.Screens.SongSelection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RhythmBox.Window.Screens.Playfield;

namespace RhythmBox.Window.Screens
{
    public class GameplayScreen : Screen
    {
        private double Accuracy { get; set; } = 100; //TODO:

        private TextFlowContainer DispayCombo, DispayScore;

        private readonly Map _map;

        public Playfield.Playfield _RbPlayfield;
        
        public HPBar hpbar { get; private set; }

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private readonly BindableDouble UserPlaybackRate = new BindableDouble(1) { Default = 1, MinValue = 0.1, MaxValue = 3, Precision = 0.1 };

        private readonly BindableBool IsPaused = new BindableBool();

        private readonly BindableBool Resuming = new BindableBool(true);
        private bool HasFailed { get; set; }

        private BreakOverlay BreakOverlay;

        public Track track;
        private BindableBool ReturntoSongSelectionAfterFail { get; } = new BindableBool();

        public readonly BindableBool GameStarted = new BindableBool();

        private GameplayScreenLoader GameplayScreenLoader;

        private readonly List<Mod> ToApplyMods;

        private readonly Sprite[] KeyPress = new Sprite[4];

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
            this.ToApplyMods = ToApplyMods;
            _map = new Map(path);
        }

        [BackgroundDependencyLoader]
        private void Load(GameHost gameHost, CachedMap cachedMap)
        {
            IResourceStore<byte[]> store = new StorageBackedResourceStore(gameHost.Storage);
            ITrackStore trackStore = audio.GetTrackStore(store);

            var audioFile = $"{Path.GetDirectoryName(_map.Path)}{Path.DirectorySeparatorChar}{_map.AFileName}";
            track = trackStore.Get(audioFile);
            
            if (track != null)
                track.Volume.Value = Gameini.Get<double>(SettingsConfig.Volume);

            InternalChildren = new Drawable[]
            {
                GameplayScreenLoader = new GameplayScreenLoader
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
                BreakOverlay = new BreakOverlay(new Action[] { () => BreakOverlay.State.Value = Visibility.Hidden, () => ReturntoSongSelectionAfterFail.Value = true })
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 0f,
                },
                KeyPress[0] = GetSprite("Skin/K1", 0.3f),
                KeyPress[1] = GetSprite("Skin/K2", 0.4f),
                KeyPress[2] = GetSprite("Skin/K3", 0.5f),
                KeyPress[3] = GetSprite("Skin/K4", 0.6f),
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
            
            BreakOverlay.State.Value = Visibility.Hidden;
            BreakOverlay.State.ValueChanged += async (e) =>
            {
                if (e.NewValue == Visibility.Hidden)
                {
                    BreakOverlay.AnimationOut();
                    await Task.Delay(1500);
                    Resuming.Value = true;
                    rhythmBoxClockContainer.Start();
                    track.Start();
                    _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                _RbPlayfield = new Playfield.Playfield(ToApplyMods)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both, 
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 0.9f),
                    Map = _map,
                    Y = 0.02f
                },
                hpbar = new HPBar(ToApplyMods)
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft, 
                    RelativeSizeAxes = Axes.Both, 
                    Size = new Vector2(0.8f, 1f),
                    Colour = Color4.AliceBlue
                },
                DispayCombo = new TextFlowContainer
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.BottomLeft,
                },
                DispayScore = new TextFlowContainer
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

            rhythmBoxClockContainer.IsPaused.BindTo(IsPaused);
            rhythmBoxClockContainer.UserPlaybackRate.BindTo(UserPlaybackRate);

            _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            DispayScore.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            DispayCombo.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            hpbar.Clock = rhythmBoxClockContainer.RhythmBoxClock;

            _RbPlayfield.Resuming.BindTo(Resuming);

            DispayCombo.AddText("0x", x => x.Font = new FontUsage("Roboto", 40));
            DispayScore.AddText("000000", x => x.Font = new FontUsage("Roboto", 40));
            
            Score.Combo.PrivateComboBindable.ValueChanged += (e) => hpbar.CurrentValue.Value += hpbar.CalcValue(Score.Combo.currentHit);
            _RbPlayfield.HasFinished.ValueChanged += (e) =>
            {
                if (!e.NewValue) 
                    return;

                rhythmBoxClockContainer.Stop();

                var currentTime = track?.CurrentTime;
                track?.Stop();

                _RbPlayfield.HasFinished.UnbindEvents();

                cachedMap.Map = _map;
                cachedMap.LoadTrackFile();
                cachedMap.Seek(currentTime.GetValueOrDefault());

                Scheduler.AddDelayed(() => this.Expire(), 1000);
                LoadComponentAsync(new Selection(), this.Push);
            };

            ReturntoSongSelectionAfterFail.ValueChanged += (e) =>
            {
                cachedMap.Map = _map;
                cachedMap.LoadTrackFile();
                cachedMap.Seek(track.CurrentTime);

                Selection songSelction;
                LoadComponent(songSelction = new Selection());
                Schedule(() => this.Push(songSelction));
            };

            _RbPlayfield.CanStart.ValueChanged += (e) =>
            {
                if (e.NewValue) 
                    Load(1000);
            };
        }

        protected override void LoadComplete()
        {
            for (int i = 0; i < 4; i++)
            {
                Enum.TryParse(Gameini.Get<string>((SettingsConfig)i), out Key key);
                keys[i] = key;
            }

            base.LoadComplete();
        }

        private async void Load(int time)
        {
            GameplayScreenLoader.StartRotating();
            await Task.Delay(time);

            GameplayScreenLoader.StopRotating();
            GameplayScreenLoader.FadeOut(time, Easing.In).Delay(time).Finally((action) => GameplayScreenLoader.Expire());

            GameStarted.Value = true;

            rhythmBoxClockContainer.Start();
            track?.Start();
            
            rhythmBoxClockContainer.Seek(_map.StartTime);
            track?.Seek(_map.StartTime);
            
            if (!hpbar.Enabled.Value) return;
            hpbar.Drain(false);
        }

        protected override void Update()
        {
            if (hpbar.Enabled.Value && hpbar.CurrentValue.Value <= 0)
                if (!HasFailed)
                    Fail();

            DispayCombo.Text = DispayScore.Text = string.Empty;

            DispayCombo.AddText($"{Score.Combo.ComboInt}x", x => x.Font = new FontUsage("Roboto", 40));
            DispayScore.AddText(Score.Score.ScoreInt.ToString(), x => x.Font = new FontUsage("Roboto", 40));

            base.Update();
        }

        private async void Fail()
        {
            HasFailed = true;
            _RbPlayfield.Failed = true;

            Box box;

            AddInternal(box = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Colour = Color4.Red,
                Alpha = 0f,
            });

            box.FadeTo(0.7f, 500, Easing.In);

            foreach (var x in this._RbPlayfield)
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
            
            if (track == null)
                return;

            for (double i = track.Frequency.Value; i > 0; i -= 0.1d)
            {
                try
                {
                    track.Frequency.Value = i;
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
                if (Resuming.Value)
                {
                    Resuming.Value = false;
                    rhythmBoxClockContainer.Stop();
                    
                    track?.Stop();
                    BreakOverlay.ToggleVisibility();
                }

                if (HasFailed)
                    ReturntoSongSelectionAfterFail.Value = true;

                _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
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

            KeyPress[i].FadeTo(Down ? 0.5f : 1f, 50);
        }
        
        protected override bool OnScroll(ScrollEvent e)
        {
            volume.ChangeVolume(true, e);
            volume.FadeIn(100).OnComplete(x => x.Delay(1000).FadeOut(100));

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
            track?.Stop();
            Schedule(this.Exit);

            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            track?.Stop();
            return base.OnExiting(next);
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();
            base.Dispose(isDisposing);
        }
    }
}
