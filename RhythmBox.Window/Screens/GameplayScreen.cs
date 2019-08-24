using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;
using System.IO;
using System.Reflection;
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

        private BreakOverlay BreakOverlay;

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
            };
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            InternalChildren = new Drawable[]
            {
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

            rhythmBoxClockContainer.Seek(_map.StartTime);
            rhythmBoxClockContainer.Start();
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
                    this.Push(songSelction);
                }
            }
            else
            {
                _hpBar.ResizeBox(CalcHpBarValue(_hpBar._box.Width, _hpBar.BoxMaxValue, 0f, Hit.Hit100, true), 10000, Easing.OutCirc);

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
                    BreakOverlay.ToggleVisibility();
                }
                _RbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;
            }
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
                        result = currentvalue * 1.5f;
                        break;

                    case Hit.Hit100:
                        result = currentvalue * 0.8f;
                        break;

                    case Hit.Hit50:
                        result = currentvalue * 0.7f;
                        break;

                    case Hit.Hitx:
                        result = currentvalue * 0.3f;
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

            return currentvalue * 0.995f;
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero<GameplayScreen>(500, Easing.In);
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            //If this screen is faded to 0 then the screen isn't exiting.
            this.FadeTo<GameplayScreen>(0.01f, 0, Easing.In);
            Scheduler.AddDelayed(() => this.Exit(), 0);

            base.OnSuspending(next);
        }
    }
}
