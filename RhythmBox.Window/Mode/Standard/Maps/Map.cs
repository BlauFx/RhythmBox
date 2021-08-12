using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

            this.Path = path;

            List<string> x = new List<string>();

            using (StreamReader strm = new StreamReader(path))
                for (string str; (str = strm.ReadLine()) != null;)
                    x.Add(str);

            ReadFromList(x);

            if (title != null) 
                this.Title = title;
        }

        public Map(List<string> list) => ReadFromList(list);

        private void ReadFromList(List<string> list)
        {
            var version = list.FirstOrDefault(y => y.Contains("v1", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(version) || string.IsNullOrEmpty(version))
                throw new Exception("Could not find version");

            if (version.Equals("v1", StringComparison.OrdinalIgnoreCase))
            {
                AFileName = SearchAndCut<string>(list, "AFileName");
                BGFile = SearchAndCut<string>(list, "BGFile");
                MapId =  SearchAndCut<int>(list, "MapId");
                MapSetId = SearchAndCut<int>(list, "MapSetId");
                BPM = SearchAndCut<int>(list, "BPM");
                Mode = EnumParser<GameMode>(SearchAndCut<string>(list, "Mode"));
                Title = SearchAndCut<string>(list, "Title");
                Artist = SearchAndCut<string>(list, "Artist");
                Creator = SearchAndCut<string>(list, "Creator");
                DifficultyName = SearchAndCut<string>(list, "DifficultyName");
                string timings = SearchAndCut<string>(list, "Timings");
                int num = timings.IndexOf(",", StringComparison.Ordinal);
                StartTime = int.Parse(timings[..num]);
                num++;
                EndTime =  int.Parse(timings[num..]);
                int index = list.FindIndex(str => str.Contains("HitObjects:", StringComparison.OrdinalIgnoreCase)) + 1;
                HitObjects = HitObjectsParser(list.GetRange(index, list.Count - index));
            }
        }

        private HitObject[] HitObjectsParser(IReadOnlyList<string> list)
        {
            var objs = new List<HitObject>();

            for (int i = 0; i < list.Count; i++)
            {
                var index = list[i].IndexOf(",", StringComparison.Ordinal);
                var lastindex = list[i].LastIndexOf(",", StringComparison.Ordinal);

                var time = double.Parse(list[i][(index + 2)..lastindex]);
                var speed = float.Parse(list[i][(lastindex + 2)..^1]);

                var dirIndex = list[i].IndexOf(".", StringComparison.Ordinal) + 1;
                var dirStr = list[i][dirIndex..index];

                var dir = EnumParser<HitObject.Direction>(dirStr);
                objs.Add(new HitObject(dir, time, speed));
            }

            return objs.ToArray();
        }

        private T SearchAndCut<T>(List<string> list, string term)
        {
            return (T)Convert.ChangeType(Cutter(list.FirstOrDefault(x => x.Contains(term, StringComparison.OrdinalIgnoreCase))), typeof(T));
        }

        private string Cutter(string cutThis)
        {
            if (!cutThis.Contains(":"))
                return cutThis;

            int x = cutThis.IndexOf(":", StringComparison.Ordinal) + 2;
            return cutThis[x..];
        }

        private T EnumParser<T>(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                return (T)Enum.Parse(typeof(T), obj, true);
            throw new NullReferenceException($"{obj} can not be null");
        }

        public void WriteToNewMap(string path)
        {
            this.Path = path;
            string folder = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));
            var folderPath = $"{Songs.SongPath}{folder}";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var list = new List<string>
            {
                "v1\n",
                $"AFileName: {AFileName}",
                $"BGFile: {BGFile}\n",
                $"MapId: {MapId.ToString()}",
                $"MapSetId: {MapSetId.ToString()}\n",
                $"BPM: {BPM.ToString()}\n",
                $"Mode: {Mode.ToString()}",
                $"Title: {Title}",
                $"Artist: {Artist}",
                $"Creator: {Creator}",
                $"DifficultyName: {DifficultyName}\n",
                $"Timings: {StartTime},{EndTime}\n",
                "HitObjects:",
            };

            WriteToFile(path, list);
            WriteToFile(path, HitObjects);
        }

        public void WriteToMap(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist!", path);

            var assemblyLocation = $@"{System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}{System.IO.Path.DirectorySeparatorChar}";

            if (!Directory.Exists($@"{assemblyLocation}SongsOLD"))
                Directory.CreateDirectory($"{assemblyLocation}SongsOLD");

            int num = path.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            string tmp = path.Substring(num, path.Length - num);
            int num1 = tmp.LastIndexOf(".", StringComparison.Ordinal);
            string filename = tmp.Substring(0, num1);

            int num2 = path.LastIndexOf("\\", StringComparison.Ordinal);
            string temp = path.Substring(0, num2);
            int num3 = temp.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            string str = temp.Substring(num3, temp.Length - num3);

            if (!Directory.Exists($@"{assemblyLocation}SongsOLD{System.IO.Path.DirectorySeparatorChar}{str}")) 
                Directory.CreateDirectory($@"{assemblyLocation}SongsOLD{System.IO.Path.DirectorySeparatorChar}{str}");

            File.Move(path, CheckIfFilenameIsAvailable($"{assemblyLocation}SongsOLD\\{str}\\{filename}.OLD"));
            WriteToNewMap(path);
            File.Delete($"{assemblyLocation}SongsOLD{System.IO.Path.DirectorySeparatorChar}{str}{System.IO.Path.DirectorySeparatorChar}{filename}.OLD");
            Directory.Delete($"{assemblyLocation}SongsOLD{System.IO.Path.DirectorySeparatorChar}{str}", true);
        }

        private string CheckIfFilenameIsAvailable(string originalPath)
        {
            if (File.Exists(originalPath))
            {
                int num = 1;
                string tmp = string.Empty;
                    for (int i = 0; i < num; i++)
                {
                    tmp = originalPath + num;
                    if (File.Exists(tmp))
                        num++;
                }
                return tmp;
            }
            return originalPath;
        }

        private void WriteToFile(string path, HitObject[] hitObjects)
        {
            using var streamWriter = new StreamWriter(path, true);

            foreach (var obj in hitObjects)
                streamWriter.WriteLine($"{obj._direction}, {obj.Time}, {obj.Speed}f");
        }

        private void WriteToFile(string path, IEnumerable<string> list)
        {
            using var strm = new StreamWriter(path, true);

            foreach (var @object in list)
                strm.WriteLine(@object);
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
