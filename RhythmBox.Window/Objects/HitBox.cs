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

            bx.Colour = Color4.Green;
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
            if (!Resuming.Value)
                return;

            clicked = true;

            if (key == keys[0] && Direction == HitObject.DirectionEnum.Up)
                Execute(bx.Y, Direction);
            else if (key == keys[1] && Direction == HitObject.DirectionEnum.Left)
                Execute(bx.X, Direction);
            else if (key == keys[2] && Direction == HitObject.DirectionEnum.Down)
                Execute(bx.Y, Direction);
            else if (key == keys[3] && Direction == HitObject.DirectionEnum.Right)
                Execute(bx.X, Direction);
        }

        private void Execute(float value, HitObject.DirectionEnum directionEnum)
        {
            var hitResult = GetHitResult(value);
            if (hitResult == null)
                return;

            var hit = hitResult.GetValueOrDefault();

            float XCalc()
            {
                return directionEnum switch
                {
                    HitObject.DirectionEnum.Left => hit == Hit.Hitx ? float.NaN : bx.X + 0.025f,
                    HitObject.DirectionEnum.Right => hit == Hit.Hit300 ? bx.X - 0.025f : float.NaN,
                    _ => float.NaN,
                };
            }

            float YCalc()
            {
                float calc(float value) => hit == Hit.Hit300 ? bx.Y + value: float.NaN;

                return directionEnum switch
                {
                    HitObject.DirectionEnum.Up => calc(0.025f),
                    HitObject.DirectionEnum.Down => calc(-0.025f),
                    HitObject.DirectionEnum.Left or HitObject.DirectionEnum.Right => bx.Y,
                    _ => float.NaN,
                };
            }

            Click(hit);
            switch (directionEnum)
            {
                case HitObject.DirectionEnum.Up or HitObject.DirectionEnum.Down:
                    Add(hitAnimation = HitAnimation(hit, YCalc()));
                    break;
                case HitObject.DirectionEnum.Left or HitObject.DirectionEnum.Right:
                    Add(hitAnimation = HitAnimation(hit, YCalc(), XCalc()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(directionEnum), directionEnum, null);
            }
            Remove();
        }

        private Hit? GetHitResult(float value)
        {
            const float hit300Range = 0.45f;
            const float hit100Range = 0.35f;
            const float hit50Range = 0.25f;
            const float maxRange = hit50Range * 2 + 0.00001f;

            Hit? result = Direction switch
            {
                HitObject.DirectionEnum.Up => value switch
                {
                    <= -hit300Range and >= -maxRange => Hit.Hit300,
                    <= -hit100Range and >= -hit300Range => Hit.Hit100,
                    <= -hit50Range and >= -hit100Range => Hit.Hitx,
                    _ => null
                },
                HitObject.DirectionEnum.Left => value switch
                {
                    <= -hit300Range and >= -maxRange => Hit.Hit300,
                    <= -hit100Range when bx.Y >= -hit300Range => Hit.Hit100,
                    <= -hit50Range when bx.Y >= -hit100Range => Hit.Hitx,
                    _ => null
                },
                HitObject.DirectionEnum.Down => value switch
                {
                    >= hit300Range and <= maxRange => Hit.Hit300,
                    >= hit100Range and <= hit300Range => Hit.Hit100,
                    >= hit50Range and <= hit100Range => Hit.Hitx,
                    _ => null
                },
                HitObject.DirectionEnum.Right => value switch
                {
                    >= hit300Range and <= maxRange => Hit.Hit300,
                    >= hit100Range when bx.Y <= hit300Range => Hit.Hit100,
                    >= hit50Range when bx.Y <= hit100Range => Hit.Hitx,
                    _ => null
                },
                _ => null
            };

            return result;
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
