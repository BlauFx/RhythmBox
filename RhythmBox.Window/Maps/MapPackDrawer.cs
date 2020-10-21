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

namespace RhythmBox.Window.Maps
{
    public class MapPackDrawer : Container, IHasFilterableChildren
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
        
        public IEnumerable<string> FilterTerms => Children.OfType<IHasFilterTerms>().SelectMany(d => d.FilterTerms);
        
        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();
        
        public Bindable<string> bindablePath = new Bindable<string>();

        public Action InvokeBox;

        private readonly string filter;
       
        private MapPackBox parentBox;

        private TextFlowContainer textFlowContainer, textFlowContainer2;

        private Map[] Maps { get; }

        public new Color4 Colour;

        /// <summary>
        /// The size of the parent box
        /// </summary>
        private const float YSize = 50f;

        /// <summary>
        /// the size of the difficulty / child
        /// </summary>
        private const float YChildSize = 40f;

        public MapPackDrawer(Map[] Maps, string filter)
        {
            this.Maps = Maps;
            this.filter = filter;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(1f, 0f);
            AutoSizeAxes = Axes.Y;

            Children = new Drawable[]
            {
                parentBox = new MapPackBox
                {
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(1f, YSize),
                    Colour = Colour,
                    Parent = true,
                    filter = filter,
                    Depth = 0,
                    map = Maps[0],
                },
                textFlowContainer = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.X,
                    Size = new Vector2(0f,parentBox.Height),
                    AutoSizeAxes = Axes.X,
                    TextAnchor = Anchor.Centre,
                    X = parentBox.X,
                    Y = parentBox.Y,
                    Depth = -1f,
                    Text = string.Empty,
                },
                textFlowContainer2 = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.X,
                    Size = new Vector2(0f,parentBox.Height),
                    AutoSizeAxes = Axes.X,
                    TextAnchor = Anchor.Centre,
                    X = parentBox.X,
                    Y = parentBox.Y,
                    Depth = -1f,
                    Text = string.Empty,
                },
            };

            textFlowContainer.Colour = Color4.Black.Opacity(0.8f);
            textFlowContainer2.Colour = Color4.Black.Opacity(0.5f);

            textFlowContainer2.Height = textFlowContainer.Height = parentBox.Height;
            
            textFlowContainer.AddText($"Title: {Maps[0].Title}", x => x.Font = new FontUsage("Roboto", 30));
            textFlowContainer2.AddText($"Artist: {Maps[0].Artist}", x => x.Font = new FontUsage("Roboto", 25));
            
            textFlowContainer.MoveToOffset(new Vector2(0f, -(textFlowContainer.Height / 5)));
            textFlowContainer2.MoveToOffset(new Vector2(0f, (textFlowContainer2.Height / 4)));

            //TODO: float - int
            for (float i = 0; i < Maps.Length; i++)
            {
                Add(new MapPackBox((parentBox.Height * (i+1)) - (((i+1) * 10) - 10f))
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(0.9f - (i / (i + Maps.Length)), YChildSize),
                    Colour = Color4.DarkGreen,
                    filter = filter,
                    map = Maps[(int)i],
                    Invoke = InvokeBox,
                    bindablePath = bindablePath,
                });
            }
        }
    }

    internal class MapPackBox : Container, IHasFilterTerms
    {
        public Bindable<string> bindablePath = new Bindable<string>();

        public Action Invoke;

        public string filter = "null";

        public new bool Parent { get; set; }

        public IEnumerable<string> FilterTerms
        {
            get
            {
                yield return filter;
            }
        }

        private TextFlowContainer textFlowContainer;

        public Map map { get; set; }

        private const Easing easing = Easing.In;
        private const int duration = 500;

        public MapPackBox(float Y = 0)
        {
            if (!Parent)
                this.Y = Y;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativePositionAxes = Axes.X;

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
            });
            
            if (Parent)
                return;

            Add(textFlowContainer = new TextFlowContainer
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.8f),
                    TextAnchor = Anchor.Centre,
                    X = 0.01f,
                    Depth = -1f,
                    Size = new Vector2(0f, this.DrawHeight),
                    AutoSizeAxes = Axes.X
                }
            );

            textFlowContainer.AddText($"Title: {map.DifficultyName}", x => x.Font = new FontUsage("Roboto-Medium", 20));
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Parent)
                return base.OnHover(e);
            
            this.MoveToX(-0.1f, duration, easing);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (Parent)
                base.OnHoverLost(e);
            
            this.MoveToX(0f, duration, easing);
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            bindablePath.Value = this.map.Path;
            Invoke?.Invoke();

            return base.OnMouseDown(e);
        }
    }
}
