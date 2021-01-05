using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;
using RhythmBox.Mode.Std.Objects;

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

        public Bindable<HitObjects.Direction> dir { get; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        public List<Tuple<HitBox, double>> objectList { get; } = new List<Tuple<HitBox, double>>();

        public bool Failed { get; set; }

        [Resolved]
        private Gameini gameini { get; set; }

        private readonly Key[] keys = new Key[4];

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
            Tuple<HitObjects.Direction?, int> direction = null;

            try
            {
                direction = GetNextObjDir(key);
            }
            catch { }

            if (direction != null)
            {
                objectList[direction.Item2].Item1.ClickKeyDown(key);
                objectList.RemoveAt(direction.Item2);
            }
        }

        private Tuple<HitObjects.Direction?, int> GetNextObjDir(Key key)
        {
            Tuple<HitObjects.Direction?, int> direction = null;

            for (int i = 0; i < objectList.Count; i++)
            {
                var x = objectList[i].Item1.direction;

                if (objectList[i].Item1 != null && objectList[i].Item1.Alpha > 0 && objectList[i].Item1.IsAlive)
                {
                    if ((key == keys[0] && x == HitObjects.Direction.Up) || key == keys[1] && x == HitObjects.Direction.Left || key == keys[2] && x == HitObjects.Direction.Down || key == keys[3] && x == HitObjects.Direction.Right)
                    {
                        direction = new Tuple<HitObjects.Direction?, int>(x, i);
                        break;
                    }
                }
            }

            return direction;
        }

        protected override void Update()
        {
            if (this.Clock.CurrentTime >= Map.EndTime && !this.Failed)
                HasFinished.Value = true;

            base.Update();
        }

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

            //TODO: If objectList is very large then it may crash due to Scheduler because it can not handle that many tasks 
            for (var index = 0; index < objectList.Count; index++)
            {
                var (item1, item2) = objectList[index];
                Scheduler.AddDelayed(() => AddInternal(item1), item2);
            }
        }
    }
}
