using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Input;
using System;

namespace RhythmBox.Window.Objects
{
    public class ClickBox : Box
    {
        public Action ClickAction;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                ClickAction?.Invoke();
                ClickAction = null;
            }
            return base.OnMouseDown(e);
        }
    }
}
