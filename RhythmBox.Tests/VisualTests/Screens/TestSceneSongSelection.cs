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
using osu.Framework.Logging;
using osuTK.Graphics;
using osuTK.Input;
using osuTK.Platform.Windows;
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
                    Size = new Vector2(0.5f,1f),
                },
            };
            scrollContainer.Show();
        }
    }
    
    internal class TestSceneThisScrollContainer : FocusedOverlayContainer
    {
        private FillFlowContainer FFContainer;

        private TestSceneMyScrollContainer FFContainerM;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;

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
                FFContainerM = new TestSceneMyScrollContainer
                {
                    ScrollbarVisible = true,
                    Depth = -1,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    DistanceDecayScroll = 0.1,
                    ScrollDistance = 0.21415454f,

                    Children = new Drawable[]
                    {
                        FFContainer = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f),
                            Direction = FillDirection.Vertical,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Margin = new MarginPadding{ Top = 0 },
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1f,25f),
                                    Colour = Color4.White,
                                    Alpha = 1f,
                                },
                                new Box
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.Centre,
                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1f,25f),
                                    Colour = Color4.Yellow.Opacity(0.7f),
                                    Alpha = 1f,
                                },
                            }
                        },
                    }
                }
            };

            for (int i = 0; i < 50; i++)
            {
                FFContainer.AddRange(
                    new Drawable[]
                    {
                        new Box
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f,25f),
                            Colour = Color4.AliceBlue.Opacity(0.4f),
                            Alpha = 1f,
                        },
                        new Box
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f,25f),
                            Colour = Color4.Yellow.Opacity(0.2f),
                            Alpha = 1f,
                        }
                    });
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var current = e.ScreenSpaceMousePosition.Y;
            float max = this.ScreenSpaceDrawQuad.AABB.Y + this.ScreenSpaceDrawQuad.AABB.Height;

            var x = max / (current - (this.ScreenSpaceDrawQuad.AABB.Y + 1f));
            x = (x / 1.07f);

            FFContainerM.ScrollTo(FFContainer.Height / x);
            Logger.Log(x.ToString());
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            bool Button = e.IsPressed(MouseButton.Right);

            if (Button)
            {
                var current = e.ScreenSpaceMousePosition.Y;
                float max = this.ScreenSpaceDrawQuad.AABB.Y + this.ScreenSpaceDrawQuad.AABB.Height;

                var x = max / (current - (this.ScreenSpaceDrawQuad.AABB.Y + 1f));
                x = (x / 1.07f);

                FFContainerM.ScrollTo(FFContainer.Height / x);
            }
            return base.OnMouseMove(e);
        }
    }
}
