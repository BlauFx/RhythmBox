using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace RhythmBox.Window.Objects
{
    public class SpriteTextButton : SpriteText
    {
        public Action ClickAction;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                ClickAction?.Invoke();
            }
            return base.OnMouseDown(e);
        }
    }
}
