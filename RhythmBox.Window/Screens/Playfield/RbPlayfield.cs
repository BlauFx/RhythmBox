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

        public bool Failed { get; set; } = false;

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
                    if ((key == Key.W && x == HitObjects.Direction.Up) || key == Key.A && x == HitObjects.Direction.Left || key == Key.S && x == HitObjects.Direction.Down || key == Key.D && x == HitObjects.Direction.Right)
                    {
                        dir = x;
                        pos = i;
                        break;
                    }
                }
            }

            return dir;
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

            int i = 0;
            int j = 0;

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

                objBoxArray[i] = new RBox(x.Speed, x._direction, MaxStartSpeed)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Resuming = Resuming,
                    mods = mods,
                };

                Scheduler.AddDelayed(() =>
                {
                    Add(objBoxArray[j]);
                    j++;
                }, time);

                i++;
            }
        }
    }
}
