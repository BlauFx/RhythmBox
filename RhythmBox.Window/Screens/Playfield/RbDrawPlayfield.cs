using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Window.Objects;
using System;

namespace RhythmBox.Window.Playfield
{
    public class RbDrawPlayfield : Container<ClickBox>
    {
        public Color4 color = Color4.White;

        public Action action;

        public bool EditorMode { get; set; } = false;

        public Bindable<HitObjects.Direction> dir { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        public ClickBox[] NewBox { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Add(drawable(new Vector2(0.89f, 2f), 0.949f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Down
            Add(drawable(new Vector2(0.89f, 2f), 0.051f, 0f, Anchor.TopCentre, Anchor.Centre, Axes.X)); //Up

            Add(drawable(new Vector2(2f, 1f), 0.5f, 0f)); //Left Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.057f)); //Left

            Add(drawable(new Vector2(2f, 1f), 0.5f, 1f)); //Right Outside
            Add(drawable(new Vector2(2f, 0.9f), 0.5f, 0.943f)); //Right

            if (EditorMode)
            {
                Add(drawableMiddle(new Vector2(36f, 1f), 0.5f, 0f, Anchor.TopLeft, Anchor.CentreLeft, Axes.Y, true, false, 0f, HitObjects.Direction.Left)); //Left-Inside
                Add(drawableMiddle(new Vector2(1f, 36f), 0.949f, 0f, Anchor.TopCentre, Anchor.TopCentre, Axes.X, true, false, 0f, HitObjects.Direction.Down)); //Down-Inside
                Add(drawableMiddle(new Vector2(1f, 36f), 0.051f, 0f, Anchor.TopCentre, Anchor.BottomCentre, Axes.X, true, false, 0f, HitObjects.Direction.Up)); //Up-Inside
                Add(drawableMiddle(new Vector2(36f, 1f), 0.5f, 0f, Anchor.TopRight, Anchor.CentreRight, Axes.Y, true, false, 0f, HitObjects.Direction.Right)); //Right-Inside

                NewBox[0] = drawableMiddle(new Vector2(36f, 1f), 0.5f, 0.2f, Anchor.TopLeft, Anchor.CentreLeft, Axes.Y, false, true, 1f, HitObjects.Direction.Left);
                NewBox[1] = drawableMiddle(new Vector2(0.6f, 36f), 0.949f, 0f, Anchor.TopCentre, Anchor.TopCentre, Axes.X, false, true, 1f, HitObjects.Direction.Down);
                NewBox[2] = drawableMiddle(new Vector2(0.6f, 36f), 0.051f, 0f, Anchor.TopCentre, Anchor.BottomCentre, Axes.X, false, true, 1f, HitObjects.Direction.Up);
                NewBox[3] = drawableMiddle(new Vector2(36f, 1f), 0.5f, -0.2f, Anchor.TopRight, Anchor.CentreRight, Axes.Y, false, true, 1f, HitObjects.Direction.Right);
            }
        }

        private ClickBox drawable(Vector2 size, float Y, float X, Anchor anchor = Anchor.TopLeft, Anchor origin = Anchor.Centre, Axes RelativeSizeAxes = Axes.Y,
            bool Editor = false, float Alpha = 1f, HitObjects.Direction direction = HitObjects.Direction.Up) =>
            new ClickBox
            {
                Anchor = anchor,
                Origin = origin,
                RelativeSizeAxes = RelativeSizeAxes,
                Size = size,
                Y = Y,
                X = X,
                RelativePositionAxes = Axes.Both,
                Colour = color,
                Depth = int.MinValue,
                EdgeSmoothness = new Vector2(2f),
                ClickAction = action,
                EditorMode = Editor,
                Alpha = Alpha,
                dir = direction,
                ChangeDir = dir,
                BoxAction = BoxAction,
                BoxAction2 = BoxAction2,
            };

        private ClickBox drawableMiddle(Vector2 size, float Y, float X, Anchor anchor, Anchor origin, Axes relativsize,
            bool Editor = false, bool Editor2 = false, float alpha = 0.5f, HitObjects.Direction direction = HitObjects.Direction.Up) =>
            new ClickBox
            {
                Anchor = anchor,
                Origin = origin,
                RelativeSizeAxes = relativsize,
                Size = size,
                Y = Y,
                X = X,
                RelativePositionAxes = Axes.Both,
                Colour = color,
                Depth = int.MinValue,
                EdgeSmoothness = new Vector2(2f),
                ClickAction = action,
                EditorMode = Editor,
                EditorMode2 = Editor2,
                Alpha = alpha,
                dir = direction,
                ChangeDir = dir,
                BoxAction = BoxAction,
                BoxAction2 = BoxAction2,
            };
    }
}
