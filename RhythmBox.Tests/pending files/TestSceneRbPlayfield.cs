using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Mode.Std.Tests.Objects;
using Direction = RhythmBox.Mode.Std.Tests.Objects.Direction;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbPlayfield : Container
    {
        public TestSceneMap Map;

        private TestSceneRBox objBox;

        private TestSceneRBox[] objBoxArray = new TestSceneRBox[4];

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new TestSceneRbDrawPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                }, 
                objBoxArray[0] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Up,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 699,
                    speed = 1f,
                },
                objBoxArray[1] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Left,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 700,
                    speed = 1f,
                },
                objBoxArray[2] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Down,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 1200,
                    speed = 1f,
                },
                objBoxArray[3] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Right,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 3100,
                    speed = 1f,
                },
            };
            LoadMap();
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
            return base.OnKeyDown(e);
        }

        private void LoadMap()
        {
            //TODO
//            foreach (var objBox in Map)
//            {
//                Add(objBox);
//            }
        }
    }
}
