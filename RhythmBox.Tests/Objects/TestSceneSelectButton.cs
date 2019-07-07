using System;
using System.Drawing;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace RhythmBox.Tests.Objects
{
    public class TestSceneSelectButton : Container
    {
        public string text { get; set; } = string.Empty;

        public float FontSize { get; set; } = 20f;

        public Action ClickAction;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                },
                new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Black,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Text = text,
                    Font = new FontUsage("Roboto",FontSize),
                },
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            ClickAction?.Invoke();

            return base.OnClick(e);
        }
    }
}
