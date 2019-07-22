using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelectionBeatmapPack : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new BeatmapTest
                {
                    Maps = 5,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(0.5f,0.1f),
                }
            };
        }
    }

    public class BeatmapTest : Container, IHasFilterableChildren
    {
        public string Search = "null";

        public IEnumerable<string> FilterTerms => Children.OfType<IHasFilterTerms>().SelectMany(d => d.FilterTerms);

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

        private BoxTest parentBoxTest;

        public int Maps { get; set; } = 1;

        public Color4 Colour;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            //Size = new Vector2(1.5f, 40 * (Maps + 1));
            Size = new Vector2(1f,40*(Maps+1));

            Children = new Drawable[]
            {
                parentBoxTest = new BoxTest
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(1f,40f),
                    Colour = Colour,
                    Parent = true,
                    Search2 = Search,
                },
            };

            for (float i = 1; i < Maps+1; i++)
            {
                Add(new BoxTest
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2((0.9f - (i / (i + Maps))), 40f),
                    Colour = Color4.DarkGreen,
                    Y = (parentBoxTest.Height*i),
                    Search2 = Search,
                }); ;
            }
        }

        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();
    }

    internal class BoxTest : Box, IHasFilterTerms
    {
        public string Search2 = "null";

        public bool Parent { get; set; } = false;

        public IEnumerable<string> FilterTerms
        {
            get
            {
                yield return Search2;
            }
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativePositionAxes = Axes.X;
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Parent)
            {
                return base.OnHover(e);
            }
            this.MoveToX(-0.1f, 500, Easing.In);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (Parent)
            {
                base.OnHoverLost(e);
            }
            this.MoveToX(0f, 500, Easing.In);
            base.OnHoverLost(e);
        }
    }
}
