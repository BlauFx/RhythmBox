using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Window.Mode.Standard.Animations;
using RhythmBox.Window.Mode.Standard.Maps;
using RhythmBox.Window.Mode.Standard.Mods;
using RhythmBox.Window.Mode.Standard.Objects;

namespace RhythmBox.Window.Screens.Playfield
{
    public class Playfield : Container
    {
        public Map Map;

        public Hit currentHit { get; set; }

        //TODO:
        public bool HasStarted { get; set; } = false;

        public readonly BindableBool Resuming = new BindableBool();

        public BindableBool CanStart { get; set; } = new BindableBool();

        public readonly BindableBool HasFinished = new BindableBool();

        private List<Mod> mods { get; set; }

        public Action action { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public Bindable<HitObject.Direction> dir { get; } = new Bindable<HitObject.Direction>(HitObject.Direction.Up);

        public List<Tuple<HitBox, double>> objectList { get; } = new List<Tuple<HitBox, double>>();

        public bool Failed { get; set; }

        [Resolved]
        private Gameini gameini { get; set; }

        private readonly Key[] keys = new Key[4];

        private Tuple<HitBox, double> currentobj;

        public Playfield(List<Mod> mods)
        {
            this.mods = mods;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = new DrawPlayfield
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                action = action,
                dir = dir,
                BoxAction = BoxAction,
                BoxAction2 = BoxAction2,
            };
        }

        protected override void LoadComplete()
        {
            for (int i = 0; i < 4; i++)
            {
                Enum.TryParse(gameini.Get<string>((SettingsConfig)i ), out Key key);
                keys[i] = key;
            }
            
            LoadMap();
            CanStart.Value = true;

            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            //TODO: Add check if key was previously up so the cannot be pressed always
            if (e.Key == keys[0] || e.Key == keys[1] || e.Key == keys[2] || e.Key == keys[3])
                CheckClick(e.Key);

            return base.OnKeyDown(e);
        }

        private void CheckClick(Key key)
        {
            var nextHit = GetNextHit(key);

            if (nextHit?.Item1 != null)
            {
                nextHit.Item1?.ClickKeyDown(key);

                if (objectList.Count > 0)
                    objectList.Remove(nextHit);
            }
        }

        private Tuple<HitObject.Direction?, int> GetNextObjDir(Key key)
        {
            Tuple<HitObject.Direction?, int> direction = null;

            for (int i = 0; i < objectList.Count; i++)
            {
                var x = objectList[i].Item1.direction;

                if (objectList[i].Item1 != null && objectList[i].Item1.Alpha > 0 && objectList[i].Item1.IsAlive)
                {
                    if (key == keys[0] && x == HitObject.Direction.Up || key == keys[1] && x == HitObject.Direction.Left || key == keys[2] && x == HitObject.Direction.Down || key == keys[3] && x == HitObject.Direction.Right)
                    {
                        direction = new Tuple<HitObject.Direction?, int>(x, i);
                        break;
                    }
                }
            }

            return direction;
        }

        private Tuple<HitBox, double> GetNextHit(Key key)
        {
            if (objectList.Count <= 0) 
                return null;
            
            var obj = objectList.FirstOrDefault(x => x.Item1 != null && x.Item1.IsPresent && x.Item1.IsAlive && (key == keys[0] && x.Item1.direction == HitObject.Direction.Up || key == keys[1] && x.Item1.direction == HitObject.Direction.Left || key == keys[2] &&  x.Item1.direction == HitObject.Direction.Down || key == keys[3] &&  x.Item1.direction == HitObject.Direction.Right));
            return obj;
        }

        protected override void Update()
        {
            if (this.Clock.CurrentTime >= Map.EndTime && !this.Failed)
                HasFinished.Value = true;

            if (SpawnNextObj(this.Clock.CurrentTime))
            {
                if (!currentobj.Item1.IsAlive)
                    AddInternal(currentobj.Item1); 
            }

            base.Update();
        }

        private bool SpawnNextObj(double time)
        {
            if (objectList.Count <= 0)
                return false;
            
            currentobj = objectList.FirstOrDefault(x => x.Item2 >= time && (!x.Item1.IsAlive || !x.Item1.IsPresent));
            return currentobj != null && CloseEnoughForMe(currentobj.Item2, time, 1000d);
        }

        //https://stackoverflow.com/a/3420834
        private static bool CloseEnoughForMe(double value1, double value2, double acceptableDifference) => Math.Abs(value1 - value2) <= acceptableDifference;

        private void LoadMap()
        {
            Score.Score.ResetScore();
            Score.Combo.ResetCombo();

            foreach (var objBox in Map.HitObjects)
            {
                if (objBox.Time < Map.StartTime)
                    continue;

                var duration = objBox.Speed * 1000f;

                objectList.Add(new Tuple<HitBox, double>(new HitBox(objBox._direction, duration, keys)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Resuming = Resuming,
                    mods = mods,
                }, objBox.Time));
            }

            // for (var index = 0; index < objectList.Count; index++)
            // {
            //     var (item1, item2) = objectList[index];
            //     Scheduler.AddDelayed(() => AddInternal(item1), item2);
            // }
        }
    }
}
