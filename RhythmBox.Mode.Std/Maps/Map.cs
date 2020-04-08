using RhythmBox.Mode.Std.Interfaces;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace RhythmBox.Mode.Std.Maps
{
    public class Map : IMap, IEnumerable
    {
        public string AFileName { get; set; } = string.Empty;

        public string BGFile { get; set; } = string.Empty;

        public int MapId { get; set; } = 0; //TODO side note: MapId[]

        public int MapSetId { get; set; } = 0;

        public int BPM { get; set; } = 0;

        public GameMode Mode { get; set; } = GameMode.STD;

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string Creator { get; set; } = string.Empty;

        public string DifficultyName { get; set; } = string.Empty;

        public HitObjects[] HitObjects { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Path { get; set; }

        public IEnumerator GetEnumerator() => HitObjects.GetEnumerator();

        private object instantiatedType;

        public Map(string path, string title = null)
        {
            if (path == null) return;

            var assembly = Assembly.LoadFrom("RhythmBox.Window.dll");
            var classes = assembly.GetTypes().Where(p => p.Namespace == "RhythmBox.Window.Maps" && p.Name.Contains("MapReader"));

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            instantiatedType = Activator.CreateInstance(classes.FirstOrDefault(), flags, null, new[] { $"{path}" }, null);

            AFileName = GetValue(0).ToString();
            BGFile = GetValue(1).ToString();
            MapId = (int)GetValue(2);
            MapSetId = (int)GetValue(3);
            BPM = (int)GetValue(4);
            Mode = (GameMode)GetValue(5);
            Title = Title != null ? title : GetValue(6).ToString();
            Artist = GetValue(7).ToString();
            Creator = GetValue(8).ToString();
            DifficultyName = GetValue(9).ToString();
            StartTime = (int)GetValue(10);
            EndTime = (int)GetValue(11);
            HitObjects = (HitObjects[])GetValue(12);
            Path = GetValue(13).ToString();
        }

        private object GetValue(int i) => instantiatedType.GetType().GetProperties()[i].GetValue(instantiatedType, null);
    }
}
