﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window
{
    public class RhythmBoxWindow : RhythmBoxResources
    {
        private ScreenStack stack;

        [BackgroundDependencyLoader]
        private void Load() => Child = stack = new ScreenStack { RelativeSizeAxes = Axes.Both, Depth = 0 };

        protected override void LoadComplete()
        {
            _ = new DefaultFolder();
            
            LoadComponentAsync(new MainMenu
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(0f)
            }, stack.Push);

            License.Licenses("https://raw.githubusercontent.com/BlauFx/RhythmBox/master/Licenses",
                new[]
                {
                    "RhythmBox",
                    "osu-framework",
                    "Roboto-Font"
                });

            base.LoadComplete();
        }

        public override void SetHost(GameHost host)
        {
            host.Window.Title = Name;

            base.SetHost(host);
        }
    }
}
