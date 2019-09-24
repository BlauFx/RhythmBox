using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace RhythmBox.Mode.Std.Tests.Animations
{
    public class HitAnimation : Container
    {
        private Sprite hitSprite;

        private Hit hit;

        private const float FadeInTime = 400;

        private const float FadeOutTime = 400;

        private const float WaitTilFadeOutTime = 200;

        private const float HitXRoation = FadeInTime - 100;

        public HitAnimation(Hit hit = Hit.Hit300)
        {
            this.hit = hit;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Child = hitSprite = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Alpha = 0f,
                RelativePositionAxes = Axes.Both,
            };

            if (hit == Hit.Hit300)
            {
                hitSprite.Texture = store.Get("Skin/hit300.png");
                hitSprite.FadeInFromZero(FadeInTime, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitSprite.FadeOutFromOne(FadeOutTime, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), FadeOutTime);
                }, FadeInTime + WaitTilFadeOutTime);
            }
            else if (hit == Hit.Hit100)
            {
                hitSprite.Texture = store.Get("Skin/hit100.png");
                hitSprite.FadeInFromZero(FadeInTime, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitSprite.FadeOutFromOne(FadeOutTime, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), FadeOutTime);
                }, FadeInTime + WaitTilFadeOutTime);
            }
            else if (hit == Hit.Hit50)
            {
                hitSprite.Texture = store.Get("Skin/hit50.png");
                hitSprite.FadeInFromZero(FadeInTime, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitSprite.FadeOutFromOne(FadeOutTime, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), FadeOutTime);
                }, FadeInTime + WaitTilFadeOutTime);
            }
            else if (hit == Hit.Hitx)
            {
                hitSprite.Texture = store.Get("Skin/hitx.png");

                hitSprite.Rotation = 0f;

                hitSprite.FadeInFromZero(FadeInTime, Easing.OutQuart);

                Scheduler.AddDelayed(() =>
                {
                    hitSprite.FadeOutFromOne(FadeOutTime, Easing.OutQuart);
                    Scheduler.AddDelayed(() => this.Expire(), FadeOutTime);
                }, FadeInTime + WaitTilFadeOutTime);

                Scheduler.AddDelayed(() =>
                {
                    hitSprite.RotateTo(-10, FadeInTime + WaitTilFadeOutTime, Easing.Out);
                    hitSprite.MoveToOffset(new Vector2(0f, 0.01f), 600, Easing.In);

                }, HitXRoation);
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
