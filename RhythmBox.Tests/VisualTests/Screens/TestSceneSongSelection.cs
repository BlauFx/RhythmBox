using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using RhythmBox.Window.Screens;
using RhythmBox.Window.Screens.SongSelection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelection : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            { typeof(ScreenStack), typeof(SongSelcetion) };
        
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

                LoadComponentAsync(new SongSelcetion(true), _stack.Push);
            });

            AddStep("Remove TestSceneSongSelection", () => _stack?.Expire());
        }
    }
}
