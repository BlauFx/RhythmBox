using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using RhythmBox.Window.Screens;


namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelection : TestScene
    {
        private ScreenStack stack = null;

        private SongSelction gameplayScreen;

        private bool newScreen = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestSceneGameplayScreen", () =>
            {
                if (newScreen)
                {
                    newScreen = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(gameplayScreen = new SongSelction());
                    stack.Push(gameplayScreen);
                }
            });

            AddStep("Remove TestSceneGameplayScreen", () =>
            {
                this.stack?.Expire();
                this.gameplayScreen?.Exit();
                this.gameplayScreen?.Expire();
                this.gameplayScreen = null;

                newScreen = true;
            });
        }
    }
}
