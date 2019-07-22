using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using RhythmBox.Window.pending_files;
using System.Collections.Generic;
using System.Drawing;

namespace RhythmBox.Window
{
    public class RythmBoxResources : Game
    {
        protected new DependencyContainer Dependencies;

        public RythmBoxResources()
        {
            Name = "RhythmBox";
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            Dependencies.Cache(this);
            Dependencies.Cache(new Gameini(Host.Storage));

            return base.CreateChildDependencies(parent);
        }

        //protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults() =>
        //    new Dictionary<FrameworkSetting, object>()
        //    {
        //        {
        //            FrameworkSetting.WindowedSize, new Size(700,500)
        //        }
        //    };

        protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults()
        {
            new Dictionary<FrameworkSetting, object>()
            {
                {
                    FrameworkSetting.WindowedSize, new Size(700,500)

                }
            };
            return base.GetFrameworkConfigDefaults();
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new DllResourceStore("RhythmBox.Window.Resources.dll"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Medium"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Thin"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Bold"));
        }
    }
}
