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
        private ScreenStack stack = null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add Settings", () =>
            {
                if (stack?.IsAlive ?? false) return;

                Add(stack = new ScreenStack
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
                }, stack.Push);
            });

            AddStep("Remove Settings", () =>
            {
                this.stack?.Expire();
            });
        }
    }
}
