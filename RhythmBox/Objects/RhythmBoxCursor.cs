using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace RhythmBox.Window.Objects
{
    public class RhythmBoxCursor : CursorContainer
    {
        public RhythmBoxCursor(string getForStore)
        {
            this.getFromStore = getForStore;
        }
        
        public string getFromStore { get; set; }
        
        public CursorDrawable cursor { get; set; }
        
        protected override Drawable CreateCursor() => cursor = new CursorDrawable(getFromStore);
    }

    public class CursorDrawable : Container
    {
        public Sprite sprite { get; set; }
        
        public string getFromStore { get; set; }
        
        public CursorDrawable(string getFromStore)
        {
            this.getFromStore = getFromStore;
            
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
        }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        sprite = new Sprite
                        {
                            Texture = store.Get(getFromStore),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                    }
                }
            };
        }
    }
}
