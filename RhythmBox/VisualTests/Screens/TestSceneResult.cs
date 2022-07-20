using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Screens.Result;

namespace RhythmBox.Window.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneResult : TestScene
    {
        private ScreenStack _stack = null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add ResultScreen", () =>
            {
                if (_stack?.IsAlive ?? false) return;

                Add(_stack = new ScreenStack{ RelativeSizeAxes = Axes.Both });

                LoadComponentAsync(new ResultScreen
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f),
                    Alpha = 1f,
                }, _stack.Push);
            });

            AddStep("Remove ResultScreen", () => this._stack?.Expire());
        }
    }
}