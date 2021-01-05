using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using RhythmBox.Mode.Std.Mods;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RhythmBox.Mode.Std.Mods.Interfaces;

namespace RhythmBox.Mode.Std.Animations
{
    public class HPBar : Container
    {
        private Box _box;

        public float Duration = 100f;

        public Easing easing = Easing.None;

        public BindableFloat CurrentValue { get; set; } = new BindableFloat(1f)
        {
            MinValue = 0,
            MaxValue = 1
        };

        public BindableBool Enabled { get; set; } = new BindableBool(true);

        public HPBar(List<Mod> mods = null)
        {
            if (mods != null)
            {
                var ModsToApply = mods.Where(x => x is IApplyToHP).ToList();

                for (int i = 0; i < ModsToApply.Count; i++)
                {
                    (ModsToApply[i] as IApplyToHP)?.ApplyToHP(this);
                }
            }
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = _box = new Box
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativePositionAxes = Axes.Both,
                Alpha = 1f,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.05f),
                X = 0f,
                Y = 0f,
            };
            
            CurrentValue.ValueChanged += (e) => _box.ResizeWidthTo(e.NewValue, Duration, easing);
        }

        public void Drain(bool Stop)
        {
            if (Stop)
            {
                Scheduler.CancelDelayedTasks();
                return;
            }
            
            Scheduler.AddDelayed(() =>
            {
                Schedule(() => CurrentValue.Value -= 0.01f);
            }, Duration, true);
        }
        
        public float CalcValue(Hit currenthit)
        {
            return currenthit switch
            {
                Hit.Hit300 => 0.1f,
                Hit.Hit100 => 0.03f,
                Hit.Hitx => -0.1f,
                _ => throw new NoNullAllowedException(),
            };
        }
    }
}
