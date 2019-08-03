using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHpBar : TestScene
    {
        private Box _box;

        private float BoxMaxValue = 0.4f;
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                _box = new Box
                {
                    Depth = 0,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativePositionAxes = Axes.Both,
                    Alpha = 1f,
                    Colour = Color4.AliceBlue,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(BoxMaxValue,0.04f),
                    X = 0f,
                    Y = 0f,
                },
            };
            
            AddStep("Set HPBar to 100%", () =>
            {
                _box.ResizeWidthTo(0.4f, 1000, Easing.InOutSine);
            });
            
            AddStep("Set HPBar to 0%", () =>
            {
                _box.ResizeWidthTo(0f, 1000, Easing.InOutSine);
            });
            
            AddStep("Reduce Hp by 10%", () =>
            {
                _box.ResizeWidthTo(_box.Width - (BoxMaxValue / 10), 1000, Easing.OutCirc);
            });
            
            AddStep("Increase Hp by 10%", () =>
            {
                _box.ResizeWidthTo(_box.Width + (BoxMaxValue / 10), 1000, Easing.OutCirc);
            });
        }
    }
}
