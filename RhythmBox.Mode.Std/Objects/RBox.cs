using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Mode.Std.Mods;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace RhythmBox.Mode.Std.Objects
{
    public class RBox : Container
    {
        /// <summary>
        /// if speed is higher then the animation / animation of the drawble get's slower
        /// </summary>
        public float speed { get; set; } = 1f;

        public HitObjects.Direction direction { get; set; }

        public RBoxObj obj { get; set; }

        /// <summary>
        /// AlphaA is the alpha of the drawable
        /// </summary>
        public float AlphaA => obj.bx.Alpha;

        public BindableBool Resuming { get; set; } = new BindableBool();

        public List<Mod> mods { get; set; }

        public double Duration { get; set; }

        private Key[] keys = new Key[4];

        public RBox(float speed, HitObjects.Direction direction, double Duration, Key[] keys)
        {
            this.speed = speed;
            this.direction = direction;
            this.Duration = Duration;

            this.keys = keys;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                obj = new RBoxObj(speed, direction, Duration, keys)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1f,
                    Resuming = Resuming,
                },
            };

            obj.OnLoadComplete += (e) => ApplyMods(mods);
            obj.DisposableBx += (e) => this.Expire(true);
        }

        public void OnClickKeyDown(Key e) => obj.ClickKeyDown(e);

        private void ApplyMods(List<Mod> mod)
        {
            if (mod is null) return;

            for (int i = 0; i < mod.Count; i++)
            {
                mod[i]?.AppyToHitObj(this);
            }
        }
    }

    public delegate void DisposableBxHandler(EventArgs e);

    public class RBoxObj : Container
    {
        public RBoxObj(float speed, HitObjects.Direction direction, double DurationTime, Key[] keys)
        {
            this.direction = direction;
            this.Duration = DurationTime * speed;

            //TODO: Leave it 300 or muiltply it with speed?
            this.Expire = 300; // speed;
            this.Clear = this.Expire * 0.5;

            this.keys = keys;
        }

        public Box bx;

        private HitObjects.Direction direction { get; set; }

        private new int Expire { get; set; }

        private new double Clear { get; set; }

        private double Duration { get; set; }

        public Hit currentHit { get; protected set; }

        public BindableBool Resuming = new BindableBool();

        private bool Clicked = false;

        public event DisposableBxHandler DisposableBx;

        private HitAnimation hitAnimation { get; set; } = null;

        private Key[] keys = new Key[4];

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
                Alpha = 0,
                Depth = int.MinValue
            });

            bx.MoveToY(0f, 0, Easing.InCirc);
            bx.FadeInFromZero(Duration * 0.2, Easing.None);

            if (direction == HitObjects.Direction.Up)
            {
                bx.MoveToY(-0.5f, Duration, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), Duration, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Down)
            {
                bx.Rotation = 180f;
                bx.MoveToY(0.5f, Duration, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), Duration, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Left)
            {
                bx.Origin = Anchor.CentreLeft;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), Duration, Easing.InCirc);
                bx.MoveToX(-0.5f, Duration, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Right)
            {
                bx.Origin = Anchor.CentreRight;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), Duration, Easing.InCirc);
                bx.MoveToX(0.5f, Duration, Easing.InCirc);
            }

            Scheduler.AddDelayed(() => Remove(), Duration + Expire);
        }

        private async void Remove()
        {
            Scheduler.CancelDelayedTasks();

            if (!Clicked)
            {
                object[] param = new object[1];

                param[0] = Hit.Hitx;

                await Task.Run(() =>
                {
                    _InvokeNamespaceClassesStaticMethod("RhythmBox.Window.Score", "UpdateCombo", param);
                });

                Schedule(() => Add(hitAnimation = HitAnimation(Hit.Hitx)));
            }

            bx.Colour = Color4.Red;

            Scheduler.AddDelayed(() => bx.Colour = Color4.White, this.Clear / 2);

            bx.FadeOut(this.Clear);

            bx.ScaleTo(1.1f, this.Clear, Easing.OutCirc);

            async void WaitAndInvoke()
            {
                //We need to wait so the HitAnimation class can finish it's animation.
                await Task.Run(async () =>
                {
                    //TODO:
                    await Task.Delay(hitAnimation.WaitTime);
                    DisposableBx?.Invoke(new EventArgs());
                });
            }

            if (hitAnimation != null)
            {
                WaitAndInvoke();
                return;
            }

            Schedule(() =>
            {
                Add(hitAnimation = HitAnimation(Hit.Hitx));

                WaitAndInvoke();
            });
        }

        public void ClickKeyDown(Key key)
        {
            Clicked = true;

            async void Click(Hit currentHit)
            {
                object[] param = new object[1];

                param[0] = currentHit;

                await Task.Run(() =>
                {
                    _InvokeNamespaceClassesStaticMethod("RhythmBox.Window.Score", "UpdateCombo", param);
                });
            }

            if (!Resuming.Value) return;

            if (key == keys[0] && direction == HitObjects.Direction.Up)
            {
                if (bx.Y <= -0.5 + 0.05f && bx.Y >= -0.50001f)
                {
                    Click(Hit.Hit300);
                    Add(hitAnimation = HitAnimation(Hit.Hit300, bx.Y + 0.025f));
                }
                else if (bx.Y <= -0.35f && bx.Y >= -0.5f + 0.05f)
                {
                    Click(Hit.Hit100);
                    Add(hitAnimation = HitAnimation(Hit.Hit100));
                }
                else if (bx.Y <= -0.25f && bx.Y >= -0.35f)
                {
                    Click(Hit.Hit50);
                    Add(hitAnimation = HitAnimation(Hit.Hit50));
                }
                else if (bx.Y <= 0f && bx.Y >= -0.25f)
                {
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();
            }
            else if (key == keys[1] && direction == HitObjects.Direction.Left)
            {
                if (bx.X <= -0.5 + 0.05f && bx.X >= -0.50001f)
                {
                    Click(Hit.Hit300);
                    Add(hitAnimation = HitAnimation(Hit.Hit300, bx.Y, bx.X + 0.025f));
                }
                else if (bx.X <= -0.35f && bx.Y >= -0.5f + 0.05f)
                {
                    Click(Hit.Hit100);
                    Add(hitAnimation = HitAnimation(Hit.Hit100, bx.Y, bx.X + 0.025f));
                }
                else if (bx.X <= -0.25f && bx.Y >= -0.35f)
                {
                    Click(Hit.Hit50);
                    Add(hitAnimation = HitAnimation(Hit.Hit50, bx.Y, bx.X + 0.025f));
                }
                else if (bx.X <= 0f && bx.Y >= -0.25f)
                {
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();
            }
            else if (key == keys[2] && direction == HitObjects.Direction.Down)
            {
                if (bx.Y >= 0.5f - 0.05f && bx.Y <= 0.50001f)
                {
                    Click(Hit.Hit300);
                    Add(hitAnimation = HitAnimation(Hit.Hit300, bx.Y - 0.025f));
                }
                else if (bx.Y >= 0.35f && bx.Y <= 0.5f - 0.05f)
                {
                    Click(Hit.Hit100);
                    Add(hitAnimation = HitAnimation(Hit.Hit100));
                }
                else if (bx.Y >= 0.25f && bx.Y <= 0.35f)
                {
                    Click(Hit.Hit50);
                    Add(hitAnimation = HitAnimation(Hit.Hit50));
                }
                else if (bx.Y >= 0f && bx.Y <= 0.25f)
                {
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();
            }
            else if (key == keys[3] && direction == HitObjects.Direction.Right)
            {
                if (bx.X >= 0.5 - 0.05f && bx.X <= 0.50001f)
                {
                    Click(Hit.Hit300);
                    Add(hitAnimation = HitAnimation(Hit.Hit300, bx.Y, bx.X - 0.025f));
                }
                else if (bx.X >= 0.35f && bx.Y <= 0.5f + 0.05f)
                {
                    Click(Hit.Hit100);
                    Add(hitAnimation = HitAnimation(Hit.Hit100));
                }
                else if (bx.X >= 0.25f && bx.Y <= 0.35f)
                {
                    Click(Hit.Hit50);
                    Add(hitAnimation = HitAnimation(Hit.Hit50));
                }
                else if (bx.X >= 0f && bx.Y <= 0.25f)
                {
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();

            }
        }

        private HitAnimation HitAnimation(Hit hit, float Y = int.MaxValue, float X = int.MaxValue) =>
            new HitAnimation(hit)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = X == int.MaxValue ? bx.X : X,
                Y = Y == int.MaxValue ? bx.Y : Y,
            };

        //https://stackoverflow.com/a/48728076
        private void _InvokeNamespaceClassesStaticMethod(string namespaceName, string methodName, params object[] parameters)
        {
            foreach (var _a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var _t in _a.GetTypes())
                {
                    try
                    {
                        if ((_t.Namespace == namespaceName) && _t.IsClass) _t.GetMethod(methodName, (BindingFlags.Static | BindingFlags.Public))?.Invoke(null, parameters);
                    }
                    catch { }
                }
            }
        }
    }
}
