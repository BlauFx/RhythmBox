using System;
using System.Collections.Generic;
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
    public class TestSceneMainMenu : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            { typeof(ScreenStack), typeof(TestMainMenu) };
        
        private ScreenStack _stack = null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestMainMenu", () =>
            {
                if (_stack?.IsAlive ?? false) return;
                
                Add(_stack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both
                });

                LoadComponentAsync(new TestMainMenu
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(0f)
                }, _stack.Push);
            });

            AddStep("Remove TestMainMenu", () =>
            {
                this._stack?.Expire();
            });
        }
    }
    
    public class TestMainMenu : MainMenu { public override Action GetAction(int pos) => null; }
}
