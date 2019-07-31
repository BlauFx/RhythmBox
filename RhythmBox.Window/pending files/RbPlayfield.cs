using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Objects;
using Direction = RhythmBox.Mode.Std.Objects.Direction;

namespace RhythmBox.Window.pending_files
{
    class RbPlayfield : Container
    {
        public Map Map;

        private RBox objBox;

        private RBox[] objBoxArray;

        public TextFlowContainer xd  { get; set; } //TODO

        [BackgroundDependencyLoader]
        private void Load()
        {
            objBoxArray = new RBox[Map.HitObjects.Length];
            
            Children = new Drawable[]
            {
                xd = new TextFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.TopRight,
                    X = -0.01f,
                    Alpha = 0f,
                },
                new RbDrawPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                }, 
            };
        }
        
        protected override void LoadComplete()
        {
            LoadMap();
            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            foreach (var x in objBoxArray)
            {
                if (x.AlphaA > 0)
                {
                    try
                    {
                        switch (e.Key)
                        {
                            case Key.W:
                                x.OnClickKeyDown(Key.W);
                                return base.OnKeyDown(e);
                            case Key.S:
                                x.OnClickKeyDown(Key.S);
                                return base.OnKeyDown(e);
                            case Key.A:
                                x.OnClickKeyDown(Key.A);
                                return base.OnKeyDown(e);
                            case Key.D:
                                x.OnClickKeyDown(Key.D);
                                return base.OnKeyDown(e);
                        }
                    }
                    catch  {  }
                }
            }
            return base.OnKeyDown(e);
        }

        private void LoadMap()
        {
            int i = 0;
            
            //TODO
            foreach (var objBox in Map)
            {
                var x = (Mode.Std.Interfaces.HitObjects) objBox;
                Add(objBoxArray[i] = new RBox
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    direction = x._direction,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    time = x.Time,
                    speed = x.Speed,
                });
                i++;
            }
        }
    }
}
