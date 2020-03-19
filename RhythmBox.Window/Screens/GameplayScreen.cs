using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
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
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;
using RhythmBox.Window.Playfield;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RhythmBox.Window.Screens
{
    public class GameplayScreen : Screen
    {
        private double Accuracy { get; set; } = 100; //TODO:

        private TextFlowContainer DispayCombo;

        private TextFlowContainer DispayScore;

        private Map _map;

        private Playfield.Playfield _RbPlayfield;

        public HpBar _hpBar { get; set; }

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        public BindableDouble UserPlaybackRate = new BindableDouble(1) { Default = 1, MinValue = 0.1, MaxValue = 3, Precision = 0.1 };

        public readonly BindableBool IsPaused = new BindableBool();

        private BindableBool Resuming = new BindableBool(true);

        private bool HasFinished { get; set; } = true;

        public bool HasFailed { get; set; } = false;

        private BreakOverlay BreakOverlay;

        [Resolved]
        private AudioManager audio { get; set; }

        [Resolved]
        private GameHost gameHost { get; set; }

        private ITrackStore trackStore;

        private IResourceStore<byte[]> store;

        private Track track;

        private bool Resizing { get; set; } = false;

        private const float HP_Update = 80f;

        private const float HP_300 = 0.01f;

        private const float HP_100 = 0.005f;

        private const float HP_50 = 0.0025f;

        private const float HP_X = 0.1f;

        private const float HP_Drain = 0.001f;

        public BindableBool ReturntoSongSelectionAfterFail { get; set; } = new BindableBool();

        private BindableBool Startable = new BindableBool();

        private GameplayScreenLoader GameplayScreenLoader;

        private List<Mod> ToApplyMods;

        private bool FirstUpdate = true;

        public GameplayScreen(string path, List<Mod> ToApplyMods)
        {
            this.ToApplyMods = ToApplyMods;

            _map = new Map(path);
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            store = new StorageBackedResourceStore(gameHost.Storage);
            trackStore = audio.GetTrackStore(store);

            int num = _map.Path.LastIndexOf("\\");
            string tmp = _map.Path.Substring(0, num);

            string AudioFile = $"{tmp}\\{_map.AFileName}";
            track = trackStore.Get(AudioFile);

            track.Volume.Value = 0.1d;

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
                BreakOverlay = new BreakOverlay
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 0f,
                }
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
                    Size = new Vector2(0.6f, 1f),
                    Map = _map,
                },
                 _hpBar = new HpBar(0.1f)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    colour = Color4.AliceBlue,
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
            _hpBar.Clock = rhythmBoxClockContainer.RhythmBoxClock;

            _RbPlayfield.Resuming.BindTo(Resuming);

            DispayCombo.AddText("0x", x => x.Font = new FontUsage("Roboto", 40));
            DispayScore.AddText("000000", x => x.Font = new FontUsage("Roboto", 40));

            Score.Combo.PrivateComboBindable.ValueChanged += (e) =>
            {
                _hpBar.ResizeBox(CalcHpBarValue(_hpBar.CurrentValue, _hpBar.BoxMaxValue, 0f, Score.Combo.currentHit), (HP_Update / 1.5f), Easing.OutCirc);
            };

            _RbPlayfield.HasFinished.ValueChanged += (e) =>
            {
                if (e.NewValue == false) return;

                rhythmBoxClockContainer.Stop();
                track?.Stop();

                _RbPlayfield.HasFinished.UnbindEvents();

                Scheduler.AddDelayed(() => this.Expire(), 1000);
                LoadComponentAsync(new SongSelction(), this.Push);
            };

            ReturntoSongSelectionAfterFail.ValueChanged += (e) =>
            {
                rhythmBoxClockContainer.Stop();
                SongSelction songSelction;
                LoadComponent(songSelction = new SongSelction());
                Schedule(() => this.Push(songSelction));
            };

            Startable.ValueChanged += (e) =>
            {
                if (_RbPlayfield.CanStart.Value == true)
                {
                    Load(500);
                }
                else
                {
                    _RbPlayfield.CanStart.ValueChanged += (e2) =>
                    {
                        if (e2.NewValue == true)
                        {
                            Load(500);
                        }
                    };
                }
            };
        }

        protected override void LoadComplete()
        {
            Startable.Value = true;

            base.LoadComplete();
        }

        private async void Load(int time)
        {
            GameplayScreenLoader.StopRotaing(time);

            GameplayScreenLoader.FadeOut(time, Easing.In).Delay(time).Finally((Action) => GameplayScreenLoader.Expire());

            await Task.Delay(time);

            rhythmBoxClockContainer.Seek(_map.StartTime);
            track?.Seek(_map.StartTime);

            rhythmBoxClockContainer.Start();
            track?.Start();

            UpdateHPBar();
        }

        private void UpdateHPBar()
        {
            _hpBar.ResizeBox(CalcHpBarValue(_hpBar.BoxMaxValue, _hpBar.BoxMaxValue, 0f, Hit.Hit100, true), HP_Update, Easing.OutCirc);

            Scheduler.AddDelayed(() =>
            {
                _hpBar.ResizeBox(CalcHpBarValue(_hpBar.CurrentValue, _hpBar.BoxMaxValue, 0f, Hit.Hit100, true), HP_Update, Easing.OutCirc);
            }, HP_Update, true);
            FirstUpdate = false;
        }

        protected override void Update()
        {
            if (_hpBar.CurrentValue <= 0)
            {
                if (FirstUpdate)
                {
                    return;
                }
                else if (!HasFailed)
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
                        if (x is RbDrawPlayfield)
                        {
                            foreach (var y in (x as RbDrawPlayfield))
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

                    _ = AddJustTrack();
                }
            }

            DispayCombo.Text = string.Empty;
            DispayCombo.AddText($"{Score.Combo.ComboInt}x", x => x.Font = new FontUsage("Roboto", 40));

            DispayScore.Text = string.Empty;
            DispayScore.AddText($"{Score.Score.ScoreInt}", x => x.Font = new FontUsage("Roboto", 40));

            base.Update();
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
                _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            }

            return base.OnKeyDown(e);
        }

        private float CalcHpBarValue(float currentvalue, float maxvalue, float minvalue, Hit hit, bool auto = false)
        {
            if (!auto)
            {
                float result = hit switch
                {
                    Hit.Hit300 => currentvalue + HP_300,
                    Hit.Hit100 => currentvalue + HP_100,
                    Hit.Hit50 => currentvalue + HP_50,
                    Hit.Hitx => currentvalue - HP_X * 10,
                    _ => 0
                };

                if (result < maxvalue && result > minvalue)
                    return result;
                else if (result > maxvalue)
                    return maxvalue;
                else if (result < minvalue)
                    return minvalue;
            }

            return currentvalue - HP_Drain;
        }

        private async Task AddJustTrack()
        {
            if (track == null) return;

            for (double i = track.Frequency.Value; i > 0; i -= 0.1d)
            {
                try
                {
                    track.Frequency.Value = i;
                }
                catch { }
                await Task.Delay(500);
            }

            //ReturntoSongSelectionAfterFail.Value = true;
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero<GameplayScreen>(500, Easing.In);
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            track?.Stop();

            Scheduler.AddDelayed(() => this.Exit(), 0);

            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            track?.Stop();

            return base.OnExiting(next);
        }
    }
}
