using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Tests.pending_files
{
    public class RbDrawPlayfield : Container<Box>
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Add(drawable(new Vector2(0.89f, 2f), 0.949f, 0f, Anchor.TopCentre, Axes.X)); //Up
            Add(drawable(new Vector2(0.89f, 2f), 0.051f, 0f, Anchor.TopCentre, Axes.X)); //Down

            Add(drawable(new Vector2(2f, 1f), 0.5f, 0f)); //Left Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.057f)); //Left

            Add(drawable(new Vector2(2f, 1f), 0.5f, 1f)); //Right Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.943f)); //Right
        }

        private Box drawable(Vector2 size, float Y, float X, Anchor anchor = Anchor.TopLeft, Axes RelativeSizeAxes = Axes.Y)
        {
            return new Box
            {
                Anchor = anchor,
                Origin = Anchor.Centre,
                RelativeSizeAxes = RelativeSizeAxes,
                Size = size,
                Y = Y,
                X = X,
                RelativePositionAxes = Axes.Both,
                Colour = Color4.Yellow,
                Depth = int.MinValue,
                EdgeSmoothness = new Vector2(2f),
            };
        }
    }
}
