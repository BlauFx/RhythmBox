using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osu.Framework.Screens;
using RhythmBox.Window.Screens.SongSelection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSearchContainer : TestScene
    {
        private HandleSearch hsearch;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add HandleSearch", () =>
            {
                if (hsearch?.IsAlive ?? false)
                    return;

                Add(hsearch = new HandleSearch
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f),
                });
            });

            AddStep("Remove HandleSearch", this.Clear);
        }
    }
}
