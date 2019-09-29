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
    public class TestSceneEditor : TestScene
    {
        private ScreenStack stack = null;

        private EditorDefault testEditorDefault;

        private bool Can_new_TestSceneEditorDefault = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestEditorDefault", () =>
            {
                if (Can_new_TestSceneEditorDefault)
                {
                    Can_new_TestSceneEditorDefault = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(testEditorDefault = new EditorDefault()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(2f),
                        Alpha = 0f,
                    });

                    stack.Push(testEditorDefault);
                }
            });

            AddStep("Remove TestEditorDefault", () =>
            {
                this.stack?.Expire();
                this.testEditorDefault?.Exit();
                this.testEditorDefault?.Expire();
                this.testEditorDefault = null;

                Can_new_TestSceneEditorDefault = true;
            });
        }
    }
}
