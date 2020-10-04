using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Screens.SongSelection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSearchContainer : TestScene
    {
        private HandleSearch hsearch;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add HandleSearch", () =>
            {
                if (hsearch != null && hsearch.IsAlive)
                    return;

                Add(new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f),
                    Colour = Color4.Red.Opacity(0.8f),
                });
                
                Add(hsearch = new HandleSearch
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f),
                });
            });

            AddStep("Remove HandleSearch", this.Clear);
        }
    }
}
