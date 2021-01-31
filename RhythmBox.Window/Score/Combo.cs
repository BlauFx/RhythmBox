using osu.Framework.Bindables;
using RhythmBox.Window.Mode.Standard.Animations;

namespace RhythmBox.Window.Score
{
    public static class Combo
    {
        public static BindableInt PrivateComboBindable = new BindableInt(0);

        public static Hit currentHit { get; private set; }

        public static int ComboInt { get; private set; }

        public static void ResetCombo()
        {
            ComboInt = 0;
            PrivateComboBindable.Value = ComboInt;
        }

        public static void UpdateCombo(Hit hit)
        {
            currentHit = hit;

            if (hit == Hit.Hitx)
                ComboInt = 0;
            else
                ComboInt++;

            PrivateComboBindable.Value = ComboInt;
            Score.CalculateScore(ComboInt, hit);
        }
    }
}
