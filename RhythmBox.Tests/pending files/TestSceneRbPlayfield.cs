using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbPlayfield : Container
    {
        public TestSceneMap Map;

        private TestSceneRBox objBox;

        private TestSceneRBox[] objBoxArray;

        public TextFlowContainer xd  { get; set; } //TODO

        public string xdWrapper { get; set; } = "s";

        [BackgroundDependencyLoader]
        private void Load()
        {
            objBoxArray = new TestSceneRBox[Map.HitObjects.Length];
            
            Children = new Drawable[]
            {
                xd = new TextFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.TopRight,
                    X = -0.01f,
                    Alpha = 0f,
                },
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
                                return base.OnKeyDown(e);
                            case Key.S:
                                x.OnClickKeyDown(Key.S);
                                return base.OnKeyDown(e);
                            case Key.A:
                                x.OnClickKeyDown(Key.A);
                                return base.OnKeyDown(e);
                            case Key.D:
                                x.OnClickKeyDown(Key.D);
                                return base.OnKeyDown(e);
                        }
                    }
                    catch  {  }
                }
            }
            xd.Text = "WWSEDSE";
            xdWrapper = "WWSEDSE";
            return base.OnKeyDown(e);
        }
        

        private void LoadMap()
        {
            int i = 0;
            
            //TODO
            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Tests.Interfaces.HitObjects) objBox;
                Add(objBoxArray[i] = new TestSceneRBox
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
        }
    }
}
