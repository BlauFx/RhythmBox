using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Mode.Std.Tests.Objects;
using System;
using System.Threading.Tasks;
using Direction = RhythmBox.Mode.Std.Tests.Objects.Direction;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbPlayfield : Container
    {
        public TestSceneBeatmap Beatmap;

        private TestSceneRBox objBox;

        private TestSceneRBox[] objBoxArray = new TestSceneRBox[4];

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box //Up
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.9f,1f),
                    Y = 0.05f,
                    RelativePositionAxes = Axes.Both,
                },
                new Box //Down
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.9f,1f),
                    Y = 0.95f,
                    RelativePositionAxes = Axes.Both,
                },
                new Box //Left
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1.009f,1f),
                    Y = 0.5f,
                    X = 0.05f,
                    Rotation = 90f,
                    RelativePositionAxes = Axes.Both,
                },
                new Box //Right
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1.009f,1f),
                    Y = 0.5f,
                    X = 0.95f,
                    Rotation = 90f,
                    RelativePositionAxes = Axes.Both,
                },
                new Box //Left Outside
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(1f,1f),
                    Y = 0.5f,
                    RelativePositionAxes = Axes.Both,
                },
                new Box //Right Outside
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(1f,1f),
                    Y = 0.5f,
                    X = 1f,
                    RelativePositionAxes = Axes.Both,
                },
                objBoxArray[0] = new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Up,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 430,
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
            LoadBeatmap();
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

        private void LoadBeatmap()
        {
            //TODO
            //foreach (var objBox in Beatmap)
            //{
            //    Add(objBox);
            //}
        }
    }
}
