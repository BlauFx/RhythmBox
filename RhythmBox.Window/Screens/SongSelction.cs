using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Window.Objects;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class SongSelction : Screen
    {
        private ThisScrollContainer scrollContainer;

        private BasicTextBox textBox;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                new SpriteButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Texture = store.Get("Skin/Back"),
                    ClickAction = () =>
                    {
                        this.Exit();
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
                        GameplayScreen gameplayScreen;
                        LoadComponent(gameplayScreen = new GameplayScreen());
                        this.Push(gameplayScreen);
                        //this.LoadComponentAsync(new GameplayScreen(), this.Push);
                    },
                },
            };

            textBox.Current.ValueChanged += e => scrollContainer.search.SearchTerm = e.NewValue;
            scrollContainer.Show();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
            {
                this.Exit();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero<SongSelction>(200, Easing.In);
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            this.FadeOutFromOne<SongSelction>(50, Easing.In);
            Scheduler.AddDelayed(() => this.Exit(), 50);
            base.OnSuspending(next);
        }
    }

    internal class ThisScrollContainer : FocusedOverlayContainer
    {
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

                                            Children = new Drawable[]
                                            {
                                                new MapPack
                                                {
                                                    Maps = 3,
                                                    RelativeSizeAxes = Axes.Both,
                                                    Anchor = Anchor.TopRight,
                                                    Origin = Anchor.TopRight,
                                                    Colour = Color4.LightYellow,
                                                    Search = "3",
                                                    InvokeBox = ClickOnMap,
                                                }
                                            },
                                        },
                                    }
                                }
                            }
                        },
                    }
                }
            };

            for (int i = 0; i < 100 /*Maps*/; i++)
            {
                head.AddRange(
                    new Drawable[]
                    {
                        new MapPack
                        {
                            Maps = 3,
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Colour = Color4.Blue,
                            Search = "1",
                            InvokeBox = ClickOnMap,
                        },
                        new MapPack
                        {
                            Maps = 10,
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Colour = Color4.Pink,
                            Search = "2",
                            InvokeBox = ClickOnMap,
                        }
                    });
            }
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

    internal class HeaderContainer : Container, IHasFilterableChildren
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

    internal class HeaderText : SpriteText, IFilterable
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
