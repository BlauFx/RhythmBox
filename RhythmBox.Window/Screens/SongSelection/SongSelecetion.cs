﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Objects;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens.SongSelection
{
    public class SongSelcetion : Screen
    {
        Bindable<string> bindablePath = new Bindable<string>();

        private ThisScrollContainer scrollContainer;

        private BasicTextBox textBox;

        private bool WaitUntilLoaded = true;

        private ModOverlay ModOverlay;

        private bool AllowToPlay = true;

        [Resolved]
        private CurrentMap currentMap { get; set; }

        public SongSelcetion(bool test = false)
        {
            if (test)
                AllowToPlay = false;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            SpriteButton Back;

            InternalChildren = new Drawable[]
            {
                ModOverlay = new ModOverlay
                {
                    Depth = -1,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
                new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Texture = store.Get("Skin/menu-background"), //TOOD: Maybe Map => Background?
                },
                Back = new SpriteButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativePositionAxes = Axes.Both,
                    Y = 0f,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    Texture = store.Get("Skin/Back"),
                    ClickAction = () =>
                    {
                        this.Exit();
                    },
                },
                new SpriteButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativePositionAxes = Axes.Both,
                    Y = -0.1f,
                    Texture = store.Get("Skin/Mods"),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    ClickAction = () =>
                    {
                      ModOverlay.State.Value = Visibility.Visible;
                    },
                },
                textBox = new BasicTextBox
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Size = new Vector2(200, 40),
                },
                scrollContainer = new ThisScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.5f,1f),
                    ClickOnMap = () =>
                    {
                        currentMap.Map = new Map(bindablePath.Value);

                        if (!AllowToPlay) return;

                        if (!WaitUntilLoaded)
                        {
                            currentMap.Stop();

                            GameplayScreen gameplayScreen;
                            LoadComponent(gameplayScreen = new GameplayScreen(bindablePath.Value, ModOverlay.modBox.ToApplyMods));
                            this.Push(gameplayScreen);
                        }
                    },
                }
            };

            bindablePath.BindTo(scrollContainer.bindablePath);

            textBox.Current.ValueChanged += e => scrollContainer.search.SearchTerm = e.NewValue;

            scrollContainer.Show();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
                this.Exit();

            return base.OnKeyDown(e);
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero<SongSelcetion>(250, Easing.In);
            Scheduler.AddDelayed(() => WaitUntilLoaded = false, 250);
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next) => Schedule(() => this.Exit());
    }

    public class ThisScrollContainer : FocusedOverlayContainer
    {
        public Bindable<string> bindablePath = new Bindable<string>();

        public Action ClickOnMap;

        private FillFlowContainer FFContainer;

        private ScrollContainer FFContainerM;

        public SearchContainer search;

        private HeaderContainer head;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;

            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 0,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.DimGray,
                    Alpha = 0.9f,
                },
                FFContainerM = new ScrollContainer
                {
                    ScrollbarVisible = true,
                    Depth = -1,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    DistanceDecayScroll = 0.1,
                    ScrollDistance = 0.21415454f,

                    Children = new Drawable[]
                    {
                        FFContainer = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f),
                            Direction = FillDirection.Vertical,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Y,

                            Children = new Drawable[]
                            {
                                search = new SearchContainer
                                {
                                    Anchor = Anchor.TopRight,
                                    Origin = Anchor.TopRight,

                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1f),

                                    AutoSizeAxes = Axes.Y,

                                    Children = new Drawable[]
                                    {
                                        head = new HeaderContainer
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Size = new Vector2(1f),

                                            AutoSizeAxes = Axes.Y,
                                        },
                                    }
                                }
                            }
                        },
                    }
                }
            };

            List<string> list = new List<string>();

            foreach (var dirs in Directory.GetDirectories(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs"))
            {
                int count = GetFiles(dirs, "*.ini", SearchOption.TopDirectoryOnly).Length;
                if (count > 0)
                {
                    list.Add(dirs);
                }
            }

            int[] MapsCount2 = new int[list.Count];
            Map[,] Maps = new Map[MapsCount2.Length, short.MaxValue];

            for (int i = 0; i < list.Count; i++)
            {
                MapsCount2[i] = GetFiles(list[i], "*.ini", SearchOption.TopDirectoryOnly).Length;

                if (MapsCount2[i] > 0)
                {
                    DirectoryInfo d = new DirectoryInfo(list[i]);
                    FileInfo[] Files = d.GetFiles("*.ini");

                    for (int j = 0; j < Files.Length; j++)
                    {
                        Maps[i, j] = new Map(Files[j].FullName);
                    }
                }
            }

            MapPack[] mapPack = new MapPack[MapsCount2.Length];

            for (int i = 0; i < mapPack.Length; i++)
            {
                mapPack[i] = new MapPack
                {
                    Maps = MapsCount2[i],
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Colour = Color4.Blue,
                    Search = Maps[i, 0].Title,
                    Map = Maps,
                    MapPos = i,
                    InvokeBox = ClickOnMap,
                    bindablePath = bindablePath,
                };
            }

            head.AddRange(mapPack);
        }

        private string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
            {
                files.AddRange(Directory.GetFiles(path, sp, searchOption));
            }
            files.Sort();
            return files.ToArray();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var current = e.ScreenSpaceMousePosition.Y;
            float max = this.ScreenSpaceDrawQuad.AABB.Y + this.ScreenSpaceDrawQuad.AABB.Height;

            var x = max / (current - (this.ScreenSpaceDrawQuad.AABB.Y + 1f));
            x = (x / 1.07f);

            FFContainerM.ScrollTo(FFContainer.Height / x);
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            bool Button = e.IsPressed(MouseButton.Right);

            if (Button)
            {
                var current = e.ScreenSpaceMousePosition.Y;
                float max = this.ScreenSpaceDrawQuad.AABB.Y + this.ScreenSpaceDrawQuad.AABB.Height;

                var x = max / (current - (this.ScreenSpaceDrawQuad.AABB.Y + 1f));
                x = (x / 1.07f);

                FFContainerM.ScrollTo(FFContainer.Height / x);
            }
            return base.OnMouseMove(e);
        }
    }

    public class HeaderContainer : Container, IHasFilterableChildren
    {
        public IEnumerable<string> FilterTerms => header.FilterTerms;

        public bool MatchingFilter
        {
            set
            {
                if (value)
                {
                    this.FadeIn();
                }
                else
                {
                    this.FadeOut();
                }
            }
        }

        public bool FilteringActive { get; set; }

        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();

        protected override Container<Drawable> Content => flowContainer;

        private readonly HeaderText header;
        private readonly FillFlowContainer flowContainer;

        public HeaderContainer(string headerText = "")
        {
            AddInternal(header = new HeaderText
            {
                Text = headerText,
            });
            AddInternal(flowContainer = new FillFlowContainer
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,

                RelativeSizeAxes = Axes.X,
                Size = new Vector2(1f, 0f),

                AutoSizeAxes = Axes.Y,

                Direction = FillDirection.Vertical,
            });
        }
    }

    public class HeaderText : SpriteText, IFilterable
    {
        public bool MatchingFilter
        {
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public bool FilteringActive { get; set; }
    }
}