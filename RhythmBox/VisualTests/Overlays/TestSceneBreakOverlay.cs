using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using osuTK.Input;
using RhythmBox.Window.Mods;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window.VisualTests.Overlays
{
   
    [TestFixture]
    public class TestSceneBreakOverlay : ManualInputManagerTestScene
    {
        private BreakOverlay breakOverlay;
        private Box backgroundBox;
        
        private ScreenStack stack;

        [SetUp]
        public void Run()
        {
            breakOverlay = new BreakOverlay(new Action[] {null, null})
            {
                Depth = -1,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
            };

            backgroundBox = new Box
            {
                Depth = 0,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
            };
        }
        
        [Test]
        public void TestBreakOverlay()
        {
            AddStep("Add", () =>
            {
                if (stack != null)
                    Remove(stack);
                Add(backgroundBox);
                Add(breakOverlay);
            });
            
            AddStep("FadeIn", () => breakOverlay.State.Value = Visibility.Visible);
            AddWaitStep("wait for complete", 5);
            AddStep("FadeOut", () => breakOverlay.State.Value = Visibility.Hidden);
        }

        [Test]
        public void TestInGameplay()
        {
            AddStep("Setup gameplay", () =>
            {
                Remove(backgroundBox);
                Remove(breakOverlay);
                //Add(stack);

                Child = stack = new ScreenStack
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                };
                GameplayScreen gameplayScreen;
                LoadComponent(gameplayScreen = new GameplayScreen(DefaultFolder.GetTestMap, new List<Mod>())
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 0.5f,
                });
                stack.Push(gameplayScreen);
            });
            
            AddWaitStep("wait", 4);
            AddStep("Trigger break overlay", () => InputManager.PressKey(Key.Escape));
            AddWaitStep("wait", 20);
            AddStep("Continue", () =>
            {
                InputManager.MoveMouseTo(InputManager.ScreenSpaceDrawQuad.Centre);
                InputManager.Click(MouseButton.Left);
            });
        }
    }
}
