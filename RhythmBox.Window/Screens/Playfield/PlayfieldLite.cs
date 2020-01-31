using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osuTK;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Objects;
using RhythmBox.Window.Objects;
using System;
using System.Linq;

namespace RhythmBox.Window.Playfield
{
    public class PlayfieldLite : Container
    {
        public Map Map;

        public RBoxLite[] objBoxArrayLite;

        //TODO:
        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        public BindableBool CanStart { get; set; } = new BindableBool();

        public Action action { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public Bindable<HitObjects.Direction> dir { get; set; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        public ClickBox[] NewBox { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            objBoxArrayLite = new RBoxLite[Map.HitObjects.Length];

            Children = new Drawable[]
            {
                new RbDrawPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    action = action,
                    EditorMode = true,
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

        public void LoadMap()
        {
            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                var x = (HitObjects)objBox;

                objBoxArrayLite[i] = new RBoxLite(x.Time, 0)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                };

                Scheduler.AddDelayed(() =>
                {
                    Add(objBoxArrayLite[j]);
                    j++;
                }, x.Time - Map.StartTime);

                i++;
            }
        }

        public void LoadMap2(double time)
        {
            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                var x = (HitObjects)objBox;

                objBoxArrayLite[i] = new RBoxLite(x.Time, (float)time)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                };

                var Delay = x.Time - time;

                if (Delay <= 0)
                {
                    Delay = x.Time;
                }

                Logger.Log("Delay:" + Delay.ToString());
                Logger.Log("x.Time:" + x.Time.ToString());
                Logger.Log("time:" + time.ToString());

                Scheduler.AddDelayed(() =>
                {
                    Add(objBoxArrayLite[j]);
                    j++;
                }, Delay);

                i++;
            }
        }

        public void StopScheduler() => Scheduler.CancelDelayedTasks();

        public void AddHitObj(double time, HitObjects.Direction dir, float speed)
        {
            //Add(new RBoxLite(x.Time, (float) time, false)
            //{
            //    Anchor = Anchor.Centre,
            //    Origin = Anchor.Centre,
            //    direction = dir,
            //    RelativeSizeAxes = Axes.Both,
            //    Size = new Vector2(1f),
            //    speed = speed,
            //});

            //var Hitobj = new HitObjects()
            //{
            //    Speed = speed,
            //    Time = time,
            //    _direction = dir,
            //};

            //var list = Map.HitObjects.ToList();
            //list.Add(Hitobj);
            //Map.HitObjects = list.ToArray();
        }

        public void LoadMapForEditor(double time)
        {
            StopScheduler();

            RemoveRange(objBoxArrayLite);

            LoadMap2(time);
        }

        public void LoadMapForEditor2(double time, HitObjects.Direction dir, float speed)
        {
            StopScheduler();

            RemoveRange(objBoxArrayLite);

            CanStart.Value = false;

            int i = 0;
            int j = 0;

            var Hitobj = new HitObjects()
            {
                Speed = speed,
                Time = time,
                _direction = dir,
            };

            var list = Map.HitObjects.ToList();
            list.Add(Hitobj);

            Map.HitObjects = list.ToArray();

            objBoxArrayLite = new RBoxLite[list.Count];

            foreach (var objBox in list)
            {
                var x = objBox;

                objBoxArrayLite[i] = new RBoxLite(x.Time, (float)time)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    speed = x.Speed,
                };

                double SchedulerStartTime = x.Time - Map.StartTime;

                if (time <= SchedulerStartTime)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Remove(objBoxArrayLite[j]);

                        Add(objBoxArrayLite[j]);

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
