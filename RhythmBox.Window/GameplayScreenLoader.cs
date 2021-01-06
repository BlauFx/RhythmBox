using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;

namespace RhythmBox.Window
{
    public class GameplayScreenLoader : CompositeDrawable
    {
        private Sprite boxLoading;

        private const double Duration = 1000;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
            => InternalChild = boxLoading = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(100f),
                Texture = store.Get("Skin/LoadingCircle"),
            };

        public void StartRotating() => boxLoading.RotateTo(0f).RotateTo(360f, 1000).Loop();

        public void StopRotating()
        {
            try
            {
                Schedule(() => boxLoading.ClearTransforms(false, "Rotation"));
            }
            catch (Exception e)
            {
                boxLoading.Alpha = 0;
                Logger.Log($"{e.Message}", LoggingTarget.Runtime, LogLevel.Error);
            }
        }
    }
}
