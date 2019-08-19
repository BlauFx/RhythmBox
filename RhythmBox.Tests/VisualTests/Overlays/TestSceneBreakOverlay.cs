using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    public class TestSceneBreakOverlay : TestScene
    {
        private TextFlowContainer _text;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                _text = new TextFlowContainer
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextAnchor = Anchor.Centre,
                    Spacing = new Vector2(2f),
                    AutoSizeAxes = Axes.Both,
                    Alpha = 0f,
                }
            };

            AddStep("Animation 1 FadeIn", () =>
            {
                Reset();
                _text.AddText("You've paused the game!", x => x.Font = new FontUsage("Roboto", 100));
                _text.Scale = new Vector2(0f);
                _text.FadeInFromZero(500, Easing.InBack);
                _text.ScaleTo(1f, 2000, Easing.OutElastic);
                Scheduler.AddDelayed(() => _text.MoveToOffset(new Vector2(0f, -0.25f), 500, Easing.In), 1000);
            });

            AddWaitStep("wait for complete", 10);

            AddStep("Animation 1 FadeOut", () =>
            {
                _text.FadeOutFromOne(500, Easing.OutBack);
                _text.MoveToOffset(new Vector2(0f,-0.25f), 500, Easing.In);
            });
        }

        private void Reset()
        {
            _text.Text = string.Empty;
            _text.MoveTo(new Vector2(0f));
        }
    }
}
