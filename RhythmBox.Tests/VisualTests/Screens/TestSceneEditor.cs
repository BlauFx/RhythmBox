using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window;
using RhythmBox.Window.Screens;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneEditor : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            { typeof(ScreenStack), typeof(EditorDefault) };
        
        private ScreenStack _stack = null;

        private EditorDefault _editorDefault;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestEditorDefault", () =>
            {
                if (_stack?.IsAlive ?? false) return;
                
                Add(_stack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both
                });

                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                
                if (!File.Exists(path))
                    _ = new DefaultFolder();

                LoadComponentAsync(_editorDefault = new EditorDefault(path)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(2f),
                    Alpha = 0f,
                }, _stack.Push);
            });

            AddStep("Remove TestEditorDefault", () =>
            {
                this._editorDefault?.StopTrack();
                this._stack?.Expire();
            });
        }
    }
}
