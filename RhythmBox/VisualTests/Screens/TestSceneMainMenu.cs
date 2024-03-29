﻿using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneMainMenu : TestScene
    {
        private ScreenStack _stack;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestMainMenu", () =>
            {
                if (_stack?.IsAlive ?? false) 
                    return;
                
                Add(_stack = new ScreenStack { RelativeSizeAxes = Axes.Both });
                LoadComponentAsync(new TestMainMenu
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(0f)
                }, _stack.Push);
            });

            AddStep("Remove TestMainMenu", () => this._stack?.Expire());
        }
    }
    
    public class TestMainMenu : MainMenu { protected override Action GetAction(int pos) => null; }
}
