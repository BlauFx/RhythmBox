using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using RhythmBox.Window.pending_files;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window
{
    public class RhythmBoxWindow : RhythmBoxResources
    {
        private ScreenStack Stack;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Stack = new ScreenStack { RelativeSizeAxes = Axes.Both, Depth = 0 };

            Children = new Drawable[]
            {
                Stack
            };

            LoadComponentAsync(new MainMenu(), Stack.Push);

            Check_Licenses.License();
        }

        public override void SetHost(GameHost host)
        {
            host.Window.Title = Name;

            base.SetHost(host);
        }
    }
}
