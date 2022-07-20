using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace RhythmBox.Window.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneGameplayScreenLoader : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            GameplayScreenLoader gameplayScreenLoader;

            Add(gameplayScreenLoader = new GameplayScreenLoader
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
            
            AddStep("Start loading", () => gameplayScreenLoader.StartRotating());
            AddWaitStep("Wait", 10);
            AddStep("Stop loading", () => gameplayScreenLoader.StopRotating());
        }
    }
}
