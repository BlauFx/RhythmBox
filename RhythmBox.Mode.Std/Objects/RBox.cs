using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Mode.Std.Mods;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using RhythmBox.Mode.Std.Mods.Interfaces;

namespace RhythmBox.Mode.Std.Objects
{
    public class RBox : Container
    {
        /// <summary>
        /// if speed is higher then the animation / animation of the drawble get's slower
        /// </summary>
        public float speed { get; }

        public HitObjects.Direction direction { get; }

        public RBoxObj obj { get; private set; }

        /// <summary>
        /// AlphaA is the alpha of the drawable
        /// </summary>
        public float AlphaA => obj.bx.Alpha;

        public BindableBool Resuming { get; set; } = new BindableBool();

        public List<Mod> mods { get; set; }

        public double Duration { get; }

        private readonly Key[] keys;

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
            Child = obj = new RBoxObj(direction, Duration, keys)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 1f,
                Resuming = Resuming,
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
                if (!(mod[i] is IApplyToHitobject))
                    continue;

                (mod[i] as IApplyToHitobject)?.ApplyToHitObj(this);
            }
        }
    }

    public delegate void DisposableBxHandler(EventArgs e);

    public class RBoxObj : Container
    {
        public RBoxObj(HitObjects.Direction direction, double duration, Key[] keys)
        {
            this.direction = direction;
            this.Duration = duration;

            this.Expire = duration * 0.3d;
            this.Clear = Expire * 0.5;

            this.keys = keys;
        }

        public Box bx;

        private HitObjects.Direction direction { get; set; }

        private new double Expire { get; set; }

        private new double Clear { get; set; }

        private double Duration { get; set; }

        public BindableBool Resuming = new BindableBool();

        private bool Clicked = false;

        public event DisposableBxHandler DisposableBx;

        private HitAnimation hitAnimation { get; set; } = null;

        private readonly Key[] keys;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = bx = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.1f, 0.01f),
                RelativePositionAxes = Axes.Both,
                Alpha = 0,
                Depth = int.MinValue
            };

            bx.MoveToY(0f, 0, Easing.InCirc);
            bx.FadeInFromZero(Duration * 0.2, Easing.None);

            Vector2 ResizeAmount = direction switch
            {
                HitObjects.Direction.Up => new Vector2(1f, 0.05f),
                HitObjects.Direction.Down => new Vector2(1f, 0.05f),
                HitObjects.Direction.Left => new Vector2(0.056f, 1f),
                HitObjects.Direction.Right => new Vector2(0.056f, 1f),
                _ => throw new Exception()
            };

            switch (direction)
            {
                case HitObjects.Direction.Up:
                    bx.MoveToY(-0.5f, Duration, Easing.InCirc);
                    break;
                case HitObjects.Direction.Down:
                    bx.Rotation = 180f;
                    bx.MoveToY(0.5f, Duration, Easing.InCirc);
                    
                    break;
                case HitObjects.Direction.Left:
                    bx.Origin = Anchor.CentreLeft;
                    bx.Size = new Vector2(0.01f, 0.1f);
                        
                    bx.MoveToX(-0.5f, Duration, Easing.InCirc);
                    break;
                case HitObjects.Direction.Right:
                    bx.Origin = Anchor.CentreRight;
                    bx.Size = new Vector2(0.01f, 0.1f);
                    bx.MoveToX(0.5f, Duration, Easing.InCirc);
                    break;
                default:
                    throw new Exception();
            }

            bx.ResizeTo(ResizeAmount, Duration, Easing.InCirc);
            Scheduler.AddDelayed(Remove, Duration + Expire);
        }

        private async void Click(Hit hit) => await Task.Run(() => _InvokeNamespaceClassesStaticMethod("RhythmBox.Window.Score", "UpdateCombo", hit));
        
        private void Remove()
        {
            async void WaitAndInvoke()
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(hitAnimation.WaitTime);
                    DisposableBx?.Invoke(new EventArgs());
                });
            }
            
            Scheduler.CancelDelayedTasks();

            if (!Clicked)
            {
                Click(Hit.Hitx);
                Add(hitAnimation = HitAnimation(Hit.Hitx));
            }

            bx.Colour = Color4.Red;
            Scheduler.AddDelayed(() => bx.Colour = Color4.White, this.Clear / 2);

            bx.FadeOut(this.Clear);
            bx.ScaleTo(1.1f, this.Clear, Easing.OutCirc);

            if (hitAnimation == null)
            {
                Click(Hit.Hitx);
                Add(hitAnimation = HitAnimation(Hit.Hitx));
            }

            WaitAndInvoke();
        }

        public void ClickKeyDown(Key key)
        {
            Clicked = true;
            if (!Resuming.Value) return;

            if (key == keys[0] && direction == HitObjects.Direction.Up)
            {
                Hit? Condition = bx.Y switch
                {
                    <= -0.5f + 0.05f and >= -0.50001f => Hit.Hit300,
                    <= -0.35f and >= -0.5f + 0.05f => Hit.Hit100,
                    <= -0.25f and >= -0.35f => Hit.Hitx,
                    _ => null
                };

                var now = Condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hit300 ? bx.Y + 0.025f : float.NaN));
                }

                Remove();
            }
            else if (key == keys[1] && direction == HitObjects.Direction.Left)
            {
                Hit? ConditionLeft = bx.X switch
                {
                    <= -0.5f + 0.05f and >= -0.50001f => Hit.Hit300,
                    _ => null
                };

                if (ConditionLeft == Hit.Hit300)
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
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();
            }
            else if (key == keys[2] && direction == HitObjects.Direction.Down)
            {
                Hit? Condition = bx.Y switch
                {
                    >= 0.5f - 0.05f and <= 0.50001f => Hit.Hit300,
                    >= 0.35f and <= 0.5f - 0.05f => Hit.Hit100,
                    >= 0.25f and <= 0.35f => Hit.Hitx,
                    _ => null
                };

                var now = Condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hit300 ? bx.Y - 0.025f : float.NaN));
                }

                Remove();
            }
            else if (key == keys[3] && direction == HitObjects.Direction.Right)
            {
                Hit? ConditionRight = bx.X switch
                {
                    >= 0.5f - 0.05f and <= 0.50001f => Hit.Hit300,
                    _ => null
                };

                if (ConditionRight == Hit.Hit300)
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
                    Click(Hit.Hitx);
                    Add(hitAnimation = HitAnimation(Hit.Hitx));
                }

                Remove();
            }
        }

        private HitAnimation HitAnimation(Hit hit, float Y = float.NaN, float X = float.NaN) 
            => new HitAnimation(hit)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = float.IsNaN(X) ? bx.X : X,
                Y = float.IsNaN(Y) ? bx.Y : Y
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
                        if ((_t.Namespace == namespaceName) && _t.IsClass)
                        {
                            _t.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public)?.Invoke(null, parameters);
                            return;
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
