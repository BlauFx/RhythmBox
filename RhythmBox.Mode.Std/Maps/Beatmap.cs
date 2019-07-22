using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Maps
{
    public class Beatmap
    {
        private int length { get; set; } = 0;

        private int maxCombo { get; set; } = 0;

        private int maxObjects { get; set; } = 0;

        private float[] timings = new float[0];

        private Direction direction { get; set; }
    }
}
