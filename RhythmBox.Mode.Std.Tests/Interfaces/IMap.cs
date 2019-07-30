using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Mode.Std.Tests.Interfaces
{
    interface IMap
    {
        int Length { get; set; }

        int maxCombo { get; set; }

        int maxObjects { get; set; }

        float[] Timings { get; set; }

        int ID { get; set; }

        Direction Direction { get; set; }
    }
}
