using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Screens;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneMainMenu : TestScene
    {
        private ScreenStack stack = null;

        private TestMainMenu testMainMenu;

        private bool Can_new_TestSceneMainMenu = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestMainMenu", () =>
            {
                if (Can_new_TestSceneMainMenu)
                {
                    Can_new_TestSceneMainMenu = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(testMainMenu = new TestMainMenu()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0f)
                    });
                    testMainMenu.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);
                    stack.Push(testMainMenu);
                }
            });

            AddStep("Remove TestMainMenu", () =>
            {
                this.stack?.Expire();
                this.testMainMenu?.Exit();
                this.testMainMenu?.Expire();
                this.testMainMenu = null;

                Can_new_TestSceneMainMenu = true;
            });
        }
    }
    public class TestMainMenu : MainMenu
    {
        public TestMainMenu()
        {
            base.Disable_buttons = true;
        }
    }
}
