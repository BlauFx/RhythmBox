using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Clocks;
using RhythmBox.Window.Objects;
using System;
using System.IO;
using System.Threading.Tasks;
using RhythmBox.Window.Mode.Standard.Maps;

namespace RhythmBox.Window.Screens
{
    public class EditorDefault : Screen
    {
        private RhythmBoxCursor _cursor;

        private Sprite background;

        private Playfield.Playfield playfield;

        private RhythmBoxClockContainer rhythmBoxClockContainer;

        private BindableBool IsPaused = new();

        public BindableDouble UserPlaybackRate = new(1)
            {Default = 1, MinValue = 0.1, MaxValue = 3, Precision = 0.1};

        private Map map { get; }

        private BindableBool resuming = new();

        private Objects.Progress<float> progress;

        private SpriteText SpriteCurrentTime;

        private ClickBox[] box = new ClickBox[4];

        BindableFloat bindable { get; set; } = new(0);

        private double time = 0f;

        private bool CursorCreated;

        private bool HitObjCursorActive;

        private bool first_run;

        [Resolved] private AudioManager audio { get; set; }

        [Resolved] private GameHost GameHost { get; set; }

        private ITrackStore trackStore;

        private IResourceStore<byte[]> store;

        private Track track;

        public EditorDefault(string path = null)
        {
            if (path is null)
                throw new NullReferenceException("Path cannot be null!");

            map = new Map(path);
        }

        [BackgroundDependencyLoader]
        private void Load(LargeTextureStore largeStore, GameHost host)
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
                    Texture = largeStore.Get("Skin/menu-background"),
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
                    ClickAction = () => { UserPlaybackRate.Value = 0.1f; },
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
                    ClickAction = () => { UserPlaybackRate.Value = 0.5f; },
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
                    ClickAction = () => { UserPlaybackRate.Value = 1f; },
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

            rhythmBoxClockContainer.Children = new Drawable[]
            {
                playfield = new Playfield.Playfield(null)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = map,
                    //NewBox = box,
                    action = () =>
                    {
                        if (HitObjCursorActive)
                        {
                            double time = rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;
                            rhythmBoxClockContainer.Stop();

                            //playfield.AddHitObj(time, playfield.dir.Value, 1f);

                            rhythmBoxClockContainer.Start();
                        }
                    },
                    BoxAction = () =>
                    {
                        if (HitObjCursorActive)
                        {
                            int i = playfield.dir.Value switch
                            {
                                HitObject.Direction.Left => 0,
                                HitObject.Direction.Down => 1,
                                HitObject.Direction.Up => 2,
                                HitObject.Direction.Right => 3,
                                _ => 4,
                            };

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
            playfield.Resuming.BindTo(resuming);

            rhythmBoxClockContainer.Stop();

            this.store = new StorageBackedResourceStore(GameHost.Storage);
            trackStore = audio.GetTrackStore(this.store);

            int num = map.Path.LastIndexOf(Path.DirectorySeparatorChar);
            string tmp = map.Path[..num];

            string audioFile = $"{tmp}{Path.DirectorySeparatorChar}{map.AFileName}";
            track = trackStore.Get(audioFile);

            track.Start();
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

                    progress.box.Width = (float) z;
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
                track?.Start();
            }

            playfield.CanStart.ValueChanged -= CanStart_ValueChanged;
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
                    bindable.Value = (float) rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;
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
                    track?.Stop();
                }
                else
                {
                    rhythmBoxClockContainer.Start();
                    track?.Start();
                }
            }
            else if (e.Key == osuTK.Input.Key.Escape)
            {
                this.ClearTransforms();
                rhythmBoxClockContainer?.Stop();
                track?.Stop();
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

            rhythmBoxClockContainer.Stop();
            rhythmBoxClockContainer.Seek(calcPos);
            rhythmBoxClockContainer.Start();
            rhythmBoxClockContainer.Stop();

            TimeSpan result = TimeSpan.FromMilliseconds(calcPos);
            string time = result.ToString(@"mm\:ss");
            time += $":{result.Milliseconds}";
            SpriteCurrentTime.Text = time;
            bindable.Value = (float) rhythmBoxClockContainer.RhythmBoxClock.CurrentTime;

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

            this.FadeInFromZero(1500, Easing.OutExpo);
            this.TransformTo(nameof(Scale), new Vector2(1f), 1500, Easing.InOutCirc).OnComplete((e) =>
            {
                rhythmBoxClockContainer.Start();
                track?.Start();
            });

            base.OnEntering(last);
        }

        public void StopTrack() => track?.Stop();

        public override bool OnExiting(IScreen next)
        {
            StopTrack();

            return base.OnExiting(next);
        }

        public override void OnSuspending(IScreen next) => StopTrack();
    }
}
