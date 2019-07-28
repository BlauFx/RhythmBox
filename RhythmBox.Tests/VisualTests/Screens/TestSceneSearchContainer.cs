using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSearchContainer : TestScene
    {
        //https://github.com/ppy/osu-framework/blob/master/osu.Framework.Tests/Visual/UserInterface/TestSceneSearchContainer.cs

        private SearchContainer search;
        private BasicTextBox textBox;

        [SetUp]
        public void SetUp()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;

            Schedule(() =>
            {
                Children = new Drawable[]
                {
                    textBox = new BasicTextBox
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Size = new Vector2(430, 40),
                    },
                    search = new SearchContainer
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        AutoSizeAxes = Axes.Both,
                        Margin = new MarginPadding { Top = 40 },
                        Children = new Drawable[]
                        {
                            new HeaderContainer
                            {
                                AutoSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new MapPackTest
                                    {
                                        Maps = 5,
                                        RelativeSizeAxes = Axes.X,
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        Colour = Color4.Red,
                                        Search = "1",
                                    },
                                    new MapPackTest
                                    {
                                        Maps = 5,
                                        RelativeSizeAxes = Axes.X,
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        Colour = Color4.Orange,
                                        Search = "2",
                                    },
                                    new MapPackTest
                                    {
                                        Maps = 5,
                                        RelativeSizeAxes = Axes.X,
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        Colour = Color4.Violet,
                                        Search = "3",
                                    },
                                    new Box
                                    {
                                        Size = new Vector2(400f),
                                        Alpha = 0.001f,
                                    },
                                },
                            },
                        }
                    }
                };
                textBox.Current.ValueChanged += e => search.SearchTerm = e.NewValue;
            });
        }

        private class HeaderContainer : Container, IHasFilterableChildren
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
                    Margin = new MarginPadding { Left = 30 },
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                });
            }
        }

        private class HeaderText : SpriteText, IFilterable
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
}