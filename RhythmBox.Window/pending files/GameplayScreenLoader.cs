﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using System.Threading.Tasks;

namespace RhythmBox.Window.pending_files
{
    public class GameplayScreenLoader : Container
    {
        private Sprite boxLoading;

        private bool ShouldRotate = true;

        private float newRotationValue = 360f;

        public float RotationValue = 360f;

        public double Duration = 1000;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                boxLoading = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(100f),
                    Texture = store.Get("Skin/LoadingCircle"),
                }
            };
        }

        protected override void LoadAsyncComplete()
        {
            StartRoating();

            base.LoadAsyncComplete();
        }

        public void StartRoating()
        {
            ShouldRotate = true;
            Rotate();
        }

        private async void Rotate()
        {
            if (ShouldRotate)
            {
                RotationValue = newRotationValue;
                newRotationValue += 360f;

                boxLoading.TransformTo(nameof(Rotation), RotationValue, Duration);
                await Task.Delay((int)Duration);
                Rotate();
            }
        }

        public void StopRotaing()
        {
            ShouldRotate = false;
        }
    }
}