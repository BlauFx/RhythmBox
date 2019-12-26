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
using System;
using osu.Framework.Platform;

namespace RhythmBox.Window.Screens
{
    public class EditorDefault : Screen
    {
        private RhythmBoxCursor _cursor;
        
        private Sprite background;

        private RbPlayfield playfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new BindableBool();

        private BindableDouble UserPlaybackRate = new BindableDouble(1);

        private Map map;

        private BindableBool Resuming = new BindableBool(false);

        private Objects.Progress<float> progress;

        private SpriteText SpriteCurrentTime;

        private ClickBox[] box = new ClickBox[4];

        BindableFloat bindable = new BindableFloat(0);

        private double time = 0f;

        private bool CursorCreated = false;

        private bool HitObjCursorActive = false;

        private bool first_run = false;

        public EditorDefault(string path = null)
        {
            if (path is null)
            {
                throw new NullReferenceException("Path/Map cannot be null");
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
        private void Load(LargeTextureStore largeStore, GameHost host, TextureStore store)
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
                progress = new Objects.Progress<float>(0, map.EndTime, map.StartTime)
                {
                    Depth = 1,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.AliceBlue,
                    Alpha = 1F,
                    Size = new Vector2(0.2f, 0.1f),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
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
                        host.Window.CursorState = CursorState.Hidden;

                        if (CursorCreated)
                        {
                            RemoveInternal(_cursor);
                        }
                        
                        AddInternal(_cursor = new RhythmBoxCursor(@"Game/HitObjCursor")
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(1f),
                        });

                        CursorCreated = true;
                        HitObjCursorActive = true;
                    },
                },
                new SpriteTextButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1f,
                    Size = new Vector2(0.05f),
                    RelativePositionAxes = Axes.Both,
                    X = -0.45f,
                    Y = -0.2f,
                    Text = "DefaultCursor",
                    ShadowColour = Color4.Black,
                    Spacing = new Vector2(0.1f),
                    Font = new FontUsage("Roboto", 30),
                    AllowMultiline = false,
                    ClickAction = () =>
                    {
                        host.Window.CursorState = CursorState.Hidden;

                        if (CursorCreated)
                        {
                            RemoveInternal(_cursor);
                        }
                        
                        AddInternal(_cursor = new RhythmBoxCursor(@"Game/DefaultCursor")
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(1f),
                        });

                        CursorCreated = true;
                        HitObjCursorActive = false;
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
                        if (HitObjCursorActive)
                        {
                            rhythmBoxClockContainer.Stop();

                            time = rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;

                            playfield.StopScheduler();
                            playfield.RemoveRange(playfield.objBoxArray);

                            rhythmBoxClockContainer.Seek(time);
                            playfield.LoadMapForEditor2(time, playfield.dir.Value, 1f);
                        }
                    },
                    BoxAction = () =>
                    {
                        if (HitObjCursorActive)
                        {
                            int i = 0;
                            switch (playfield.dir.Value)
                            {
                                case Mode.Std.Interfaces.HitObjects.Direction.Left:
                                    i = 0;
                                    break;
                                case Mode.Std.Interfaces.HitObjects.Direction.Down:
                                    i = 1;
                                    break;
                                case Mode.Std.Interfaces.HitObjects.Direction.Up:
                                    i = 2;
                                    break;
                                case Mode.Std.Interfaces.HitObjects.Direction.Right:
                                    i = 3;
                                    break;
                            }

                            RemoveInternal(box[i]);
                            AddInternal(box[i]);
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

            rhythmBoxClockContainer.Stop();
        }

        protected override void LoadComplete()
        {
            playfield.CanStart.ValueChanged += CanStart_ValueChanged;
            progress.BoxWidth.ValueChanged += BoxWidth_ValueChanged;
            bindable.ValueChanged += Bindable_ValueChanged;

            base.LoadComplete();
        }

        private void Bindable_ValueChanged(ValueChangedEvent<float> obj)
        {
            if (!this.IsPaused.Value)
            {
                if (!progress.CurrentlyDragging)
                {
                    var x = rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;
                    var y = map.EndTime;

                    var z = (x / y) * 1;

                    progress.box.Width = (float)z;
                }
                else
                {
                    Scheduler.CancelDelayedTasks();
                }
            }
        }

        private void CanStart_ValueChanged(ValueChangedEvent<bool> obj)
        {
            Scheduler.CancelDelayedTasks();

            if (!this.IsPaused.Value)
            {
                rhythmBoxClockContainer.Start();
            }

            // playfield.CanStart.ValueChanged -= CanStart_ValueChanged;
        }

        protected override void Update()
        {
            base.Update();

            if (rhythmBoxClockContainer.RhythmBoxClock.IsRunning)
            {
                if (rhythmBoxClockContainer.RhythmBoxClock.CurrentTime <= map.EndTime)
                {
                    TimeSpan result = TimeSpan.FromMilliseconds(rhythmBoxClockContainer.RhythmBoxClock.CurrentTime);
                    string time = result.ToString(@"mm\:ss");
                    time += $":{result.Milliseconds}";
                    SpriteCurrentTime.Text = time;
                    bindable.Value = (float)rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;
                }
                else
                {
                    rhythmBoxClockContainer.Stop();
                }
            }
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
            else if (e.Key == osuTK.Input.Key.Escape)
            {
                this.ClearTransforms();
                rhythmBoxClockContainer?.Stop();
                this.Exit();
            }
            return base.OnKeyDown(e);
        }

        private void BoxWidth_ValueChanged(ValueChangedEvent<float> obj)
        {
            if (!first_run)
            {
                first_run = true;
                return;
            }

            bool IsPaused = this.IsPaused.Value;

            double calcPos = map.EndTime * progress.box.Width;

            playfield.StopScheduler();

            playfield.RemoveRange(playfield.objBoxArray);

            rhythmBoxClockContainer.Stop();
            rhythmBoxClockContainer.Seek(calcPos);

            rhythmBoxClockContainer.Start();
            rhythmBoxClockContainer.Stop();

            playfield.LoadMapForEditor(calcPos);

            TimeSpan result = TimeSpan.FromMilliseconds(calcPos);
            string time = result.ToString(@"mm\:ss");
            time += $":{result.Milliseconds}";
            SpriteCurrentTime.Text = time;
            bindable.Value = (float)rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;

            if (!IsPaused)
            {
                rhythmBoxClockContainer.Start();
            }
        }

        public override void OnEntering(IScreen last)
        {
            this.Anchor = Anchor.Centre;
            this.Origin = Anchor.Centre;
            this.Scale = new Vector2(1.5f);

            this.FadeInFromZero<EditorDefault>(1500, Easing.OutExpo);
            this.TransformTo(nameof(Scale), new Vector2(1f), 1500, Easing.InOutCirc).OnComplete((e) => rhythmBoxClockContainer.Start());

            Discord.DiscordRichPresence.UpdateRPC(
             new DiscordRPC.RichPresence()
             {
                 Details = "Editing a map",
                 State = " ",
                 Assets = new DiscordRPC.Assets()
                 {
                     LargeImageKey = "three",
                 }
             });

            base.OnEntering(last);
        }
    }
}
