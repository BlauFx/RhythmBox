using System.Collections;
using RhythmBox.Mode.Std.Tests.Interfaces;

namespace RhythmBox.Mode.Std.Tests.Maps
{
    public class TestSceneMap : ITestSceneMap, IEnumerable
    {
        /// <summary>
        /// This is the filename of the audio file.
        /// </summary>
        public string AFileName { get; set; } = string.Empty;

        public string BGFile  { get; set; } = string.Empty;

        public int MapId  { get; set; } = 0; //TODO side note: MapId[]

        public int MapSetId  { get; set; } = 0; 

        public int BPM  { get; set; } = 0;

        public int Objects  { get; set; } = 0; //TODO might be unnecessary cuz we could do this => HitObjects.Length

        public bool AutoMap  { get; set; } = false;

        public GameMode Mode  { get; set; } = GameMode.STD;

        public string Title  { get; set; } = string.Empty;

        public string Artist  { get; set; } = string.Empty;

        public string Creator  { get; set; } = string.Empty;

        public string DifficultyName  { get; set; } = string.Empty;

        public HitObjects[] HitObjects { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Path { get; set; }

        public IEnumerator GetEnumerator()
        {
            return HitObjects.GetEnumerator();
        }
    }
}
