using osuTK.Audio;
using RhythmBox.Mode.Std.Tests.Interfaces;
using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Mode.Std.Tests.Maps
{
    public class TestSceneMap : IMap
    {
        public int Length { get; set; }
        public int maxCombo { get; set; }
        public int maxObjects { get; set; }
        public float[] Timings { get; set; }
        public int ID { get; set; }
        public Direction Direction { get; set; }

        public TestSceneMap(int length, int maxCombo, int maxObjects, float[] timings, int ID, Direction direction)
        {
            this.Length = length;
            this.maxCombo = maxCombo;
            this.maxObjects = maxObjects;
            this.Timings = timings;
            this.ID = ID;
            this.Direction = direction;
        }

        private void Load()
        {
            
        }
    }
}
