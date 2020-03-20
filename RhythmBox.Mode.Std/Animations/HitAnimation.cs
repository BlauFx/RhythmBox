using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace RhythmBox.Mode.Std.Animations
{
    public class HitAnimation : Container
    {
        private Sprite hitSprite;

        public Hit Hit { get; set; }

        private TextureStore store { get; set; }

        public int WaitTime => Delay + FadeOutDuration;

        private const int FadeInDuration = 400;
        private const int FadeOutDuration = FadeInDuration;
        private const int Delay = FadeInDuration + 200;

        private const Easing easing = Easing.OutQuart;
        private const Easing RotationEasing1 = Easing.Out;
        private const Easing RotationEasing2 = Easing.In;

        private bool Testing;

        public HitAnimation(Hit hit = Hit.Hit300, bool Testing = false)
        {
            this.Hit = hit;
            this.Testing = Testing;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            this.store = store;

            if (!Testing)
                LoadAndPrepareHitSpirte();
        }

        public void LoadAndPrepareHitSpirte()
        {
            Child = hitSprite = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0f,
                RelativePositionAxes = Axes.Both,
            };

            if (Hit == Hit.Hit300)
                hitSprite.Texture = store.Get("Skin/hit300.png");
            else if (Hit == Hit.Hit100)
                hitSprite.Texture = store.Get("Skin/hit100.png");
            else if (Hit == Hit.Hit50)
                hitSprite.Texture = store.Get("Skin/hit50.png");
            else if (Hit == Hit.Hitx)
            {
                hitSprite.Texture = store.Get("Skin/hitx.png");

                hitSprite.RotateTo(0f).MoveTo(new Vector2(0f)).FadeInFromZero(FadeInDuration, easing);
                hitSprite.Delay(Delay / 2).RotateTo(-15f, Delay, RotationEasing1).MoveToOffset(new Vector2(0f, 0.015f), Delay, RotationEasing2);
            }

            if (Hit != Hit.Hitx)
                hitSprite.FadeInFromZero(FadeInDuration, easing);

            hitSprite.Delay(Delay).FadeOutFromOne(FadeOutDuration, easing).Finally((x) => x.Expire(true));
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
