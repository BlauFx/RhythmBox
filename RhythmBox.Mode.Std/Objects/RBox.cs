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

namespace RhythmBox.Mode.Std.Objects
{
    public class RBox : Container
    {
        public RBox(double time)
        {
            this.time = time;
        }

        /// <summary>
        /// set the time when the drawable should be applied
        /// </summary>
        public double time { get; set; } = 0;

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

        [BackgroundDependencyLoader]
        private void Load()
        {
            Scheduler.AddDelayed(() =>
            {
                Children = new Drawable[]
                {
                    obj = new RBoxObj(speed, direction)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Alpha = 1f,
                        Resuming = Resuming,
                    },
                };

                obj.OnLoadComplete += (e) =>
                {
                    ApplyMods(mods);
                };

            }, time);
        }

        public void OnClickKeyDown(Key key)
        {
            obj.ClickKeyDown(key);

            Scheduler.AddDelayed(() => this.Expire(), 1800 * speed); //TODO:
        }

        private void ApplyMods(List<Mod> mod)
        {
            if (mod is null) return;

            for (int i = 0; i < mod.Count; i++)
            {
                mod[i]?.AppyToHitObj(this);
            }
        }
    }

    public class RBoxObj : Container
    {
        public RBoxObj(float speed, HitObjects.Direction direction)
        {
            this.speed = speed;
            this.direction = direction;
        }

        public Box bx;

        public float speed { get; set; }

        private HitObjects.Direction direction { get; set; }

        private new const int Expire = 300;

        private new const int Clear = 100;

        public Hit currentHit { get; protected set; }

        public BindableBool Resuming = new BindableBool();

        private bool Clicked = false;

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
            });

            var Duration = (1500 * speed) * 0.3;
            var Duration2 = (1500 * speed);
            bx.MoveToY(0f, 0, Easing.InCirc);

            Scheduler.AddDelayed(() =>
            {
                bx.FadeInFromZero((1500 * speed) * 0.2, Easing.None);
            }, Duration);

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

        private async void Remove(int clear, int expire)
        {
            if (!Clicked)
            {
                object[] param = new object[1];

                param[0] = Hit.Hitx;

                await System.Threading.Tasks.Task.Run(() =>
                {
                    _InvokeNamespaceClassesStaticMethod("RhythmBox.Window.Score", "UpdateCombo", param);
                });
            }
            this.ClearTransformsAfter(clear);
            Scheduler.AddDelayed(() => this.Expire(), expire);
            bx.Colour = Color4.Red;

            Scheduler.AddDelayed(() => bx.Colour = Color4.White, 50);

            bx.FadeOut(100);
            bx.ScaleTo(1.1f, 100, Easing.OutCirc);
        }

        public void ClickKeyDown(Key key)
        {
            Clicked = true;
            async void Click(Hit currentHit)
            {
                object[] param = new object[1];

                param[0] = currentHit;

                await System.Threading.Tasks.Task.Run(() =>
                {
                    _InvokeNamespaceClassesStaticMethod("RhythmBox.Window.Score", "UpdateCombo", param);
                });
            }

            if (!Resuming.Value)
            {
                return;
            }

            switch (key)
            {
                case Key.W:
                    {
                        if (direction == HitObjects.Direction.Up)
                        {
                            if (bx.Y <= -0.5 + 0.05f && bx.Y >= -0.50001f)
                            {
                                Click(Hit.Hit300);
                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.Y <= -0.35f && bx.Y >= -0.5f + 0.05f)
                            {
                                Click(Hit.Hit100);
                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.Y <= -0.25f && bx.Y >= -0.35f)
                            {
                                Click(Hit.Hit50);
                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.Y <= 0f && bx.Y >= -0.25f)
                            {
                                Click(Hit.Hitx);
                                Add(HitAnimation(Hit.Hitx, bx.Y));
                            }

                            Remove(Clear, Expire);
                        }

                        break;
                    }

                case Key.A:
                    {
                        if (direction == HitObjects.Direction.Left)
                        {
                            if (bx.X <= -0.5 + 0.05f && bx.X >= -0.50001f)
                            {
                                Click(Hit.Hit300);
                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.X <= -0.35f && bx.Y >= -0.5f + 0.05f)
                            {
                                Click(Hit.Hit100);
                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.X <= -0.25f && bx.Y >= -0.35f)
                            {
                                Click(Hit.Hit50);
                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.X <= 0f && bx.Y >= -0.25f)
                            {
                                Click(Hit.Hitx);
                                Add(HitAnimation(Hit.Hitx, bx.Y));
                            }

                            Remove(Clear, Expire);
                        }

                        break;
                    }

                case Key.S:
                    {
                        if (direction == HitObjects.Direction.Down)
                        {
                            if (bx.Y >= 0.5f - 0.05f && bx.Y <= 0.50001f)
                            {
                                Click(Hit.Hit300);
                                Add(HitAnimation(Hit.Hit300, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0.35f && bx.Y <= 0.5f - 0.05f)
                            {
                                Click(Hit.Hit100);
                                Add(HitAnimation(Hit.Hit100, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0.25f && bx.Y <= 0.35f)
                            {
                                Click(Hit.Hit50);
                                Add(HitAnimation(Hit.Hit50, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0f && bx.Y <= 0.25f)
                            {
                                Click(Hit.Hitx);
                                Add(HitAnimation(Hit.Hitx, bx.Y - 0.05f));
                            }

                            Remove(Clear, Expire);
                        }

                        break;
                    }

                case Key.D:
                    {
                        if (direction == HitObjects.Direction.Right)
                        {
                            if (bx.X >= 0.5 - 0.05f && bx.X <= 0.50001f)
                            {
                                Click(Hit.Hit300);
                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.X >= 0.35f && bx.Y <= 0.5f + 0.05f)
                            {
                                Click(Hit.Hit100);
                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.X >= 0.25f && bx.Y <= 0.35f)
                            {
                                Click(Hit.Hit50);
                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.X >= 0f && bx.Y <= 0.25f)
                            {
                                Click(Hit.Hitx);
                                Add(HitAnimation(Hit.Hitx, bx.Y));
                            }

                            Remove(Clear, Expire);
                        }

                        break;
                    }
            }
        }

        private Drawable HitAnimation(Hit hit, float Y)
        {
            return new HitAnimation(hit)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = bx.X,
                Y = Y,
            };
        }

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
