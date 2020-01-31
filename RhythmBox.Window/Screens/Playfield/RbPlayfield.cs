using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Threading;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;
using RhythmBox.Mode.Std.Objects;
using System;
using System.Collections.Generic;

namespace RhythmBox.Window.Playfield
{
    public class Playfield : Container
    {
        public Map Map;

        public RBox[] objBoxArray;

        public int _previousCombo = 0;

        public Hit currentHit { get; set; }

        //TODO:
        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        public BindableBool CanStart { get; set; } = new BindableBool();

        public BindableBool HasFinished = new BindableBool();

        private List<Mod> mods { get; set; }

        public Action action { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public Bindable<HitObjects.Direction> dir { get; set; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        private List<RBox> list { get; set; }

        private int pos = 0;

        public Playfield(List<Mod> mods)
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
                    dir = dir,
                    BoxAction = BoxAction,
                    BoxAction2 = BoxAction2,
                },
            };
        }

        protected override void LoadComplete()
        {
            LoadMap();
            list = new List<RBox>();
            list.AddRange(objBoxArray);

            CanStart.Value = true;

            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.W:
                    CheckClick(Key.W);
                    break;
                case Key.A:
                    CheckClick(Key.A);
                    break;
                case Key.S:
                    CheckClick(Key.S);
                    break;
                case Key.D:
                    CheckClick(Key.D);
                    break;
            }

            return base.OnKeyDown(e);
        }

        private void CheckClick(Key key)
        {
            HitObjects.Direction? dir = null;
            try
            {
                dir = GetNextObjDir(key);
            }
            catch { }

            if (dir != null)
                list[pos].OnClickKeyDown(key);
            //list.RemoveAt(pos);
        }

        private HitObjects.Direction? GetNextObjDir(Key key)
        {
            HitObjects.Direction? dir = null;

            for (int i = 0; i < list.Count; i++)
            {
                var x = list[i].direction;

                if (list[i].AlphaA > 0)
                {
                    if (key == Key.W)
                    {
                        if (x == HitObjects.Direction.Up)
                        {
                            dir = x;
                            pos = i;
                            break;
                        }
                    }
                    else if (key == Key.A)
                    {
                        if (x == HitObjects.Direction.Left)
                        {
                            dir = x;
                            pos = i;
                            break;
                        }
                    }
                    else if (key == Key.S)
                    {
                        if (x == HitObjects.Direction.Down)
                        {
                            dir = x;
                            pos = i;
                            break;
                        }
                    }
                    else if (key == Key.D)
                    {
                        if (x == HitObjects.Direction.Right)
                        {
                            dir = x;
                            pos = i;
                            break;
                        }
                    }
                }
            }

            return dir;
        }

        protected override void Update()
        {
            if (this.Clock.CurrentTime >= Map.EndTime)
            {
                HasFinished.Value = true;
            }

            base.Update();
        }

        private void LoadMap()
        {
            Score.Score.ResetScore();
            Score.Combo.ResetCombo();

            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                var x = (HitObjects)objBox;

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











            //CanStart.Value = false;
            //int i = 0;
            //int j = 0;
            //RemoveRange(objBoxArray);

            //foreach (var objBox in Map)
            //{
            //    //objBoxArray[i].Dispose();

            //    var x = (HitObjects)objBox;

            //    objBoxArray[i] = new RBox
            //    {
            //        Anchor = Anchor.Centre,
            //        Origin = Anchor.Centre,
            //        direction = x._direction,
            //        RelativeSizeAxes = Axes.Both,
            //        Size = new Vector2(1f),
            //        speed = x.Speed,
            //        Resuming = Resuming,
            //        mods = mods,
            //    };

            //    double SchedulerStartTime = x.Time;
            //    var calc = x.Time + (1500 * 1f);

            //    Logger.Log("LoadMapForEditor: " + SchedulerStartTime.ToString());
            //    Logger.Log("I: " + i.ToString());
            //    Logger.Log(calc.ToString());
            //    Logger.Log(time.ToString());

            //    var lol = calc - time;

            //    var lol2 = time - lol;

            //    if (true)
            //    {
            //        Scheduler.AddDelayed(() =>
            //        {
            //            Add(objBoxArray[j]);
            //            j++;
            //        }, 0);
            //    }
            //    else
            //    {
            //        j++;
            //    }
            //    i++;
            //}

            //CanStart.Value = true;

            //return;
        }
    }
}
