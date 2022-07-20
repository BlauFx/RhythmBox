using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using osuTK.Input;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Mods;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneGameplayScreen : ManualInputManagerTestScene
    {
        private ScreenStack _stack;

        private GameplayScreen _gameplayScreen;

        [Resolved]
        private Gameini gameini { get; set; }

        private string path = DefaultFolder.GetTestMap;

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
                _gameplayScreen?.Track?.Stop();
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
                
                var timings = _gameplayScreen.RbPlayfield.objectList;
                
                Scheduler.UpdateClock(_gameplayScreen.RbPlayfield.Clock);
                
                for (int i = 0; i < timings.Count; i++)
                {
                    Key? key = timings[i].Item1.Direction switch
                    {
                        HitObject.DirectionEnum.Up => keys[0],
                        HitObject.DirectionEnum.Left => keys[1],
                        HitObject.DirectionEnum.Down => keys[2],
                        HitObject.DirectionEnum.Right => keys[3],
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

            this._gameplayScreen.Hpbar.CurrentValue.Value = 0;
        }
    }
}
