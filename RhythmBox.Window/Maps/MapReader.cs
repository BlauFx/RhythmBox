using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RhythmBox.Window.Mode.Standard.Maps;

namespace RhythmBox.Window.Maps
{
    public class MapReader : IMap
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

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public HitObject[] HitObjects { get; set; }

        public string Path { get; set; }

        public MapReader(string path)
        {
            this.Path = path;

            List<string> x = new List<string>();

            using (StreamReader strm = new StreamReader(path))
                for (string str; (str = strm.ReadLine()) != null;)
                    x.Add(str);

            ReadFromList(x);
        }

        public MapReader(List<string> list) => ReadFromList(list);

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
    }
}
