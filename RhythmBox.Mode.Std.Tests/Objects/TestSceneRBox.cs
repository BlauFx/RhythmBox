using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Animations;
using Container = osu.Framework.Graphics.Containers.Container;

namespace RhythmBox.Mode.Std.Tests.Objects
{
    public class TestSceneRBox : Container
    {
        public int time { get; set; } = 0;

        public float speed { get; set; } = 1f;

        public Direction direction;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Scheduler.AddDelayed(() =>
            {
                Children = new Drawable[]
                {
                    new RBoxObj(direction, speed)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                };
            },time);
            
        }
    }

    internal class RBoxObj : Container
    {
        public RBoxObj(Direction direction, float speed)
        {
            this.speed = speed;
            this.direction = direction;
        }

        private Box bx;

        public float speed { get; set; }

        public Direction direction;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Add(bx = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.1f, 0.01f),
                RelativePositionAxes = Axes.Both,
            });

            bx.FadeIn(100*speed);
            bx.MoveToY(0f, 0, Easing.InCirc);

            if (direction == Direction.Up)
            {
                bx.MoveToY(-0.5f, 1500*speed, Easing.InCirc);
            }
            else if (direction == Direction.Down)
            {
                bx.Rotation = 180f;
                bx.MoveToY(0.5f, 1500*speed, Easing.InCirc);
            }
            else if (direction == Direction.Left)
            {
                bx.Rotation = -90f;
                bx.MoveToX(-0.5f, 1500*speed, Easing.InCirc);
            }
            else if (direction == Direction.Right)
            {
                bx.Rotation = 90f;
                bx.MoveToX(0.5f, 1500*speed, Easing.InCirc);
            }

            bx.ResizeTo(new Vector2(1f, 0.05f), 1500*speed, Easing.InCirc);
            Scheduler.AddDelayed(() => Rip(0, 0), 1800*speed);
        }

        private void Rip(int clear, int expire)
        {
            bx.ClearTransformsAfter(clear);
            Scheduler.AddDelayed(() => bx.Expire(), expire);

            bx.Colour = Color4.Red;

            Scheduler.AddDelayed(() => bx.Colour = Color4.White, 50);

            bx.FadeOut(100);
            bx.ScaleTo(1.1f, 100, Easing.OutCirc);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.W)
            {
                if (direction == Direction.Up)
                {
                    if (bx.Y <= -0.5 + 0.05f)
                    {
                        if (runOnce)
                        {
                            runOnce = false;
                            Add(new TestSceneHitAnimation2(Hit.Hit300)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                    }
                    else if (bx.Y >= -0.5f + 0.05f && bx.Y <= -0.3f)
                    {
                        if (runOnce)
                        {
                            runOnce = false;
                            Add(new TestSceneHitAnimation2(Hit.Hit100)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                    }
                    //else if (bx.Y >= -0.3f || bx.Y == -0.3f && bx.Y <= -0.25f)
                    //{
                    //    if (runOnce)
                    //    {
                    //        runOnce = false;
                    //        Add(new TestSceneHitAnimation2(Hit.Hit50)
                    //        {
                    //            Anchor = Anchor.Centre,
                    //            Origin = Anchor.Centre,
                    //            RelativePositionAxes = Axes.Both,
                    //            X = bx.X,
                    //            Y = bx.Y,
                    //        });
                    //    }
                    //}
                }

                Rip(1500, 500);
            }
            return base.OnKeyDown(e);
        }

        private bool runOnce = true;

        protected override void Update()
        {
            
           
            base.Update();
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
