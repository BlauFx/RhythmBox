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

        private SongSelcetion SongSelction;

        private bool newScreen = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestSceneSongSelection", () =>
            {
                if (newScreen)
                {
                    newScreen = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(SongSelction = new SongSelcetion(true));
                    stack.Push(SongSelction);
                }
            });

            AddStep("Remove TestSceneSongSelection", () =>
            {
                this.stack?.Expire();
                this.SongSelction?.Exit();
                this.SongSelction?.Expire();
                this.SongSelction = null;

                newScreen = true;
            });
        }
    }
}
