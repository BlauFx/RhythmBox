using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Performance;
using osuTK;

namespace RhythmBox.Window
{
    public class RythmBoxResources : Game
    {
        public RythmBoxResources()
        {
            Name = "RhythmBox";
        }

        [BackgroundDependencyLoader]
        private void Load(FrameworkConfigManager frameworkConfigManager)
        {
            Resources.AddStore(new DllResourceStore("RhythmBox.Window.Resources.dll"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Medium"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Thin"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Bold"));
        }
    }
}
