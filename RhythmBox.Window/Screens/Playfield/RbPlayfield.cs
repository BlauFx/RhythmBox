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
using RhythmBox.Window.Playfield;

namespace RhythmBox.Window.Screens.Playfield
{
    public class Playfield : Container
    {
        public Map Map;

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

        public Bindable<HitObjects.Direction> dir { get; } = new Bindable<HitObjects.Direction>(HitObjects.Direction.Up);

        public List<Tuple<RBox, float>> objectList { get; } = new List<Tuple<RBox, float>>();

        private int pos = 0;

        public bool Failed { get; set; } = false;

        [Resolved]
        private Gameini gameini { get; set; }

        private Key[] keys = new Key[4];

        public Playfield(List<Mod> mods)
        {
            this.mods = mods;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
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
            if (e.Key == keys[0] || e.Key == keys[1] || e.Key == keys[2] || e.Key == keys[3])
            {
                CheckClick(e.Key);
            }

            return base.OnKeyDown(e);
        }

        private void CheckClick(Key key)
        {
            HitObjects.Direction? direction = null;

            try
            {
                direction = GetNextObjDir(key);
            }
            catch { }

            if (direction != null)
            {
                objectList[pos].Item1.OnClickKeyDown(key);
                //objectList.RemoveAt(pos);
            }
        }

        private HitObjects.Direction? GetNextObjDir(Key key)
        {
            HitObjects.Direction? direction = null;

            for (int i = 0; i < objectList.Count; i++)
            {
                var x = objectList[i].Item1.direction;

                if (objectList[i].Item1.obj != null && objectList[i].Item1.AlphaA > 0 && objectList[i].Item1.obj.IsAlive)
                {
                    if ((key == keys[0] && x == HitObjects.Direction.Up) || key == keys[1] && x == HitObjects.Direction.Left || key == keys[2] && x == HitObjects.Direction.Down || key == keys[3] && x == HitObjects.Direction.Right)
                    {
                        direction = x;
                        pos = i;
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

            foreach (var objBox in Map)
            {
                var x = (HitObjects)objBox;

                int MinStartSpeed = 200;
                double MaxStartSpeed = x.Speed * 1000;
                
                if (MaxStartSpeed < MinStartSpeed)
                    MaxStartSpeed = MinStartSpeed;

                var time = x.Time - MaxStartSpeed - Map.StartTime;

                if (x.Time - MaxStartSpeed < 0)
                {
                    time = 0;
                    MaxStartSpeed = x.Time;
                }

                objectList.Add(new Tuple<RBox, float>(new RBox(x.Speed, x._direction, MaxStartSpeed, keys)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Resuming = Resuming,
                    mods = mods,
                }, (float)time));
            }

            objectList.ForEach(x => Scheduler.AddDelayed(() => Add(x.Item1), x.Item2));
        }
    }
}
