using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class MainMenu : Screen
    {
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                new Logo
                {
                    Depth = -10,
                    Size = new Vector2(0.4f,0.55f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    texture = store.Get("Game/Logo.png")
                }
            };
        }
    }
}
