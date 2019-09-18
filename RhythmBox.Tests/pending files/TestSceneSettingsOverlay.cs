using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Tests.Objects;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Tests.pending_files
{
    public class TestSceneSettingsOverlay : FocusedOverlayContainer
    {
        private TestSceneCheckbox TestCheckbox;

        private Bindable<bool> bindable1 = new Bindable<bool>();

        [Resolved(CanBeNull = true)]
        private Gameini cfg { get; set; }

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
                new TestSceneScrollContainer
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
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Direction = FillDirection.Vertical,

                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Left = 0, Top = 10, Bottom = 10 },

                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Size = new Vector2(100f,50f),
                                            Colour = Color4.White,
                                            Alpha = 0.8f,
                                            Text = "Options",
                                            Font = new FontUsage("Roboto",40),
                                        },
                                        new Box
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            RelativeSizeAxes = Axes.X,
                                            Size = new Vector2(1f,3f),
                                            Colour = Color4.Black,
                                            Alpha = 1f,
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Top = 10, Bottom = 10 },

                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Size = new Vector2(85f,30f),
                                            Colour = Color4.White,
                                            Alpha = 0.8f,
                                            Text = "Features",
                                            Font = new FontUsage("Roboto",30),
                                        },
                                        new SpriteText
                                        {
                                            Depth = -1,
                                            Anchor = Anchor.TopLeft,
                                            Origin = Anchor.TopLeft,
                                            Size = new Vector2(200f,30f),
                                            Margin = new MarginPadding { Left = 20, Top = 10, Bottom = 10, Right = 0 },
                                            Colour = Color4.White,
                                            Alpha = 0.8f,
                                            Text = "Test:",
                                            Font = new FontUsage("Roboto",25),
                                        },
                                        TestCheckbox = new TestSceneCheckbox
                                        {
                                            Depth = 2,
                                            Anchor = Anchor.TopLeft,
                                            Origin = Anchor.TopLeft,
                                            Texture = "X",
                                            Size = new Vector2(25F),
                                            Alpha = 1F,
                                            Margin = new MarginPadding { Left = 210, Top = -41, Bottom = 10, Right = 0 },
                                            ClickAction = () => ChangeValue(bindable1),
                                            
                                        },
                                        new Box
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            RelativeSizeAxes = Axes.X,
                                            Size = new Vector2(1f,3f),
                                            Colour = Color4.Black,
                                            Alpha = 1f,
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Top = 10, Bottom = 30 },

                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Size = new Vector2(75f,30f),
                                            Colour = Color4.White,
                                            Alpha = 0.8f,
                                            Text = "Version",
                                            Font = new FontUsage("Roboto",30),
                                            Margin = new MarginPadding{ Top = 0, Bottom = -5 }
                                        },
                                        new Box
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Size = new Vector2(70f,3f),
                                            Colour = Color4.Black.Opacity(0.5f),
                                            Alpha = 1f,
                                            Margin = new MarginPadding{ Top = 0, Bottom = 1 }
                                        },
                                        new SpriteText
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Size = new Vector2(110f,20f),
                                            Colour = Color4.White,
                                            Alpha = 0.8f,
                                            Text = "07/16/2019/0",
                                            Font = new FontUsage("Roboto",25),
                                        },
                                    }
                                },
                            }
                        },
                    }
                }
            };
            Bindables();
        }

        private void Bindables()
        {
            bindable1.Value = true;

            if (bindable1.Value)
            {
                TestCheckbox.sp.Texture = TestCheckbox.sptex;
            }
            else
            {
                TestCheckbox.sp.Texture = null;
            }
        }

        private void ChangeValue(Bindable<bool> bindable)
        {
            if (bindable.Value)
            {
                bindable.Value = false;
            }
            else
            {
                bindable.Value = true;
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                PopOut();
            }
            return base.OnKeyDown(e);
        }

        protected override void PopIn()
        {
            this.MoveToX(0f, 200, Easing.In);
            this.FadeIn(400, Easing.In);
            base.PopIn();
        }

        protected override void PopOut()
        {
            this.MoveToX(-0.3f, 200, Easing.In);
            this.FadeOut(400, Easing.In);
            this.Hide();
            base.PopOut();
        }
    }
}
