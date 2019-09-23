using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Tests.Clock;
using RhythmBox.Tests.pending_files;
using System.IO;
using System.Reflection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneEditor : TestScene
    {
        private ScreenStack stack = null;

        private TestEditorDefault testEditorDefault;

        private bool Can_new_TestSceneEditorDefault = true;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add TestEditorDefault", () =>
            {
                if (Can_new_TestSceneEditorDefault)
                {
                    Can_new_TestSceneEditorDefault = false;

                    Add(stack = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    });

                    LoadComponent(testEditorDefault = new TestEditorDefault()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(2f),
                        Alpha = 0f,
                    });

                    stack.Push(testEditorDefault);
                }
            });

            AddStep("Remove TestEditorDefault", () =>
            {
                this.stack?.Expire();
                this.testEditorDefault?.Exit();
                this.testEditorDefault?.Expire();
                this.testEditorDefault = null;

                Can_new_TestSceneEditorDefault = true;
            });
        }
    }
    public class TestEditorDefault : Screen
    {
        private Sprite background;

        private TestSceneRbPlayfield _testSceneRbPlayfield;

        private TestSceneRhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new BindableBool();

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        private TestSceneMap map;

        private BindableBool Resuming = new BindableBool(false);

        public TestEditorDefault()
        {
            string path = "null";
            if (path == "null")
            {
                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                if (!File.Exists(path))
                {
                    new TestSceneDefaultFolder();
                }
            }

            var testSceneMapReader = new TestSceneMapReader(path);
            map = new TestSceneMap
            {
                AFileName = testSceneMapReader.AFileName,
                BGFile = testSceneMapReader.BGFile,
                MapId = testSceneMapReader.MapId,
                MapSetId = testSceneMapReader.MapSetId,
                BPM = testSceneMapReader.BPM,
                Objects = testSceneMapReader.Objects,
                AutoMap = testSceneMapReader.AutoMap,
                Mode = testSceneMapReader.Mode,
                Title = testSceneMapReader.Title,
                Artist = testSceneMapReader.Artist,
                Creator = testSceneMapReader.Creator,
                DifficultyName = testSceneMapReader.DifficultyName,
                StartTime = testSceneMapReader.StartTime,
                EndTime = testSceneMapReader.EndTime,
                HitObjects = testSceneMapReader.HitObjects,
                Path = testSceneMapReader.Path,
            };
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                background = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 0.7f,
                    Texture = store.Get("Skin/menu-background"),
                },
                rhythmBoxClockContainer = new TestSceneRhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f)
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                _testSceneRbPlayfield = new TestSceneRbPlayfield(null)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = map,
                },
            };

            rhythmBoxClockContainer.IsPaused.BindTo(IsPaused);
            rhythmBoxClockContainer.UserPlaybackRate.BindTo(UserPlaybackRate);

            _testSceneRbPlayfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;

            _testSceneRbPlayfield.Resuming.BindTo(Resuming);
        }

        protected override void LoadComplete()
        {
            this.TransformTo(nameof(Alpha), 1f, 1500, Easing.OutExpo);
            this.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.InExpo).OnComplete((e) => rhythmBoxClockContainer.Start());

            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Space)
            {
                IsPaused.Value = IsPaused.Value ? false : true;

                if (IsPaused.Value)
                {
                    rhythmBoxClockContainer.Stop();
                }
                else
                {
                    rhythmBoxClockContainer.Start();
                }
            }
            return base.OnKeyDown(e);
        }
    }
}
