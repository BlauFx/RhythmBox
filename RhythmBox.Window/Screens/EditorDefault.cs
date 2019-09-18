﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.pending_files;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.Screens
{
    class EditorDefault : Screen
    {
        private Sprite background;

        private RbPlayfield _testSceneRbPlayfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new BindableBool();

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        private Map map;

        private BindableBool Resuming = new BindableBool(false);

        public EditorDefault()
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

            var testSceneMapReader = new MapReader(path);
            map = new Map
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
                rhythmBoxClockContainer = new RhythmBoxClockContainer(0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f)
                }
            };

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                _testSceneRbPlayfield = new RbPlayfield
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
