using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Tests.Objects;
using RhythmBox.Tests.pending_files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelection : TestScene
    {
        public Bindable<string> bindablePath = new Bindable<string>();

        private TestSceneThisScrollContainer scrollContainer;

        private BasicTextBox textBox;

        private bool WaitUntilLoaded = true;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new TestSceneSpriteButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Texture = store.Get("Skin/Back"),
                },
                textBox = new BasicTextBox
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Size = new Vector2(200, 40),
                },
                scrollContainer = new TestSceneThisScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.5f,1f),

                    ClickOnMap = () =>
                    {
                        if (!WaitUntilLoaded)
                        {
                            //Do logic...
                        }
                    },
                },
            };

            bindablePath.BindTo(scrollContainer.bindablePath);

            //Note: This line is OnEntering in SongSelection
            Scheduler.AddDelayed(() => WaitUntilLoaded = false, 250);

            textBox.Current.ValueChanged += e => scrollContainer.search.SearchTerm = e.NewValue;
            scrollContainer.Show();
        }
    }

    public class TestSceneThisScrollContainer : FocusedOverlayContainer
    {
        public Bindable<string> bindablePath = new Bindable<string>();

        public Action ClickOnMap;

        private FillFlowContainer FFContainer;

        private TestSceneMyScrollContainer FFContainerM;

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
                FFContainerM = new TestSceneMyScrollContainer
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
            TestSceneMap[,] testSceneMaps = new TestSceneMap[MapsCount2.Length, short.MaxValue];

            for (int i = 0; i < list.Count; i++)
            {
                MapsCount2[i] = GetFiles(list[i], "*.ini", SearchOption.TopDirectoryOnly).Length;

                if (MapsCount2[i] > 0)
                {
                    DirectoryInfo d = new DirectoryInfo(list[i]);
                    FileInfo[] Files = d.GetFiles("*.ini");

                    for (int j = 0; j < Files.Length; j++)
                    {
                        var testSceneMapReader = new TestSceneMapReader(Files[j].FullName);

                        testSceneMaps[i, j] = new TestSceneMap
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
                }
            }

            MapPackTest[] mapPackTests = new MapPackTest[MapsCount2.Length];

            for (int i = 0; i < mapPackTests.Length; i++)
            {
                mapPackTests[i] = new MapPackTest
                {
                    Maps = MapsCount2[i],
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Colour = Color4.Blue,
                    Search = testSceneMaps[i, 0].Title,
                    testSceneMap = testSceneMaps,
                    testSceneMapPos = i,
                    InvokeBox = ClickOnMap,
                    bindablePath = bindablePath,
                };
            }

            head.AddRange(mapPackTests);
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
                Size = new Vector2(1f),

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
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }

        public bool FilteringActive { get; set; }
    }
}
