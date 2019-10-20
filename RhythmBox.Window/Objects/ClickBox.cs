using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Input;
using RhythmBox.Mode.Std.Interfaces;
using System;

namespace RhythmBox.Window.Objects
{
    public class ClickBox : Box
    {
        public Action ClickAction;

        public bool EditorMode { get; set; } = false;

        public bool EditorMode2 { get; set; } = false;

        public HitObjects.Direction dir { get; set; }

        public Bindable<HitObjects.Direction> ChangeDir { get; set; }

        public Action BoxAction { get; set; }

        public Action BoxAction2 { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            if (EditorMode)
            {
                this.AlwaysPresent = true;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                if (EditorMode2)
                {
                    ClickAction?.Invoke();
                }
            }
            return base.OnMouseDown(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (EditorMode)
            {
                ChangeDir.Value = dir;
                BoxAction?.Invoke();
            }

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (EditorMode)
            {
                BoxAction2?.Invoke();
            }

            base.OnHoverLost(e);
        }
    }
}
