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
        private ScreenStack stack = null;

        private GameplayScreen gameplayScreen;

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

                    List<Mod> mods = new List<Mod>();
                    //mods.Add(new DummyMod());

                    string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                    if (!File.Exists(path))
                    {
                        new DefaultFolder();
                    }

                    LoadComponent(gameplayScreen = new GameplayScreen(path, mods));
                    stack.Push(gameplayScreen);
                }
            });

            AddStep("Fail", () =>
            {
                this.gameplayScreen._hpBar.CurrentValue = 0;
            });

            AddStep("Go back to SongSelection", () =>
            {
                this.gameplayScreen.ReturntoSongSelectionAfterFail.TriggerChange();
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
