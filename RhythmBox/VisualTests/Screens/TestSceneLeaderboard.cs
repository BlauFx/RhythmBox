using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Screens.SongSelection;

namespace RhythmBox.Window.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneLeaderboard : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Add Leaderboard", () =>
            {
                Child = new Leaderboard
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.7f, 0.7f),
                    Alpha = 1f,
                };
            });

            AddStep("Remove Leaderboard", () => Child.Expire());
        }
    }
}
