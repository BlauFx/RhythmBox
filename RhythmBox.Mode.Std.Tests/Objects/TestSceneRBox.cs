using System.Threading.Tasks;
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

        private RBoxObj obj { get; set; }

        public float AlphaA = xd;

        protected static float xd;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Scheduler.AddDelayed(() => //TODO switch to clock
            {
                Children = new Drawable[]
                {
                    obj = new RBoxObj(direction, speed)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Alpha = 1f,
                    },
                };
            },time);

            UpdateAlphaA();
        }


        protected async void UpdateAlphaA()
        {
            await Task.Delay(100);
            try
            {
                AlphaA = xd = obj.bx.Alpha;
            }
            catch
            {

            }

            UpdateAlphaA();
        }

        public void OnClickKeyDown(Key key)
        {
            obj.ClickKeyDown(key);
            Scheduler.AddDelayed(() => this.Expire(), 1800 * speed);
        }
    }

    internal class RBoxObj : Container
    {
        public RBoxObj(Direction direction, float speed)
        {
            this.speed = speed;
            this.direction = direction;
        }

        public Box bx;

        public float speed { get; set; }

        private Direction direction;

        private const int Expire = 300;

        private const int Clear = 100;

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
            Scheduler.AddDelayed(() => Remove(Clear, Expire), 1800 * speed);
        }

        private void Remove(int clear, int expire)
        {
            this.ClearTransformsAfter(clear);
            Scheduler.AddDelayed(() => this.Expire(), expire);
            bx.Colour = Color4.Red;

            Scheduler.AddDelayed(() => bx.Colour = Color4.White, 50);

            bx.FadeOut(100);
            bx.ScaleTo(1.1f, 100, Easing.OutCirc);
        }

        public void ClickKeyDown(Key key)
        {
            switch (key)
            {
                case Key.W:
                {
                    if (direction == Direction.Up)
                    {
                        if (bx.Y <= -0.5 + 0.05f && bx.Y >= -0.50001f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit300)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y + 0.00f,
                            });
                        }
                        else if (bx.Y <= -0.35f && bx.Y >= -0.5f + 0.05f || bx.Y == -0.5f + 0.05f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit100)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.Y <= -0.25f && bx.Y >= -0.35f || bx.Y == -0.35f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit50)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.Y <= 0f && bx.Y >= -0.25f || bx.Y == -0.25f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hitx)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        Remove(Clear, Expire);
                    }
                        break;
                }

                case Key.A:
                {
                    if (direction == Direction.Left)
                    {
                        if (bx.X <= -0.5 + 0.05f && bx.X >= -0.50001f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit300)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X <= -0.35f && bx.Y >= -0.5f + 0.05f || bx.X == -0.5f + 0.05f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit100)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X <= -0.25f && bx.Y >= -0.35f || bx.X == -0.35f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit50)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X <= 0f && bx.Y >= -0.25f || bx.X == -0.25f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hitx)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        Remove(Clear, Expire);
                    }
                    break;
                }

                case Key.S:
                {
                    if (direction == Direction.Down)
                    {
                        if (bx.Y >= 0.5 - 0.05f && bx.Y <= 0.50001f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit300)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y - 0.05f,
                            });
                        }
                        else if (bx.Y >= 0.35f && bx.Y <= 0.5f - 0.05f || bx.Y == 0.5f - 0.05f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit100)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y - 0.05f,
                            });
                        }
                        else if (bx.Y >= 0.25f && bx.Y <= 0.35f || bx.Y == 0.35f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit50)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y - 0.05f,
                            });
                        }
                        else if (bx.Y >= 0f && bx.Y <= 0.25f || bx.Y == 0.25f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hitx)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y - 0.05f,
                            });
                        }
                        Remove(Clear, Expire);
                    }
                    break;
                }

                case Key.D:
                {
                    if (direction == Direction.Right)
                    {
                        if (bx.X >= 0.5 - 0.05f && bx.X <= 0.50001f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit300)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X >= 0.35f && bx.Y <= 0.5f + 0.05f || bx.X == 0.5f - 0.05f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit100)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X >= 0.25f && bx.Y <= 0.35f || bx.X == 0.35f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hit50)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        else if (bx.X >= 0f && bx.Y <= 0.25f || bx.X == 0.25f)
                        {
                            Add(new TestSceneHitAnimation2(Hit.Hitx)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativePositionAxes = Axes.Both,
                                X = bx.X,
                                Y = bx.Y,
                            });
                        }
                        Remove(Clear, Expire);
                    }
                    break;
                }
            }
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
