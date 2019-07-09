using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace RhythmBox.Mode.Std.Tests.Animations
{
    public class TestSceneHitAnimation2 : Container
    {
        private Sprite hitx;

        private Sprite hit50;

        private Sprite hit100;

        private Sprite hit300;

        private Hit hit;

        public TestSceneHitAnimation2(Hit hit = Hit.Hit300)
        {
            this.hit = hit;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                hit300 = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hit300.png"),
                    RelativePositionAxes = Axes.Both,
                },
                hit100 = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hit100.png"),
                    RelativePositionAxes = Axes.Both,
                },
                hit50 = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hit50.png"),
                    RelativePositionAxes = Axes.Both,
                },
                hitx = new Sprite
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    Alpha = 0f,
                    Texture = store.Get("Skin/hitx.png"),
                    RelativePositionAxes = Axes.Both,
                }
            };

            if (hit == Hit.Hit300)
            {
                hit300.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hit300.FadeOutFromOne(400, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), 450);

                }, 600);
            }
            else if (hit == Hit.Hit100)
            {
                hit100.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hit100.FadeOutFromOne(400, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), 450);

                }, 600);
            }
            else if(hit == Hit.Hit50)
            {
                hit50.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hit50.FadeOutFromOne(400, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), 450);

                }, 600);
            }
            else if (hit == Hit.Hitx)
            {
                hitx.Rotation = 0f;

                hitx.FadeInFromZero(400, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitx.FadeOutFromOne(400, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), 450);
                }, 600);

                Scheduler.AddDelayed(() =>
                {
                    hitx.RotateTo(-10, 600, Easing.Out);
                    hitx.MoveToOffset(new Vector2(0f, 0.01f), 600, Easing.In);
                    
                }, 300);
            }
        }
    }

    public enum Hit
    {
        Hitx,
        Hit50,
        Hit100,
        Hit300
    }
}
