using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneRbDrawPlayfield : Container
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box //Up
                {
                    Depth = int.MinValue,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.89f,2f),
                    Y = 0.051f,
                    RelativePositionAxes = Axes.Both, 
                    Colour = Color4.Yellow,
                    EdgeSmoothness = new Vector2(2f),
                },
                new Box //Down
                {
                    Depth = int.MinValue,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.89f,2f),
                    Y = 0.949f,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Yellow,
                    EdgeSmoothness = new Vector2(2f),
                },
                new Box //Left Outside
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(2f,1f),
                    Y = 0.5f,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Yellow,
                    Depth = int.MinValue,
                    EdgeSmoothness = new Vector2(2f),
                },
                new Box //Left
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(2f,0.9f),
                    Y = 0.5f,
                    X = 0.057f,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Yellow,
                    Depth = int.MinValue,
                    EdgeSmoothness = new Vector2(2f),
                },
                new Box //Right Outside
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(2f,1f),
                    Y = 0.5f,
                    X = 1f,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Yellow,
                    Depth = int.MinValue,
                    EdgeSmoothness = new Vector2(2f),
                },
                new Box //Right
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(2f,0.9f),
                    Y = 0.5f,
                    X = 0.943f,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Yellow,
                    Depth = int.MinValue,
                    EdgeSmoothness = new Vector2(2f),
                },
            };
        }
    }
}