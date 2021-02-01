using System.Collections;
using RhythmBox.Window.Maps;

namespace RhythmBox.Window.Mode.Standard.Maps
{
    public class Map : IMap, IEnumerable
    {
        public string AFileName { get; set; } = string.Empty;

        public string BGFile { get; set; } = string.Empty;

        public int MapId { get; set; } = 0;

        public int MapSetId { get; set; } = 0;

        public int BPM { get; set; } = 0;

        public GameMode Mode { get; set; } = GameMode.STD;

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string Creator { get; set; } = string.Empty;

        public string DifficultyName { get; set; } = string.Empty;

        public HitObject[] HitObjects { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Path { get; set; }

        public IEnumerator GetEnumerator() => HitObjects.GetEnumerator();

        public Map(string path, string title = null)
        {
            if (path == null) return;

            new MapReader(path).CopyAllTo<IMap>(this);

            if (title != null) 
                this.Title = title;
        }
    }

    //https://stackoverflow.com/a/36713403
    public static class ext
    {
        public static void CopyAllTo<T>(this T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty?.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
