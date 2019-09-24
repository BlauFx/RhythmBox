using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Tests.Animations;
using RhythmBox.Mode.Std.Tests.Interfaces;
using RhythmBox.Mode.Std.Tests.Mods;
using System.Collections.Generic;

namespace RhythmBox.Mode.Std.Tests.Objects
{
    public class RBox : Container
    {
        /// <summary>
        /// set the time when the drawable should be applied
        /// </summary>
        public double time { get; set; } = 0;

        /// <summary>
        /// if speed is higher then the animation / animation of the drawble get's slower
        /// </summary>
        public float speed { get; set; } = 1f;

        public HitObjects.Direction direction;

        public RBoxObj obj { get; set; }

        /// <summary>
        /// AlphaA is the alpha of the drawable
        /// </summary>
        public float AlphaA { get => alpha; protected set => alpha = value; }

        protected float alpha;

        public bool AddCombo { get; protected set; }

        public BindableBool Resuming = new BindableBool();

        public List<Mod> mods { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
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
                    Resuming = Resuming,
                },
            };

            obj.OnLoadComplete += (e) =>
            {
                ApplyMods(mods);
            };

            UpdateAlphaA();
        }

        protected void UpdateAlphaA()
        {
            try
            {
                AlphaA = alpha = obj.bx.Alpha;
            }
            catch { }

            _ = Schedule(() => UpdateAlphaA());
        }

        public void OnClickKeyDown(Key key)
        {
            obj.ClickKeyDown(key);
            this.AddCombo = obj.AddCombo;
            Scheduler.AddDelayed(() => this.Expire(), 1800 * speed); //TODO:
        }

        public bool AddComboToCounter()
        {
            if (obj.Wait == 2 && obj.AddCombo)
            {
                return true;
            }
            return false;
        }

        public bool Miss()
        {
            if (obj.Wait == 2 && !obj.AddCombo)
            {
                return true;
            }
            return false;
        }

        public Hit GetHit()
        {
            return obj.currentHit;
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
        public RBoxObj(HitObjects.Direction direction, float speed)
        {
            this.speed = speed;
            this.direction = direction;
        }

        public Box bx;

        public float speed { get; set; }

        private HitObjects.Direction direction;

        private new const int Expire = 300;

        private new const int Clear = 100;

        public bool AddCombo = false;

        public int Wait = 0;

        public Hit currentHit { get; protected set; }

        public BindableBool Resuming = new BindableBool();

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

            bx.FadeIn(100 * speed);
            bx.MoveToY(0f, 0, Easing.InCirc);

            if (direction == HitObjects.Direction.Up)
            {
                bx.MoveToY(-0.5f, 1500 * speed, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), 1500 * speed, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Down)
            {
                bx.Rotation = 180f;
                bx.MoveToY(0.5f, 1500 * speed, Easing.InCirc);
                bx.ResizeTo(new Vector2(1f, 0.05f), 1500 * speed, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Left)
            {
                bx.Origin = Anchor.CentreLeft;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), 1500 * speed, Easing.InCirc);
                bx.MoveToX(-0.5f, 1500 * speed, Easing.InCirc);
            }
            else if (direction == HitObjects.Direction.Right)
            {
                bx.Origin = Anchor.CentreRight;
                bx.Size = new Vector2(0.01f, 0.1f);
                bx.ResizeTo(new Vector2(0.056f, 1f), 1500 * speed, Easing.InCirc);
                bx.MoveToX(0.5f, 1500 * speed, Easing.InCirc);
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

        public void ClickKeyDown(Key key)
        {
            if (!Resuming.Value)
            {
                return;
            }
            Wait++;

            switch (key)
            {
                case Key.W:
                    {
                        if (direction == HitObjects.Direction.Up)
                        {
                            if (bx.Y <= -0.5 + 0.05f && bx.Y >= -0.50001f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit300;

                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.Y <= -0.35f && bx.Y >= -0.5f + 0.05f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit100;

                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.Y <= -0.25f && bx.Y >= -0.35f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit50;

                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.Y <= 0f && bx.Y >= -0.25f)
                            {
                                //TODO: AddFail = true;
                                AddCombo = false;
                                Wait++;
                                currentHit = Hit.Hitx;

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
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit300;

                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.X <= -0.35f && bx.Y >= -0.5f + 0.05f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit100;

                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.X <= -0.25f && bx.Y >= -0.35f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit50;

                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.X <= 0f && bx.Y >= -0.25f)
                            {
                                AddCombo = false;
                                Wait++;
                                currentHit = Hit.Hitx;

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
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit300;

                                Add(HitAnimation(Hit.Hit300, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0.35f && bx.Y <= 0.5f - 0.05f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit100;

                                Add(HitAnimation(Hit.Hit100, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0.25f && bx.Y <= 0.35f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit50;

                                Add(HitAnimation(Hit.Hit50, bx.Y - 0.05f));
                            }
                            else if (bx.Y >= 0f && bx.Y <= 0.25f)
                            {
                                AddCombo = false;
                                Wait++;
                                currentHit = Hit.Hitx;

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
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit300;

                                Add(HitAnimation(Hit.Hit300, bx.Y));
                            }
                            else if (bx.X >= 0.35f && bx.Y <= 0.5f + 0.05f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit100;

                                Add(HitAnimation(Hit.Hit100, bx.Y));
                            }
                            else if (bx.X >= 0.25f && bx.Y <= 0.35f)
                            {
                                AddCombo = true;
                                Wait++;
                                currentHit = Hit.Hit50;

                                Add(HitAnimation(Hit.Hit50, bx.Y));
                            }
                            else if (bx.X >= 0f && bx.Y <= 0.25f)
                            {
                                AddCombo = false;
                                Wait++;
                                currentHit = Hit.Hitx;

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
    }
}
