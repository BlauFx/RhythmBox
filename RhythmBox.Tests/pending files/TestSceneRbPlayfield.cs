using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Animations;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbPlayfield : Container
    {
        public TestSceneMap Map;

        private TestSceneRBox[] objBoxArray;

        public int ComboCounter = 0;

        private int _previousCombo = 0;

        public int ScoreCounter = 0;

        private bool UpdateCombo = false;

        private bool AddMiss = false;

        public Hit currentHit { get; set; }

        public bool HasFinished { get; set; } = false;

        public bool HasStarted { get; set; } = false;

        public BindableBool Resuming = new BindableBool();

        [BackgroundDependencyLoader]
        private void Load()
        {
            objBoxArray = new TestSceneRBox[Map.HitObjects.Length];

            Children = new Drawable[]
            {
                new TestSceneRbDrawPlayfield
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

            if (this.Clock.CurrentTime >= Map.EndTime)
            {
                HasFinished = true;
            }
            base.Update();
        }

        private void LoadMap()
        {
            int i = 0;

            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Tests.Interfaces.HitObjects)objBox;
                Add(objBoxArray[i] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    //time = x.Time,
                    time = x.Time - Map.StartTime,
                    speed = x.Speed,
                    Resuming = Resuming,
                });
                i++;
            }
        }
    }
}