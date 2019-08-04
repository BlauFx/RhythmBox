using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    public class TestSceneBreakOverlay : TestScene
    {
        private TextFlowContainer text;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                text = new TextFlowContainer
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextAnchor = Anchor.Centre,
                    Spacing = new Vector2(2f),
                    AutoSizeAxes = Axes.Both,
                }
            };
            text.AddText("Hello", x => x.Font = new FontUsage("Roboto", 100));
        }
    }
}
