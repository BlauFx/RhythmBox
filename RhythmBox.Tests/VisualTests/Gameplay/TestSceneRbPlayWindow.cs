using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;

namespace RhythmBox.Tests.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneRbPlayWindow : TestScene
    {
        private int Score { get; set; } = 0;

        private int Combo { get; set; } = 0;

        private int hit300 { get; set; } = 0;

        private int hit100 { get; set; } = 0;

        private int hit50 { get; set; } = 0;

        private int hitx { get; set; } = 0;


        [BackgroundDependencyLoader]
        private void Load()
        {

        }
    }
}
