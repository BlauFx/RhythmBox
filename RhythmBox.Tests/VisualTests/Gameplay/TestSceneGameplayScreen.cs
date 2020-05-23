using System;
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
using System.Diagnostics;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;

namespace RhythmBox.Tests.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneGameplayScreen : ManualInputManagerTestScene
    {
        private ScreenStack _stack = null;

        private GameplayScreen _gameplayScreen;

        [Resolved]
        private Gameini gameini { get; set; }

        [Test]
        public void Add() => AddStep("Gameplay", () =>
        {
            Add(_stack = new ScreenStack
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.8f),
            });

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\TestMap\\Difficulty1.ini";

            if (!File.Exists(path))
                _ = new DefaultFolder();

            Map map = new Map(path);
            
            List<Tuple<double, HitObjects.Direction>> timings = new List<Tuple<double, HitObjects.Direction>>();

            foreach (var hitobj in map.HitObjects)
            {
                timings.Add(new Tuple<double, HitObjects.Direction>(hitobj.Time, hitobj._direction));
            }

            List<Mod> mods = new List<Mod>();
            //mods.Add(new DummyMod3());

            LoadComponent(_gameplayScreen = new GameplayScreen(path, mods)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
            });

            if (_gameplayScreen != null)
                InputManager.MoveMouseTo(_gameplayScreen);

            _gameplayScreen.GameStarted.ValueChanged += (e) =>
            {
                if (e.NewValue)
                {
                    Key[] keys = new Key[4];

                    for (int i = 0; i < 4; i++)
                    {
                        Enum.TryParse(gameini.Get<string>((SettingsConfig) i), out Key key);
                        keys[i] = key;
                    }

                    for (int i = 0; i < timings.Count; i++)
                    {
                        Key? key = null;

                        if (timings[i].Item2 == HitObjects.Direction.Up)
                            key = keys[0];
                        else if (timings[i].Item2 == HitObjects.Direction.Left)
                            key = keys[1];
                        else if (timings[i].Item2 == HitObjects.Direction.Down)
                            key = keys[2];
                        else if (timings[i].Item2 == HitObjects.Direction.Right)
                            key = keys[3];

                        Scheduler.AddDelayed(() =>
                        {
                            Debug.Assert(key.HasValue, "key is null");

                            if (InputManager.UseParentInput)
                                return;

                            InputManager.PressKey(key.Value);
                            InputManager.ReleaseKey(key.Value);
                        }, timings[i].Item1);
                    }
                }
            };

            _stack.Push(_gameplayScreen);
        });

        [Test]
        public void Fail()
        {
            if (_gameplayScreen is null) return;
            
            this._gameplayScreen.HpBar.CurrentValue = 0;
        }

        [Test]
        public void Remove() => AddStep("Remove Gameplay", () =>
        {
            _gameplayScreen?.track?.Stop();
            this._stack?.Expire();
        });
    }
}
