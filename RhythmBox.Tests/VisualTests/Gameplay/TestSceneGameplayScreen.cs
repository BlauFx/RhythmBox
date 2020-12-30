using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using RhythmBox.Window;
using System.IO;
using RhythmBox.Window.Screens;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;

namespace RhythmBox.Tests.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneGameplayScreen : ManualInputManagerTestScene
    {
        private ScreenStack _stack;

        private GameplayScreen _gameplayScreen;

        [Resolved]
        private Gameini gameini { get; set; }

        string path = $"{Songs.SongPath}{Path.DirectorySeparatorChar}TestMap{Path.DirectorySeparatorChar}Difficulty1.ini";

        Key[] keys = new Key[4];

        private const int Iterations = 1000;

        List<Mod> mods = new List<Mod>();

        [SetUp]
        public void Setup()
        {
            Add(_stack = new ScreenStack
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.8f),
            });

            if (!File.Exists(path))
                _ = new DefaultFolder();

            for (int i = 0; i < 4; i++)
            {
                Enum.TryParse(gameini.Get<string>((SettingsConfig) i), out Key key);
                keys[i] = key;
            }

            AddStep("GameplayScreen", AddGameplayScreen);
            AddStep("AddRemoveGameplayScreen", AddRemoveGameplayScreen);
            AddStep("Fail", Fail);
            AddStep("Remove GameplayScreen", () =>
            {
                _gameplayScreen?.track?.Stop();
                _gameplayScreen?.Expire();
            });
        }

        private void AddGameplayScreen()
        {
            LoadComponent(_gameplayScreen = new GameplayScreen(path, mods)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
            });

            Debug.Assert(_gameplayScreen != null, $"{nameof(_gameplayScreen)} is null!");
            
            InputManager.MoveMouseTo(_gameplayScreen);

            _gameplayScreen.GameStarted.ValueChanged += (e) =>
            {
                if (!e.NewValue) return;
                
                var timings = _gameplayScreen._RbPlayfield.objectList;
                
                Scheduler.UpdateClock(_gameplayScreen._RbPlayfield.Clock);
                
                for (int i = 0; i < timings.Count; i++)
                {
                    Key? key = timings[i].Item1.direction switch
                    {
                        HitObjects.Direction.Up => keys[0],
                        HitObjects.Direction.Left => keys[1],
                        HitObjects.Direction.Down => keys[2],
                        HitObjects.Direction.Right => keys[3],
                        _ => null
                    };
                    
                    Scheduler.AddDelayed(() =>
                    {
                        Debug.Assert(key.HasValue, "key is null");
                        
                        if (InputManager.UseParentInput)
                            return;
                        
                        InputManager.PressKey(key.Value);
                        InputManager.ReleaseKey(key.Value);
                    }, timings[i].Item2 + timings[i].Item1.Duration);
                }
            };

            _stack.Push(_gameplayScreen);
        }

        private async void AddRemoveGameplayScreen()
        {
            for (var i = 0; i < Iterations; i++)
            {
                if (IsDisposed)
                    break;

                await LoadComponentAsync(_gameplayScreen = new GameplayScreen(path, mods)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                }, _stack.Push);

                await Task.Delay(2500);

                _gameplayScreen.Expire();
            }
        }

        private void Fail()
        {
            if (_gameplayScreen is null)
                return;

            this._gameplayScreen.hpbar.CurrentValue.Value = 0;
        }
    }
}
