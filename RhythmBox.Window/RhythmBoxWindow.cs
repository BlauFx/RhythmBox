using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using RhythmBox.Window.pending_files;
using RhythmBox.Window.Screens;

namespace RhythmBox.Window
{
    public class RhythmBoxWindow : RhythmBoxResources
    {
        private ScreenStack Stack;

        private MainMenu mainMenu;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Stack = new ScreenStack { RelativeSizeAxes = Axes.Both, Depth = 0 };

            Child = Stack;
        }

        protected override void LoadComplete()
        {
            LoadComponentAsync(mainMenu = new MainMenu
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(0f)
            }, Stack.Push);

            mainMenu.OnLoadComplete += (e) => mainMenu.TransformTo(nameof(Scale), new Vector2(1f), 1000, Easing.OutExpo);

            Check_Licenses.License();

            base.LoadComplete();
        }

        public override void SetHost(GameHost host)
        {
            host.Window.Title = Name;

            base.SetHost(host);
        }
    }
}
