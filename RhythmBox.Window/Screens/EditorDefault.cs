using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Objects;
using RhythmBox.Window.pending_files;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Screens
{
    public class EditorDefault : Screen
    {
        private Sprite background;

        private RbPlayfield _testSceneRbPlayfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new BindableBool();

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        private Map map;

        private BindableBool Resuming = new BindableBool(false);

        public EditorDefault(/*CurrentMap*/)
        {
            string path = "null";
            if (path == "null")
            {
                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs\\TestMap\\Difficulty1.ini";
                if (!File.Exists(path))
                {
                    new DefaultFolder();
                }
            }

            var mapReader = new MapReader(path);
            map = new Map
            {
                AFileName = mapReader.AFileName,
                BGFile = mapReader.BGFile,
                MapId = mapReader.MapId,
                MapSetId = mapReader.MapSetId,
                BPM = mapReader.BPM,
                Objects = mapReader.Objects,
                AutoMap = mapReader.AutoMap,
                Mode = mapReader.Mode,
                Title = mapReader.Title,
                Artist = mapReader.Artist,
                Creator = mapReader.Creator,
                DifficultyName = mapReader.DifficultyName,
                StartTime = mapReader.StartTime,
                EndTime = mapReader.EndTime,
                HitObjects = mapReader.HitObjects,
                Path = mapReader.Path,
            };
        }

        [BackgroundDependencyLoader]
        private void Load(LargeTextureStore largeStore)
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
                },
                rhythmBoxClockContainer = new RhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f)
                },
                new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.044f),
                    RelativePositionAxes = Axes.Both,
                    X = 0.4f,
                    Y = 0f,
                    Text = "50%",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                    ClickAction = () =>
                    {
                        UserPlaybackRate.Value = 0.5f;
                    },
                },
                new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.044f),
                    RelativePositionAxes = Axes.Both,
                    X = 0.45f,
                    Y = 0f,
                    Text = "100%",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                    ClickAction = () =>
                    {
                        UserPlaybackRate.Value = 1f;
                    },
                }
            };

            Schedule(async () =>
            {
                Texture x = await largeStore.GetAsync("Skin/menu-background");
                background.Texture = x;
            });

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                _testSceneRbPlayfield = new RbPlayfield(null)
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
