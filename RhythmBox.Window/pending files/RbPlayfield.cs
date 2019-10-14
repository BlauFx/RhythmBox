using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Threading;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;
using RhythmBox.Mode.Std.Objects;
using RhythmBox.Window.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhythmBox.Window.pending_files
{
    public class RbPlayfield : Container
    {
        public Map Map;

        public RBox[] objBoxArray;

        public int _previousCombo = 0;

        public int ScoreCounter = 0;

        private bool UpdateCombo = false;

        private bool AddMiss = false;

        public Hit currentHit { get; set; }

        //TODO:
        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        public BindableBool CanStart = new BindableBool();

        public BindableBool HasFinished = new BindableBool();

        public BindableInt ComboCounter = new BindableInt(0);

        private List<Mod> mods;

        public Action action { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public bool EditorMode { get; set; }

        public Bindable<HitObjects.Direction> dir { get; set; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        public ClickBox[] NewBox { get; set; }

        public RbPlayfield(List<Mod> mods)
        {
            this.mods = mods;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            objBoxArray = new RBox[Map.HitObjects.Length];

            Children = new Drawable[]
            {
                new RbDrawPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    action = action,
                    EditorMode = EditorMode,
                    dir = dir,
                    BoxAction = BoxAction,
                    BoxAction2 = BoxAction2,
                    NewBox = NewBox,
                },
            };
        }

        protected override void LoadComplete()
        {
            LoadMap();
            CanStart.Value = true;

            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            //Info: Using return base.OnKeyDown(e); instead of break is adding a notelock
            for (int i = 0; i < objBoxArray.Length; i++)
            {
                var x = objBoxArray[i];
                if (x.AlphaA > 0)
                {
                    try
                    {
                        switch (e.Key)
                        {
                            case Key.W:
                                x.OnClickKeyDown(Key.W);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;

                            case Key.S:
                                x.OnClickKeyDown(Key.S);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;

                            case Key.A:
                                x.OnClickKeyDown(Key.A);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;

                            case Key.D:
                                x.OnClickKeyDown(Key.D);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;
                        }
                    }
                    catch { }
                }
            }
            return base.OnKeyDown(e);
        }

        protected override void Update()
        {
            if (UpdateCombo)
            {
                UpdateCombo = false;
                _previousCombo = ComboCounter.Value;
                ComboCounter.Value++;
            }
            else if (AddMiss)
            {
                _previousCombo = ComboCounter.Value;
                ComboCounter.Value = 0;
            }

            if (ComboCounter.Value != _previousCombo)
            {
                _previousCombo = ComboCounter.Value;
                int addAmout = 0;
                switch (currentHit)
                {
                    case Hit.Hit300:
                        addAmout = 300;
                        break;

                    case Hit.Hit100:
                        addAmout = 100;
                        break;

                    case Hit.Hit50:
                        addAmout = 50;
                        break;

                    case Hit.Hitx:
                        addAmout = 0;
                        break;
                }

                //TODO: Maybe change the way how we calculate the score?
                var CalcScore = (ComboCounter.Value * addAmout);
                ScoreCounter += CalcScore;
            }

            if (this.Clock.CurrentTime >= Map.EndTime)
            {
                HasFinished.Value = true;
            }

            base.Update();
        }

        private void LoadMap()
        {
            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                var x = (HitObjects) objBox;

                objBoxArray[i] = new RBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                    Resuming = Resuming,
                    mods = mods,
                };


                Scheduler.AddDelayed(() =>
                {
                    Add(objBoxArray[j]);
                    j++;
                }, x.Time - Map.StartTime);

                i++;
            }
        }

        public void StopScheduler() => Scheduler.CancelDelayedTasks();

        public void LoadMapForEditor(double time)
        {
            CanStart.Value = false;
            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                //objBoxArray[i].Dispose();

                var x = (HitObjects) objBox;

                objBoxArray[i] = new RBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                    Resuming = Resuming,
                    mods = mods,
                };

                double SchedulerStartTime = x.Time - Map.StartTime;

                if (time <= SchedulerStartTime)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Remove(objBoxArray[j]);

                        Add(objBoxArray[j]);

                        j++;
                    }, SchedulerStartTime - time);
                }
                else
                {
                    j++;
                }
                i++;
            }

            CanStart.Value = true;
        }

        public void LoadMapForEditor2(double time, HitObjects.Direction direction)
        {
            CanStart.Value = false;
            int i = 0;
            int j = 0;

            var Hitobj = new HitObjects()
            {
                Speed = 1f,
                Time = time,
                _direction = direction,
            };

            var list = Map.HitObjects.ToList();
            list.Add(Hitobj);

            list.ToArray();

            objBoxArray = new RBox[list.Count];

            foreach (var objBox in list)
            {
                //objBoxArray[i].Dispose();

                var x = (HitObjects) objBox;

                objBoxArray[i] = new RBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                    Resuming = Resuming,
                    mods = mods,
                };

                double SchedulerStartTime = x.Time - Map.StartTime;

                if (time <= SchedulerStartTime)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Remove(objBoxArray[j]);

                        Add(objBoxArray[j]);

                        j++;
                    }, SchedulerStartTime - time);
                }
                else
                {
                    j++;
                }
                i++;
            }

            CanStart.Value = true;
        }
    }
}
