using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window.Screens.Result
{
    public class ResultScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Texture = store.Get("Skin/Result")
                },
                new SpriteText
                {
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(600, 20),
                    Colour = Color4.Black,
                    Text = "Test",
                    Font = new FontUsage("Roboto", 40f),
                    X = 0.15f,
                    Y = 0.5f,
                }
            };
        }
    }
}
