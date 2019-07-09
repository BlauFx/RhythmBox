using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Mode.Std.Tests.Objects;
using Direction = RhythmBox.Mode.Std.Tests.Objects.Direction;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbPlayfield : Container
    {
        public TestSceneBeatmap Beatmap;

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
                new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Left,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 430,
                },
                new TestSceneRBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = Direction.Up,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = 700,
                    speed = 1f,
                },
            };

            LoadBeatmap();
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
