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
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;
using System.Threading.Tasks;

namespace RhythmBox.Window.Screens
{
    public class GameplayScreen : Screen
    {
        private int Score { get; set; } = 0;

        private int Combo { get; set; } = 0;

        private double Accuracy { get; set; } = 100; //TODO:

        private TextFlowContainer DispayCombo;

        private TextFlowContainer DispayScore;

        private Map _map;

        private RbPlayfield _RbPlayfield;

        private Mode.Std.Animations.HpBar _hpBar;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private Bindable<double> UserPlaybackRate = new BindableDouble(1);

        public readonly BindableBool IsPaused = new BindableBool();

        private BindableBool Resuming = new BindableBool(true);

        private bool HasFinished { get; set; } = true;

        private bool HasFailed { get; set; } = false;

        private BreakOverlay BreakOverlay;

        [Resolved]
        private AudioManager audio { get; set; }

        [Resolved]
        private GameHost gameHost { get; set; }

        private ITrackStore trackStore;

        private IResourceStore<byte[]> store;

        private Track track;

        private bool Resizing { get; set; } = false;

        private const float HP_Update = 100f;

        private const float HP_300 = 0.2f;

        private const float HP_100 = 0.1f;

        private const float HP_50 = 0.05f;

        private const float HP_X = 0.05f;

        private const float HP_Drain = 0.001f;

        private BindableBool bindableBool = new BindableBool();

        private BindableBool Startable = new BindableBool();

        private GameplayScreenLoader GameplayScreenLoader;

        public GameplayScreen(string path)
        {
            var MapReader = new MapReader(path);
            _map = new Map
            {
                AFileName = MapReader.AFileName,
                BGFile = MapReader.BGFile,
                MapId = MapReader.MapId,
                MapSetId = MapReader.MapSetId,
                BPM = MapReader.BPM,
                Objects = MapReader.Objects,
                AutoMap = MapReader.AutoMap,
                Mode = MapReader.Mode,
                Title = MapReader.Title,
                Artist = MapReader.Artist,
                Creator = MapReader.Creator,
                DifficultyName = MapReader.DifficultyName,
                StartTime = MapReader.StartTime,
                EndTime = MapReader.EndTime,
                HitObjects = MapReader.HitObjects,
                Path = MapReader.Path,
            };
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
                _RbPlayfield = new RbPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = _map,
                },
                 _hpBar = new Mode.Std.Animations.HpBar
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    BoxMaxValue = 0.1f,
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
        }

        protected override void Update()
        {
            if (_RbPlayfield.HasFinished)
            {
                if (HasFinished)
                {
                    HasFinished = false;
                    rhythmBoxClockContainer.Stop();
                    SongSelction songSelction;
                    LoadComponent(songSelction = new SongSelction());
                    Schedule(() => this.Push(songSelction));
                }
            }
            else
            {
                if (_hpBar.CurrentValue <= 0)
                {
                    if (!HasFailed)
                    {
                        HasFailed = true;

                        Box box;

                        AddInternal(box = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(1f),
                            Colour = Color4.Red,
                            Alpha = 0f,
                        });

                        box.FadeTo(0.7f, 500, Easing.In);

                        bindableBool.ValueChanged += (e) =>
                        {
                            Logger.Log("GameplayScreen: bindableBool.Value changed", LoggingTarget.Runtime, LogLevel.Debug);
                            rhythmBoxClockContainer.Stop();
                            SongSelction songSelction;
                            LoadComponent(songSelction = new SongSelction());
                            Schedule(() => this.Push(songSelction));
                        };

                        foreach (var x in this._RbPlayfield)
                        {
                            if (x is RbDrawPlayfield)
                            {
                                foreach (var y in (x as RbDrawPlayfield))
                                {
                                    y.TransformTo(nameof(Shear), new Vector2(osu.Framework.MathUtils.RNG.NextSingle(-0.15f, 0.15f)), 1000, Easing.In);
                                    y.TransformTo(nameof(Scale), new Vector2(osu.Framework.MathUtils.RNG.NextSingle(1.1f, 2f)), 1000, Easing.In);
                                }
                            }
                            else
                            {
                                x.TransformTo(nameof(Shear), new Vector2(osu.Framework.MathUtils.RNG.NextSingle(-0.15f, 0.15f)), 1000, Easing.In);
                                x.TransformTo(nameof(Scale), new Vector2(osu.Framework.MathUtils.RNG.NextSingle(0.6f, 2f)), 1000, Easing.In);
                                x.MoveToOffset(new Vector2(osu.Framework.MathUtils.RNG.NextSingle(0.1f, 0.4f)), 1000, Easing.In);
                            }
                        }

                        _ = AddJustTrack();
                    }
                }
                else
                {
                    if (!Resizing)
                    {
                        Resizing = true;
                        _hpBar.ResizeBox(CalcHpBarValue(_hpBar._box.Width, _hpBar.BoxMaxValue, 0f, Hit.Hit100, true), HP_Update, Easing.OutCirc);
                        Scheduler.AddDelayed(() => Resizing = false, HP_Update);
                    }
                }

                Combo = _RbPlayfield.ComboCounter;
                DispayCombo.Text = string.Empty;
                DispayCombo.AddText($"{Combo}x", x => x.Font = new FontUsage("Roboto", 40));

                Score = _RbPlayfield.ScoreCounter;
                DispayScore.Text = string.Empty;
                DispayScore.AddText($"{Score}", x => x.Font = new FontUsage("Roboto", 40));
            }

            base.Update();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                if (Resuming.Value)
                {
                    Resuming.Value = false;
                    rhythmBoxClockContainer.Stop();
                    track.Stop();
                    BreakOverlay.ToggleVisibility();
                }
                _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            }

            //TODO: hpbar may be resized on key event even if no obj is alive
            //TODO: Check if e.Key == Game.Key
            _hpBar.ResizeBox(CalcHpBarValue(_hpBar._box.Width, _hpBar.BoxMaxValue, 0f, _RbPlayfield.currentHit), 1000, Easing.OutCirc);
            return base.OnKeyDown(e);
        }

        private float CalcHpBarValue(float currentvalue, float maxvalue, float minvalue, Hit hit, bool auto = false)
        {
            if (!auto)
            {
                float result = 0;
                switch (hit)
                {
                    case Hit.Hit300:
                        result = currentvalue + HP_300;
                        break;

                    case Hit.Hit100:
                        result = currentvalue + HP_100;
                        break;

                    case Hit.Hit50:
                        result = currentvalue + HP_50;
                        break;

                    case Hit.Hitx:
                        result = currentvalue - HP_X;
                        break;
                }

                if (result < maxvalue && result > minvalue)
                {
                    return result;
                }
                else if (result > maxvalue)
                {
                    return maxvalue;
                }
                else if (result < minvalue)
                {
                    return minvalue;
                }
            }

            return currentvalue - HP_Drain;
        }

        private async Task AddJustTrack()
        {
            await Task.Run(async () =>
            {
                for (double i = track.Frequency.Value; i > 0; i -= 0.01d)
                {
                    try
                    {
                        track.Frequency.Value = i;
                    }
                    catch { }
                    await Task.Delay(1);
                }
            });

            bindableBool.Value = true;
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
    }
}
