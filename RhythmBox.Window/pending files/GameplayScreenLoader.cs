﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using System;
using System.Threading.Tasks;

namespace RhythmBox.Window.pending_files
{
    public class GameplayScreenLoader : CompositeDrawable
    {
        private Sprite boxLoading;

        public double Duration { get; set; } = 1000;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store) 
            => InternalChild = boxLoading = new Sprite
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(100f),
            Texture = store.Get("Skin/LoadingCircle"),
        };

        public void StartRoating() => boxLoading.Spin(Duration, RotationDirection.Clockwise, 0, Int16.MaxValue);

        public async void StopRotaing() => await Task.Run(() => boxLoading.ClearTransforms());
    }
}
