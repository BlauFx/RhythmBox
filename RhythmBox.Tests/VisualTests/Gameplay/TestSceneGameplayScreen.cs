using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using RhythmBox.Window.pending_files;
using System.IO;
using System.Reflection;
using RhythmBox.Window.Screens;
using System.Collections.Generic;
using RhythmBox.Mode.Std.Mods;

namespace RhythmBox.Tests.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneGameplayScreen : TestScene
    {
        private ScreenStack _stack = null;

        private GameplayScreen _gameplayScreen;

        private bool _newScreen = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestSceneGameplayScreen", () =>
            {
                if (!_newScreen) return;
                _newScreen = false;

                Add(_stack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both,
                });

                List<Mod> mods = new List<Mod>();
                //mods.Add(new DummyMod());

                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                
                if (!File.Exists(path))
                    _ = new DefaultFolder();

                LoadComponentAsync(_gameplayScreen = new GameplayScreen(path, mods), _stack.Push);
            });

            AddStep("Fail", () =>
            {
                this._gameplayScreen.HpBar.CurrentValue = 0;
            });

            AddStep("Go back to SongSelection", () =>
            {
                this._gameplayScreen.ReturntoSongSelectionAfterFail.TriggerChange();
            });

            AddStep("Remove TestSceneGameplayScreen", () =>
            {
                _gameplayScreen?.track?.Stop();
                this._stack?.Expire();

                _newScreen = true;
            });
        }
    }
}
