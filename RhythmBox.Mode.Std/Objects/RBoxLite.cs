using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Interfaces;

namespace RhythmBox.Mode.Std.Objects
{
    public class RBoxLite : Container
    {
        public RBoxLite(double SpawnTime, float SeekTime, bool Animation = true)
        {
            this.SpawnTime = SpawnTime;
            this.TimeToSeek = SeekTime;
            this.Animation = Animation;
        }

        public float TimeToSeek { get; set; }
        public double SpawnTime { get; set; }

        private bool Animation { get; set; }

        public float speed { get; set; } = 1f;

        public HitObjects.Direction direction { get; set; }

        public RBoxObjLite obj { get; set; }

        /// <summary>
        /// AlphaA is the alpha of the drawable
        /// </summary>
        public float AlphaA => obj.bx.Alpha;


        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                obj = new RBoxObjLite(SpawnTime, TimeToSeek, speed, direction, Animation)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                },
            };
        }
    }

    public class RBoxObjLite : Container
    {
        public RBoxObjLite(double SpawnTime, float TimeToSeek, float speed, HitObjects.Direction direction, bool Animation)
        {
            this.SpawnTime = SpawnTime;
            this.TimeToSeek = TimeToSeek;
            this.speed = speed;
            this.direction = direction;
            this.Animation = Animation;
        }

        private bool Animation { get; set; }

        public Box bx;

        public float speed { get; set; }

        public float TimeToSeek { get; set; }

        public double SpawnTime { get; set; }

        private HitObjects.Direction direction { get; set; }

        private new const int Expire = 300;

        private new const int Clear = 100;

        [BackgroundDependencyLoader]
        private void Load()
        {
            if ((TimeToSeek > SpawnTime))
            {
                return;
            }

            Add(bx = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.1f, 0.01f),
                RelativePositionAxes = Axes.Both,
                Alpha = 0,
            });

            var Duration = (1500 * speed) * 0.3;
            var Duration2 = (1500 * speed) * 1.0;
            bx.MoveToY(0f, 0, Easing.InCirc);

            Scheduler.AddDelayed(() =>
            {
                bx.FadeInFromZero((1500 * speed) * 0.2, Easing.None);
            }, Duration);
          

            if (!Animation)
            {
                Duration = 0;
                Duration2 = 0;
            }

            if (true)
            {
                var CurrentTime = this.Clock.CurrentTime;


                if (CurrentTime > TimeToSeek)
                {
                }
                else
                {
                    var Diff = CurrentTime - TimeToSeek;

                    if (Diff > 0)
                    {
                        Duration = Diff;
                        Duration2 = Diff;
                    }

                }
            }

            if (direction == HitObjects.Direction.Up)
            {
                bx.MoveToY(-0.5f, Duration2, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), Duration2, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Down)
            {
                bx.Rotation = 180f;
                bx.MoveToY(0.5f, Duration2, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), Duration2, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Left)
            {
                bx.Origin = Anchor.CentreLeft;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), Duration2, Easing.InCirc);
                bx.MoveToX(-0.5f, Duration2, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Right)
            {
                bx.Origin = Anchor.CentreRight;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), Duration2, Easing.InCirc);
                bx.MoveToX(0.5f, Duration2, Easing.InCirc);
            }

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
    }
}
