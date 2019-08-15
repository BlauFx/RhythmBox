using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Window.pending_files
{
    class RbPlayfield : Container
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
        
        public bool HasStarted { get; set; } = false;

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
            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
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
                                return base.OnKeyDown(e);
                            case Key.S:
                                x.OnClickKeyDown(Key.S);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                return base.OnKeyDown(e);
                            case Key.A:
                                x.OnClickKeyDown(Key.A);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                return base.OnKeyDown(e);
                            case Key.D:
                                x.OnClickKeyDown(Key.D);
                                this.UpdateCombo = x.AddComboToCounter();
                                this.AddMiss = x.Miss();
                                this.currentHit = x.GetHit();
                                return base.OnKeyDown(e);
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
                var CalcScore = (ComboCounter * addAmout);
                ScoreCounter += CalcScore;
            }

            HasFinished = HasAliveChildren();
            
            base.Update();
        }


        private void LoadMap()
        {
            int i = 0;
            
            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Interfaces.HitObjects) objBox;
                Add(objBoxArray[i] = new RBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = x.Time,
                    speed = x.Speed,
                });
                i++;
            }

            Scheduler.AddDelayed(() => { HasStarted = true; },objBoxArray[0].time);
        }
        
        private bool HasAliveChildren()
        {
            bool[] alive = new bool[objBoxArray.Length];
            for (int i = 0; i < objBoxArray.Length; i++)
            {
                if (objBoxArray[i].IsAlive)
                {
                    alive[i] = true;
                }
                else
                {
                    alive[i] = false;
                }
            }

            int j = 0;
            foreach (var x in alive)
            {
                if (x == false)
                {
                    j++;
                }
            }

            if (j == alive.Length)
            {
               return true; 
            }
            return false;
        }
    }
}
