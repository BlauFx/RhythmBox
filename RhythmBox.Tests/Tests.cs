using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;
using RhythmBox.Window;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Tests
{
    public class Tests : RythmBoxResources
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new DrawSizePreservingFillContainer
            {
                Children = new Drawable[]
                {
                    new TestBrowser("RhythmBox"),
                    new CursorContainer(),
                },
            };
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
