using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
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

        private RbPlayfield playfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new BindableBool();

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        private Map map;

        private BindableBool Resuming = new BindableBool(false);

        private Progress<float> progress;

        private SpriteText SpriteCurrentTime;

        private bool AddNewHitObj = false;

        private double time = 0;

        private ClickBox[] box = new ClickBox[4];

        private float LastCalcPos = 0f;

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
                    Size = new Vector2(1f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.044f),
                    RelativePositionAxes = Axes.Both,
                    X = 0.35f,
                    Y = 0f,
                    Text = "10%",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                    ClickAction = () =>
                    {
                        UserPlaybackRate.Value = 0.1f;
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
                },
                progress = new Progress<float>(0, map.EndTime, map.StartTime)
                {
                    Depth = 1,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.AliceBlue,
                    Alpha = 1F,
                    Size = new Vector2(0.2f, 0.1f),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Action = () =>
                    {
                        float calcPos = map.EndTime * progress.bindableValue.Value;
                        if (LastCalcPos > calcPos)
                        {
                            playfield.StopScheduler();
                            playfield.RemoveRange(playfield.objBoxArray);

                            rhythmBoxClockContainer.Stop();
                            rhythmBoxClockContainer.Seek(calcPos);
                            playfield.LoadMapForEditor(calcPos);
                            rhythmBoxClockContainer.Start();
                        }
                        else
                        {
                            rhythmBoxClockContainer.Seek(calcPos);
                        }

                        LastCalcPos = calcPos;

                        SpriteCurrentTime.Text = string.Empty;
                        SpriteCurrentTime.Text = $"{calcPos}";
                    },
                },
                SpriteCurrentTime = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.044f),
                    RelativePositionAxes = Axes.Both,
                    X = -0.45f,
                    Y = -0.35f,
                    Text = "0",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                },
                new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.044f),
                    RelativePositionAxes = Axes.Both,
                    X = -0.45f,
                    Y = -0.1f,
                    Text = "HitObj",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                    ClickAction = () =>
                    {
                        //TODO:
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
                playfield = new RbPlayfield(null)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = map,
                    EditorMode = true,
                    NewBox = box,
                    action = () =>
                    {
                        rhythmBoxClockContainer.Stop();

                        var x = rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;

                        playfield.StopScheduler();
                        playfield.RemoveRange(playfield.objBoxArray);

                        SpriteCurrentTime.Text = string.Empty;
                        SpriteCurrentTime.Text = $"{x}";

                        time = x;
                        AddNewHitObj = true;

                        rhythmBoxClockContainer.Seek(time);
                        playfield.LoadMapForEditor2(time, playfield.dir.Value);
                        rhythmBoxClockContainer.Start();

                        rhythmBoxClockContainer.Stop();
                    },
                    BoxAction = () =>
                    {
                        switch (playfield.dir.Value)
                        {
                            case Mode.Std.Interfaces.HitObjects.Direction.Left:
                                RemoveInternal(box[0]);
                                AddInternal(box[0]);
                                break;
                            case Mode.Std.Interfaces.HitObjects.Direction.Down:
                                RemoveInternal(box[1]);
                                AddInternal(box[1]);
                                break;
                            case Mode.Std.Interfaces.HitObjects.Direction.Up:
                                RemoveInternal(box[2]);
                                AddInternal(box[2]);
                                break;
                            case Mode.Std.Interfaces.HitObjects.Direction.Right:
                                RemoveInternal(box[3]);
                                AddInternal(box[3]);
                                break;
                        }
                    },
                    BoxAction2 = () =>
                    {
                        for (int i = 0; i < box.Length; i++)
                        {
                             RemoveInternal(box[i]);
                        }
                    },
                },
            };

            rhythmBoxClockContainer.IsPaused.BindTo(IsPaused);
            rhythmBoxClockContainer.UserPlaybackRate.BindTo(UserPlaybackRate);

            playfield.Clock = rhythmBoxClockContainer.RhythmBoxClock;

            playfield.Resuming.BindTo(Resuming);
        }

        protected override void LoadComplete()
        {

            playfield.CanStart.ValueChanged += CanStart_ValueChanged;

            this.TransformTo(nameof(Alpha), 1f, 1500, Easing.OutExpo);
            this.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.InExpo).OnComplete((e) => rhythmBoxClockContainer.Start());

            base.LoadComplete();
        }

        private void CanStart_ValueChanged(ValueChangedEvent<bool> obj)
        {
            progress.AllowAction = false;

            if (obj.NewValue)
            {
                progress.AllowAction = true;
            }
            
            if (!this.IsPaused.Value)
            {
                rhythmBoxClockContainer.Start();
            }

            Scheduler.AddDelayed(() =>
            {
                if (progress.AllowAction)
                {
                    if (!this.IsPaused.Value)
                    {
                        //progress.Current.Value += 0.01f;

                        float calcPos = map.EndTime * progress.bindableValue.Value;

                        SpriteCurrentTime.Text = string.Empty;
                        SpriteCurrentTime.Text = $"{calcPos}";
                    }
                }

            }, 10, true);

            // playfield.CanStart.ValueChanged -= CanStart_ValueChanged;
        }

        protected override void Update()
        {
            //Logger.Log(rhythmBoxClockContainer.RhythmBoxClock.CurrentTime.ToString());
            base.Update();
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
                    if (AddNewHitObj)
                    {
                        AddNewHitObj = false;
                        //rhythmBoxClockContainer.Seek(time);
                        //playfield.LoadMapForEditor2(time, playfield.dir.Value);
                    }
                    rhythmBoxClockContainer.Start();
                }
            }
            return base.OnKeyDown(e);
        }
    }
}
