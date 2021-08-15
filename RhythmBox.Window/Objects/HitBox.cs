using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Window.Animation;
using RhythmBox.Window.Interfaces;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Mods;
using RhythmBox.Window.Score;

namespace RhythmBox.Window.Objects
{
    public class HitBox : Container
    {
        public HitObject.DirectionEnum Direction { get; }

        public Box bx;
        
        public BindableBool Resuming { get; set; } = new();

        public List<Mod> Mods { get; init; }

        public double Duration { get; }

        private readonly Key[] keys;
        
        private new double Expire { get; }

        private new double Clear { get; }
        
        private HitAnimation hitAnimation { get; set; }

        private bool clicked;
        
        private bool alreadyRun;
        
        public HitBox(HitObject.DirectionEnum direction, double duration, Key[] keys)
        {
            this.Direction = direction;
            this.Duration = duration;

            this.Expire = duration * 0.3d;
            this.Clear = Expire * 0.5;

            this.keys = keys;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = bx = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Size = new Vector2(0.1f, 0.01f),
                Alpha = 0,
                Depth = int.MinValue
            };

            bx.MoveToY(0f, 0, Easing.InCirc);
            bx.FadeInFromZero(Duration * 0.2, Easing.None);

            Vector2 resizeAmount = Direction switch
            {
                HitObject.DirectionEnum.Up => new Vector2(1f, 0.05f),
                HitObject.DirectionEnum.Down => new Vector2(1f, 0.05f),
                HitObject.DirectionEnum.Left => new Vector2(0.056f, 1f),
                HitObject.DirectionEnum.Right => new Vector2(0.056f, 1f),
                _ => throw new Exception()
            };

            Easing easing = Easing.InCirc; //TODO: Easing.InExpo;

            switch (Direction)
            {
                case HitObject.DirectionEnum.Up:
                    bx.MoveToY(-0.5f, Duration, easing);
                    break;
                case HitObject.DirectionEnum.Down:
                    bx.Rotation = 180f;
                    bx.MoveToY(0.5f, Duration, easing);
                    
                    break;
                case HitObject.DirectionEnum.Left:
                    bx.Origin = Anchor.CentreLeft;
                    bx.Size = new Vector2(0.01f, 0.1f);

                    bx.MoveToX(-0.5f, Duration, easing);
                    break;
                case HitObject.DirectionEnum.Right:
                    bx.Origin = Anchor.CentreRight;
                    bx.Size = new Vector2(0.01f, 0.1f);
                    bx.MoveToX(0.5f, Duration, easing);
                    break;
                default:
                    throw new Exception();
            }

            bx.ResizeTo(resizeAmount, Duration, easing);
            Scheduler.AddDelayed(Remove, Duration + Expire);
        }

        protected override void LoadComplete()
        {
            ApplyMods(Mods);
            base.LoadComplete();
        }

        private void ApplyMods(List<Mod> mods)
        {
            if (mods is null) return;

            for (int i = 0; i < mods.Count; i++)
            {
                if (!(mods[i] is IApplyToHitobject))
                    continue;

                (mods[i] as IApplyToHitobject)?.ApplyToHitObj(this);
            }
        }
        
        private async void Remove()
        {
            if (!alreadyRun) 
                alreadyRun = !alreadyRun;
            
            Scheduler.CancelDelayedTasks();

            bx.Colour = Color4.Red; //TODO: Green
            Scheduler.AddDelayed(() => bx.Colour = Color4.White, this.Clear / 2);

            bx.FadeOut(this.Clear);
            bx.ScaleTo(1.1f, this.Clear, Easing.OutCirc); //TODO: InSine

            if (hitAnimation == null || !clicked)
            {
                Click(Hit.Hitx);
                Add(hitAnimation = HitAnimation(Hit.Hitx));
            }

            await Task.Delay(hitAnimation.WaitTime);
            this.Expire(true);
        }

        public void ClickKeyDown(Key key)
        {
            if (!Resuming.Value) return;
            clicked = true;

            if (key == keys[0] && Direction == HitObject.DirectionEnum.Up)
            {
                Hit? condition = bx.Y switch
                {
                    <= -0.45f and >= -0.50001f => Hit.Hit300,
                    <= -0.35f and >= -0.45f => Hit.Hit100,
                    <= -0.25f and >= -0.35f => Hit.Hitx,
                    _ => null
                };

                var now = condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hit300 ? bx.Y + 0.025f : float.NaN));
                }

                Remove();
            }
            else if (key == keys[1] && Direction == HitObject.DirectionEnum.Left)
            {
                Hit? condition = bx.X switch
                {
                    <= -0.45f and >= -0.50001f => Hit.Hit300,
                    <= -0.35f when bx.Y >= -0.45f => Hit.Hit100,
                    <= -0.25f when bx.Y >= -0.35f => Hit.Hitx,
                    _ => null
                };

                var now = condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hitx ? float.NaN : bx.Y, now == Hit.Hitx ? float.NaN : bx.X + 0.025f));
                }

                Remove();
            }
            else if (key == keys[2] && Direction == HitObject.DirectionEnum.Down)
            {
                Hit? condition = bx.Y switch
                {
                    >= 0.45f and <= 0.50001f => Hit.Hit300,
                    >= 0.35f and <= 0.45f => Hit.Hit100,
                    >= 0.25f and <= 0.35f => Hit.Hitx,
                    _ => null
                };
            
                var now = condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hit300 ? bx.Y - 0.025f : float.NaN));
                }
            
                Remove();
            }
            else if (key == keys[3] && Direction == HitObject.DirectionEnum.Right)
            {
                Hit? condition = bx.X switch
                {
                    >= 0.45f and <= 0.50001f => Hit.Hit300,
                    >= 0.35f when bx.Y <= 0.45f => Hit.Hit100,
                    >= 0.25f when bx.Y <= 0.35f => Hit.Hitx,
                    _ => null
                };
                
                var now = condition;
                if (now != null)
                {
                    Click(now.GetValueOrDefault());
                    Add(hitAnimation = HitAnimation(now.GetValueOrDefault(), now == Hit.Hit300 ? bx.Y : float.NaN, now == Hit.Hit300 ? bx.X - 0.025f : float.NaN));
                }
            
                Remove();
            }
        }
        
        private void Click(Hit hit) => Combo.UpdateCombo(hit);

        private HitAnimation HitAnimation(Hit hit, float Y = float.NaN, float X = float.NaN) 
            => new(hit)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = float.IsNaN(X) ? bx.X : X,
                Y = float.IsNaN(Y) ? bx.Y : Y
            };
    }
}
