using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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

        public Hit currentHit { get; set; }

        //TODO:
        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        public BindableBool CanStart = new BindableBool();

        public BindableBool HasFinished = new BindableBool();

        private List<Mod> mods;

        public Action action { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public bool EditorMode { get; set; }

        public Bindable<HitObjects.Direction> dir { get; set; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        public ClickBox[] NewBox { get; set; }

        private List<RBox> list { get; set; }

        private int pos = 0;

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
            var nxtobjDir = GetNextObjDir(key);

            if (nxtobjDir != null)
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

            objBoxArray[i] = new RBox(x.Time - Map.StartTime)
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

            var x = (HitObjects)objBox;

            objBoxArray[i] = new RBox(x.Time - Map.StartTime)
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

    public void LoadMapForEditor2(double time, HitObjects.Direction direction, float speed)
    {
        CanStart.Value = false;
        int i = 0;
        int j = 0;

        var Hitobj = new HitObjects()
        {
            Speed = speed,
            Time = time,
            _direction = direction,
        };

        var list = Map.HitObjects.ToList();
        list.Add(Hitobj);

        Map.HitObjects = list.ToArray();

        objBoxArray = new RBox[list.Count];

        foreach (var objBox in list)
        {
            //objBoxArray[i].Dispose();

            var x = (HitObjects)objBox;

            objBoxArray[i] = new RBox(x.Time - Map.StartTime)
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
