using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace RhythmBox.Tests.Objects
{
    public class TestSceneSpriteButton : Sprite
    {
        public Action ClickAction;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                Start();
            }
            return base.OnMouseDown(e);
        }

        private void Start()
        {
            ClickAction?.Invoke();
        }
    }
}
