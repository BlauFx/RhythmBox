using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Screens.Playfield
{
    public class DrawPlayfield : Container<ClickBox>
    {
        public Color4 BorderColor = Color4.White;

        public Action action;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Add(drawable(new Vector2(1f, 2f), 0f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Up Outside
            Add(drawable(new Vector2(0.89f, 2f), 0.051f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Up
            
            Add(drawable(new Vector2(1f, 2f), 1f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Down Outside
            Add(drawable(new Vector2(0.89f, 2f), 0.949f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Down

            Add(drawable(new Vector2(2f, 1f), 0.5f, 0f)); //Left Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.057f)); //Left

            Add(drawable(new Vector2(2f, 1f), 0.5f, 1f)); //Right Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.943f)); //Right
        }

        private ClickBox drawable(Vector2 size, float Y, float X, Anchor anchor = Anchor.TopLeft, Anchor origin = Anchor.Centre, Axes relativeSizeAxes = Axes.Y,
            float alpha = 1f) =>
            new()
            {
                Anchor = anchor,
                Origin = origin,
                RelativeSizeAxes = relativeSizeAxes,
                Size = size,
                Y = Y,
                X = X,
                RelativePositionAxes = Axes.Both,
                Colour = BorderColor,
                Depth = int.MinValue,
                EdgeSmoothness = new Vector2(2f),
                ClickAction = action,
                Alpha = alpha,
            };
    }
}
