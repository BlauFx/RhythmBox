using System;
using System.Threading.Tasks;
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
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
            => InternalChild = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(100f),
                Texture = store.Get("Skin/LoadingCircle"),
            };

        public async Task Rotate(int milliseconds)
        {
            StartRotating();
            await Task.Delay(milliseconds);
            StopRotating();
        }
        
        public void StartRotating() => InternalChild.RotateTo(0f).RotateTo(360f, 1000).Loop();

        public void StopRotating()
        {
            try
            {
                Schedule(() => InternalChild.ClearTransforms(false, "Rotation"));
            }
            catch (Exception e)
            {
                InternalChild.Alpha = 0;
                Logger.Log($"{e.Message}", LoggingTarget.Runtime, LogLevel.Error);
            }
        }
    }
}
