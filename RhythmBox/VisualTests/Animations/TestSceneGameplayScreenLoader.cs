using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneGameplayScreenLoader : TestScene
    {
        private GameplayScreenLoader gameplayScreenLoader;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = gameplayScreenLoader = new GameplayScreenLoader
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            AddStep("Start loading", () => gameplayScreenLoader.StartRotating());
            AddWaitStep("Wait", 20);

            AddStep("Stop loading", () => gameplayScreenLoader.StopRotating());
        }
    }
}
