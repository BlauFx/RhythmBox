using osu.Framework.Bindables;
using RhythmBox.Window.Animation;

namespace RhythmBox.Window.Score
{
    public static class Combo
    {
        public static Hit CurrentHit { get; private set; }

        public static BindableInt ComboInt { get; private set; } = new();

        public static void ResetCombo() => ComboInt.Value = 0;

        public static void UpdateCombo(Hit hit)
        {
            CurrentHit = hit;

            if (hit == Hit.Hitx)
                ComboInt.Value = 0;
            else
                ComboInt.Value++;

            Score.CalculateScore(ComboInt.Value, hit);
        }
    }
}
