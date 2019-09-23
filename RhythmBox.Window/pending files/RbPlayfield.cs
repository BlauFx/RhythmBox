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
using System.Collections.Generic;

namespace RhythmBox.Window.pending_files
{
    internal class RbPlayfield : Container
    {
        public Map Map;

        private RBox[] objBoxArray;

        public int ComboCounter = 0;

        private int _previousCombo = 0;

        public int ScoreCounter = 0;

        private bool UpdateCombo = false;

        private bool AddMiss = false;

        public Hit currentHit { get; set; }

        public bool HasFinished { get; set; } = false;

        //TODO:
        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        public BindableBool CanStart = new BindableBool();

        private List<Mod> mods;

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
            foreach (var x in objBoxArray)
            {
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
                                //return base.OnKeyDown(e);

                            case Key.S:
                                x.OnClickKeyDown(Key.S);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;
                                //return base.OnKeyDown(e);

                            case Key.A:
                                x.OnClickKeyDown(Key.A);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;
                                //return base.OnKeyDown(e);

                            case Key.D:
                                x.OnClickKeyDown(Key.D);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                break;
                                //return base.OnKeyDown(e);
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
                _previousCombo = ComboCounter;
                ComboCounter++;
            }
            else if (AddMiss)
            {
                _previousCombo = ComboCounter;
                ComboCounter = 0;
            }

            if (ComboCounter != _previousCombo)
            {
                _previousCombo = ComboCounter;
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
                var CalcScore = (ComboCounter * addAmout);
                ScoreCounter += CalcScore;
            }

            if (this.Clock.CurrentTime >= Map.EndTime)
            {
                HasFinished = true;
            }

            base.Update();
        }

        private void LoadMap()
        {
            int i = 0;
            int j = 0;

            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Interfaces.HitObjects) objBox;

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
    }
}