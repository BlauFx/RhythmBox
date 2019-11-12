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
    public class TestSceneSettings : TestScene
    {
        private ScreenStack stack = null;

        private Settings settingsScreen;

        private bool CreateNewStack = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add Settings", () =>
            {
                if (CreateNewStack)
                {
                    CreateNewStack = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(settingsScreen = new Settings
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(1f),
                        Alpha = 0f,
                    });

                    stack.Push(settingsScreen);
                }
            });

            AddStep("Remove Settings", () =>
            {
                this.stack?.Expire();
                this.settingsScreen?.Exit();
                this.settingsScreen?.Expire();
                this.settingsScreen = null;

                CreateNewStack = true;
            });
        }
    }
}
