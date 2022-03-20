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

        public Box Bx;

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
            Child = Bx = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Size = new Vector2(0.1f, 0.01f),
                Alpha = 0,
                Depth = int.MinValue
            };

            Bx.FadeInFromZero(Duration * 0.2, Easing.None);
            var easing = Easing.InCirc;

            const float moveOffset = 0.5f;
            var size = new Vector2(0.01f, 0.1f);

            switch (Direction)
            {
                case HitObject.DirectionEnum.Up:
                    Bx.MoveToY(-moveOffset, Duration, easing);
                    break;
                case HitObject.DirectionEnum.Down:
                    Bx.Rotation = 180f;
                    Bx.MoveToY(moveOffset, Duration, easing);
                    break;
                case HitObject.DirectionEnum.Left:
                    Bx.Origin = Anchor.CentreLeft;
                    Bx.Size = size;
                    Bx.MoveToX(-moveOffset, Duration, easing);
                    break;
                case HitObject.DirectionEnum.Right:
                    Bx.Origin = Anchor.CentreRight;
                    Bx.Size = size;
                    Bx.MoveToX(moveOffset, Duration, easing);
                    break;
                default:
                    throw new Exception();
            }

            var resizeAmount = Direction switch
            {
                HitObject.DirectionEnum.Up or HitObject.DirectionEnum.Down => new Vector2(1f, 0.05f),
                HitObject.DirectionEnum.Left or HitObject.DirectionEnum.Right => new Vector2(0.056f, 1f),
                _ => throw new Exception()
            };

            Bx.ResizeTo(resizeAmount, Duration, easing);
            Scheduler.AddDelayed(Remove, Duration + Expire);
        }

        protected override void LoadComplete()
        {
            ApplyMods(Mods);
            base.LoadComplete();
        }

        private void ApplyMods(List<Mod> mods)
        {
            if (mods is null)
                return;

            foreach (var mod in mods)
            {
                if (mod is not IApplyToHitobject obj)
                    continue;

                obj.ApplyToHitObj(this);
            }
        }
        
        private async void Remove()
        {
            if (!alreadyRun) 
                alreadyRun = !alreadyRun;
            
            Scheduler.CancelDelayedTasks();

            Bx.Colour = Color4.Green;
            Scheduler.AddDelayed(() => Bx.Colour = Color4.White, this.Clear / 2);

            Bx.FadeOut(this.Clear);
            Bx.ScaleTo(1.1f, this.Clear, Easing.None);

            if (!clicked || hitAnimation == null)
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

            if ((key == keys[0] && Direction == HitObject.DirectionEnum.Up) || (key == keys[2] && Direction == HitObject.DirectionEnum.Down))
                ProcessClick(Bx.Y, Direction);
            else if ((key == keys[1] && Direction == HitObject.DirectionEnum.Left) || (key == keys[3] && Direction == HitObject.DirectionEnum.Right))
                ProcessClick(Bx.X, Direction);
        }

        private void ProcessClick(float value, HitObject.DirectionEnum directionEnum)
        {
            var hitResult = GetHitResult(value);
            if (hitResult == null)
                return;

            var hit = hitResult.GetValueOrDefault();

            float CalcPos()
            {
                if (hit != Hit.Hit300)
                    return float.NaN;

                const float offset = 0.025f;
                return directionEnum switch
                {
                    HitObject.DirectionEnum.Left => Bx.X + offset,
                    HitObject.DirectionEnum.Right => Bx.X - offset,
                    HitObject.DirectionEnum.Up => Bx.Y + offset,
                    HitObject.DirectionEnum.Down => Bx.Y - offset,
                    _ => float.NaN,
                };
            }

            Click(hit);
            switch (directionEnum)
            {
                case HitObject.DirectionEnum.Up or HitObject.DirectionEnum.Down:
                    Add(hitAnimation = HitAnimation(hit, CalcPos()));
                    break;
                case HitObject.DirectionEnum.Left or HitObject.DirectionEnum.Right:
                    Add(hitAnimation = HitAnimation(hit, Bx.Y, CalcPos()));
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
                HitObject.DirectionEnum.Up or HitObject.DirectionEnum.Left => value switch
                {
                    <= -hit300Range and >= -maxRange => Hit.Hit300,
                    <= -hit100Range and >= -hit300Range => Hit.Hit100,
                    <= -hit50Range and >= -hit100Range => Hit.Hitx,
                    _ => null
                },
                HitObject.DirectionEnum.Down or HitObject.DirectionEnum.Right => value switch
                {
                    >= hit300Range and <= maxRange => Hit.Hit300,
                    >= hit100Range and <= hit300Range => Hit.Hit100,
                    >= hit50Range and <= hit100Range => Hit.Hitx,
                    _ => null
                },
                _ => null
            };

            return result;
        }
        
        private void Click(Hit hit) => Combo.UpdateCombo(hit);

        private HitAnimation HitAnimation(Hit hit, float y = float.NaN, float x = float.NaN) 
            => new(hit)
            {
                Depth = float.MinValue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = float.IsNaN(x) ? Bx.X : x,
                Y = float.IsNaN(y) ? Bx.Y : y
            };
    }
}
