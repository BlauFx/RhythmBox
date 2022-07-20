using System;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using RhythmBox.Window.Maps;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using osu.Framework.Platform;

namespace RhythmBox.Window
{
    public class RhythmBoxResources : Game
    {
        protected new DependencyContainer Dependencies;

        private CachedMap cachedMap;

        public RhythmBoxResources()
        {
            Name = "RhythmBox";
        }

        private void OnDragDrop(string filePath)
        {
            if (!File.Exists(filePath)) 
                return;
            
            var file = File.Open(filePath, FileMode.Open);
            var name = Path.GetFileName(file.Name);
            Directory.CreateDirectory($"{Songs.SongPath}/{Path.GetFileNameWithoutExtension(file.Name)}");
            file.Close();
            File.Copy(filePath, $"{Songs.SongPath}/{Path.GetFileNameWithoutExtension(file.Name)}/{name}");

            Songs.GenerateSong($"{Songs.SongPath}/{Path.GetFileNameWithoutExtension(file.Name)}/{name}", Host, Audio);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            return Dependencies;
        }

        protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults() => new Dictionary<FrameworkSetting, object>() { {  FrameworkSetting.WindowedSize, new Size(1600,800) } };

        [BackgroundDependencyLoader]
        private void Load()
        {
            ((SDL2DesktopWindow) this.Window).DragDrop += OnDragDrop;
            Resources.AddStore(new DllResourceStore("RhythmBox.Resources.dll"));

            Dependencies.Cache(new LargeTextureStore(Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"Textures"))));
            Dependencies.Cache(new Gameini(Host.Storage));

            Dependencies.Cache(cachedMap = new CachedMap(Host.Storage));

            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Medium"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Thin"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Roboto-Bold"));

            Add(cachedMap);
        }
    }
}
