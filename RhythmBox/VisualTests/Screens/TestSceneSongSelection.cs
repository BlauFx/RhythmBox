using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using RhythmBox.Window.Screens.SongSelection;

namespace RhythmBox.Window.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelection : TestScene
    {
        private ScreenStack _stack = null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestSceneSongSelection", () =>
            {
                if (_stack?.IsAlive ?? false) return;
                
                Add(_stack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both,
                });

                LoadComponentAsync(new Selection(true), _stack.Push);
            });

            AddStep("Remove TestSceneSongSelection", () => _stack?.Expire());
        }
    }
}
