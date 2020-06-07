using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Mods;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Utils;

namespace RhythmBox.Mode.Std.Animations
{
    public class HpBar : Container
    {
        private Box _box;

        public readonly float BoxMaxValue;

        public BindableFloat CurrentValue { get; set; } = new BindableFloat(0f);

        public Color4 colour;

        private const float HP_300 = 0.01f;

        private const float HP_100 = 0.005f;

        private const float HP_50 = 0.0025f;

        private const float HP_X = 0.1f;

        public bool HPBarEnabled = true;

        public HpBar(float BoxMaxValue, List<Mod> mods = null)
        {
            this.BoxMaxValue = BoxMaxValue;
            this.CurrentValue.Value = BoxMaxValue;

            if (mods != null)
            {
                var ModsToApply = mods.Where(x => x is IApplyToHP).ToList();

                for (int i = 0; i < ModsToApply.Count; i++)
                {
                    (ModsToApply[i] as IApplyToHP).ApplyToHP(this);
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
                Colour = colour,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(BoxMaxValue, 0.05f),
                X = 0f,
                Y = 0f,
            };
        }

        public void ResizeBox(float value, double duration, Easing easing)
        {
            if ((value > BoxMaxValue || value < -0.0001f) || !HPBarEnabled)
                return;

            lock (CurrentValue)
            {
                lock (_box)
                {
                    CurrentValue.Value = value;
                    _box.ResizeWidthTo(value, duration, easing);
                }
            }
        }

        public float CalcHpBarValue(float currentvalue, float maxvalue, float minvalue, Hit hit, bool auto = false)
        {
            DrainHP();
            return 1f;
            if (!auto)
            {
                float result = hit switch
                {
                    Hit.Hit300 => currentvalue + HP_300,
                    Hit.Hit100 => currentvalue + HP_100,
                    Hit.Hit50 => currentvalue + HP_50,
                    Hit.Hitx => currentvalue - HP_X * 10,
                    _ => 0
                };

                if (result < maxvalue && result > minvalue)
                    return result;
                else if (result > maxvalue)
                    return maxvalue;
                else if (result < minvalue)
                    return minvalue;
            }

            //TODO: HP_Drain has a different value if BoxMaxValue has as well a different value
            //TODO: There is a huge diffrence between BoxMaxValue .1f and 1f.
            //TODO: This is definitly a huge game breaker
            float HP_Drain = -0.0001f;
            
            return currentvalue - HP_Drain;
        }
        
        public void DrainHP()
        {
            Scheduler.AddDelayed(() =>
            {
                var calc = BoxMaxValue * 1 / 500;

                lock (_box)
                {
                    lock (CurrentValue)
                    {
                        _box.ResizeWidthTo(CurrentValue.Value - calc, 50, Easing.InExpo); 
                        CurrentValue.Value = _box.Width;
                    }
                }
            }, 200, true);
        }
    }
}
