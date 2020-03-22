using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using RhythmBox.Window.Maps;
using RhythmBox.Window.pending_files;
using System.Collections.Generic;
using System.Drawing;

namespace RhythmBox.Window
{
    public class RhythmBoxResources : Game
    {
        protected new DependencyContainer Dependencies;

        private LargeTextureStore largeStore;

        public RhythmBoxResources()
        {
            Name = "RhythmBox";
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            return Dependencies;
        }

        protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults()
        {
            return new Dictionary<FrameworkSetting, object>()
            {
                {
                    FrameworkSetting.WindowedSize, new Size(1600,800)
                }
            };
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new DllResourceStore("RhythmBox.Window.Resources.dll"));

            largeStore = new LargeTextureStore(Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"Textures")));

            Dependencies.Cache(largeStore);
            Dependencies.Cache(new Gameini(Host.Storage));
            Dependencies.Cache(new CurrentMap(Host.Storage));
            Dependencies.Cache(this);

            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Medium"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Thin"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Bold"));
        }
    }
}
