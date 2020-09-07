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
    public class TestSceneSettings : TestScene
    {
        private ScreenStack _stack = null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add Settings", () =>
            {
                if (_stack?.IsAlive ?? false) return;

                Add(_stack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both,
                });

                LoadComponentAsync(new Settings
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f),
                    Alpha = 0f,
                }, _stack.Push);
            });

            AddStep("Remove Settings", () =>
            {
                this._stack?.Expire();
            });
        }
    }
}
