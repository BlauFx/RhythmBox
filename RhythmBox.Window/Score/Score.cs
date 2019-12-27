﻿using RhythmBox.Mode.Std.Animations;

namespace RhythmBox.Window.Score
{
    public static class Score
    {
        public static int ScoreInt { get; private set; }

        public static void ResetScore()
        {
            ScoreInt = 0;
        }

        public static void CalculateScore(int combo, Hit currentHit)
        {
            int addAmout = currentHit switch
            {
                Hit.Hit300 => 300,
                Hit.Hit100 => 100,
                Hit.Hit50 => 50,
                Hit.Hitx => 0,
                _ => 0,
            };

            var CalcScore = (combo * addAmout);
            ScoreInt += CalcScore;
        }
    }
}
