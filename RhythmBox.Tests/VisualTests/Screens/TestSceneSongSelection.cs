using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Tests.Objects;
using RhythmBox.Window.pending_files;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using RhythmBox.Tests.pending_files;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSongSelection : TestScene
    {
        private TestSceneThisScrollContainer scrollContainer;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                scrollContainer = new TestSceneThisScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(0.4f,0.1f),
                },
            };
            scrollContainer.Show();
        }
    }
    
    internal class TestSceneThisScrollContainer : FocusedOverlayContainer
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Size = new Vector2(0.3f, 1f);

            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 0,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.DimGray,
                    Alpha = 0.9f,
                },
                new TestSceneMyScrollContainer
                {
                    ScrollbarVisible = true,
                    Depth = -1,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    DistanceDecayScroll = 0.005,

                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(1f),
                            Direction = FillDirection.Vertical,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Padding = new MarginPadding{ Top = 10 },

                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.Centre,
                                    RelativeSizeAxes = Axes.Both,
                                    Size = new Vector2(1f,3f),
                                    Colour = Color4.White,
                                    Alpha = 1f,
                                }
                            }
                        },
                    }
                }
            };
        }
    }
}
