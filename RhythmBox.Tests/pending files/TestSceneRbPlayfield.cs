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

        private TestSceneRBox[] objBoxArray;  // = new TestSceneRBox[4];

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
//                objBoxArray[0] = new TestSceneRBox
//                {
//                    Anchor = Anchor.Centre,
//                    Origin = Anchor.Centre,
//                    direction = Direction.Up,
//                    RelativeSizeAxes = Axes.Both,
//                    Size = new Vector2(1f),
//                    time = 699,
//                    speed = 1f,
//                },
//                objBoxArray[1] = new TestSceneRBox
//                {
//                    Anchor = Anchor.Centre,
//                    Origin = Anchor.Centre,
//                    direction = Direction.Left,
//                    RelativeSizeAxes = Axes.Both,
//                    Size = new Vector2(1f),
//                    time = 700,
//                    speed = 1f,
//                },
//                objBoxArray[2] = new TestSceneRBox
//                {
//                    Anchor = Anchor.Centre,
//                    Origin = Anchor.Centre,
//                    direction = Direction.Down,
//                    RelativeSizeAxes = Axes.Both,
//                    Size = new Vector2(1f),
//                    time = 1200,
//                    speed = 1f,
//                },
//                objBoxArray[3] = new TestSceneRBox
//                {
//                    Anchor = Anchor.Centre,
//                    Origin = Anchor.Centre,
//                    direction = Direction.Right,
//                    RelativeSizeAxes = Axes.Both,
//                    Size = new Vector2(1f),
//                    time = 3100,
//                    speed = 1f,
//                },
            };
        }

        protected override void LoadComplete()
        {
            LoadMap();
            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
//            return base.OnKeyDown(e);
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
            return base.OnKeyDown(e);
        }

        private void LoadMap()
        {
            int i = 0;
            
            //TODO
            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Tests.Interfaces.HitObjects) objBox;
                Logger.Log(x._direction.ToString());
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
