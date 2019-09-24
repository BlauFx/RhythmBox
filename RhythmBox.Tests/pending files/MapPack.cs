using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhythmBox.Tests.VisualTests.Screens
{
    public class MapPack : Container, IHasFilterableChildren
    {
        public Bindable<string> bindablePath = new Bindable<string>();

        public Action InvokeBox;

        public string Search = "null";

        public IEnumerable<string> FilterTerms => Children.OfType<IHasFilterTerms>().SelectMany(d => d.FilterTerms);

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

        private BoxTest parentBoxTest;

        private TextFlowContainer textFlowContainer;

        private TextFlowContainer textFlowContainer2;

        public int Maps { get; set; } = 1;

        public Map[,] Map { get; set; }

        public int testSceneMapPos { get; set; }

        public new Color4 Colour;

        //TODO: Maybe store this value in config, so the user can change the value
        /// <summary>
        /// The size of the parent box
        /// </summary>
        public float YSize { get; protected set; } = 50f;

        //TODO: ^
        /// <summary>
        /// the size of the difficulty / child
        /// </summary>
        public float YChildSize { get; protected set; } = 40f;

        [BackgroundDependencyLoader]
        private void Load()
        {
            var testScneneThisMap = Map[testSceneMapPos, 0];

            RelativeSizeAxes = Axes.X;
            Size = new Vector2(1f, 0f);
            AutoSizeAxes = Axes.Y;

            Children = new Drawable[]
            {
                parentBoxTest = new BoxTest
                {
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(1f, YSize),
                    Colour = Colour,
                    Parent = true,
                    Search2 = Search,
                    Depth = 0,
                    testScneneThisMap = testScneneThisMap,
                },
                textFlowContainer = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.X,
                    Size = new Vector2(0f, parentBoxTest.Height),
                    AutoSizeAxes = Axes.X,
                    Text = $"Title: {testScneneThisMap.Title}",
                    Colour = Color4.Black.Opacity(0.8f),
                    TextAnchor = Anchor.Centre,
                    X = parentBoxTest.X,
                    Y = parentBoxTest.Y,
                    Depth = -1f,
                },
                textFlowContainer2 = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.X,
                    Size = new Vector2(0f, parentBoxTest.Height),
                    AutoSizeAxes = Axes.X,
                    Text = $"Artist: {testScneneThisMap.Artist}",
                    Colour = Color4.Black.Opacity(0.5f),
                    TextAnchor = Anchor.Centre,
                    X = parentBoxTest.X,
                    Y = parentBoxTest.Y,
                    Depth = -1f,
                },
            };

            textFlowContainer.Text = string.Empty;
            textFlowContainer2.Text = string.Empty;
            textFlowContainer.AddText($"Title: {testScneneThisMap.Title}", x => x.Font = new FontUsage("Roboto", 30));
            textFlowContainer2.AddText($"Artist: {testScneneThisMap.Artist}", x => x.Font = new FontUsage("Roboto", 25));

            textFlowContainer.Height = parentBoxTest.Height;
            textFlowContainer2.Height = textFlowContainer.Height;
            textFlowContainer.MoveToOffset(new Vector2(0f, -(textFlowContainer.Height / 5)));
            textFlowContainer2.MoveToOffset(new Vector2(0f, (textFlowContainer2.Height / 4)));

            //TODO: float -int
            for (float i = 1; i < Maps + 1; i++)
            {
                var x = Map[testSceneMapPos, (int)i - 1];

                Add(new BoxTest
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(0.9f - (i / (i + Maps)), YChildSize),
                    Colour = Color4.DarkGreen,
                    i = i,
                    parentHeight = parentBoxTest.Height,
                    Search2 = Search,
                    testScneneThisMap = x,
                    Invoke = InvokeBox,
                    bindablePath = bindablePath,
                });
            }
        }

        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();
    }

    internal class BoxTest : Container, IHasFilterTerms
    {
        public float parentHeight = 0;

        public float i = 0;

        public Bindable<string> bindablePath = new Bindable<string>();

        public Action Invoke;

        public string Search2 = "null";

        public new bool Parent { get; set; } = false;

        public IEnumerable<string> FilterTerms
        {
            get
            {
                yield return Search2;
            }
        }

        private TextFlowContainer textFlowContainer;

        public Map testScneneThisMap { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativePositionAxes = Axes.X;

            if (!Parent)
            {
                this.Y = (parentHeight * i) - ((i * 10) - 10f);
            }

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
                textFlowContainer = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.Both,
                    Text = $"Difficulty: {testScneneThisMap.DifficultyName}",
                    Colour = Color4.Black.Opacity(0.8f),
                    TextAnchor = Anchor.Centre,
                    X = 0.01f,
                    Depth = -1f,
                }
            };

            textFlowContainer.Size = new Vector2(0f, this.DrawHeight);
            textFlowContainer.AutoSizeAxes = Axes.X;

            textFlowContainer.Text = string.Empty;
            textFlowContainer.AddText($"Title: {testScneneThisMap.DifficultyName}", x => x.Font = new FontUsage("Roboto-Medium", 20));

            if (Parent)
            {
                textFlowContainer.Expire();
            }
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

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            bindablePath.Value = this.testScneneThisMap.Path;

            //Invoke?.Invoke();

            return base.OnMouseDown(e);
        }
    }
}
