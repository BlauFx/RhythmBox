using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;

namespace RhythmBox.Tests.VisualTests.Animations
{
    [TestFixture]
    public class TestSceneHitAnimation : TestScene
    {
        private Sprite hitx;

        private Sprite hit50;

        private Sprite hit100;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                hit100 = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hit100.png"),
                    RelativePositionAxes = Axes.Both,
                },
                hit50 = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hit50.png"),
                    RelativePositionAxes = Axes.Both,
                },
                hitx = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hitx.png"),
                    RelativePositionAxes = Axes.Both,
                }
            };

            AddStep("Start hit100", () =>
            {
                hit100.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hit100.FadeOutFromOne(400, Easing.OutQuart);
                    
                }, 600);
            });

            AddStep("Start hit50", () =>
            {
                hit50.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hit50.FadeOutFromOne(400, Easing.OutQuart);

                }, 600);
            });

            AddStep("Start hitx", () =>
            {
                hitx.Rotation = 0f;

                hitx.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitx.RotateTo(-15, 600, Easing.Out);
                    hitx.MoveToOffset(new Vector2(0f, 0.01f), 600, Easing.In);
                }, 300);

                Scheduler.AddDelayed(() =>
                {
                    hitx.FadeOutFromOne(400, Easing.OutQuart);

                }, 600);
            });
        }
    }
}
