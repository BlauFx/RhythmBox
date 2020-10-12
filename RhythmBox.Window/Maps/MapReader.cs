using RhythmBox.Mode.Std.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public HitObjects[] HitObjects { get; set; }

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

        public void ReadFromList(List<string> list)
        {
            string version = list.FirstOrDefault(y => y.Contains("v1", StringComparison.OrdinalIgnoreCase));
            
            if (string.IsNullOrWhiteSpace(version) || string.IsNullOrEmpty(version))
                throw new Exception("Could not find version");

            if (version.Equals("v1", StringComparison.OrdinalIgnoreCase))
            {
                AFileName = Cutter(list.FirstOrDefault(x => x.Contains("AFileName", StringComparison.OrdinalIgnoreCase)));
                BGFile = Cutter(list.FirstOrDefault(x => x.Contains("BGFile", StringComparison.OrdinalIgnoreCase)));
                MapId =  int.Parse(Cutter(list.FirstOrDefault(x => x.Contains("MapId", StringComparison.OrdinalIgnoreCase))));
                MapSetId = int.Parse(Cutter(list.FirstOrDefault(x => x.Contains("MapSetId", StringComparison.OrdinalIgnoreCase))));
            
                BPM = int.Parse(Cutter(list.FirstOrDefault(x => x.Contains("BPM", StringComparison.OrdinalIgnoreCase))));
                Mode = EnumParser<GameMode>(Cutter(list.FirstOrDefault(x => x.Contains("Mode", StringComparison.OrdinalIgnoreCase))));
                Title = Cutter(list.FirstOrDefault(x => x.Contains("Title", StringComparison.OrdinalIgnoreCase)));
                Artist = Cutter(list.FirstOrDefault(x => x.Contains("Artist", StringComparison.OrdinalIgnoreCase)));
                Creator = Cutter(list.FirstOrDefault(x => x.Contains("Creator", StringComparison.OrdinalIgnoreCase)));
                DifficultyName = Cutter(list.FirstOrDefault(x => x.Contains("DifficultyName", StringComparison.OrdinalIgnoreCase)));
            
                string Timings = Cutter(list.FirstOrDefault(x => x.Contains("Timings", StringComparison.OrdinalIgnoreCase)));
                int num = Timings.IndexOf(",", StringComparison.Ordinal);
            
                StartTime = int.Parse(Timings[0..num]);
                num++;
                EndTime =  int.Parse(Timings[num..]);
            
                int index = list.FindIndex(str => str.Contains("HitObjects:", StringComparison.OrdinalIgnoreCase)) + 1;
                HitObjects = HitObjectsParser(list.GetRange(index, list.Count - index));
            }
        }
        
        private HitObjects[] HitObjectsParser(List<string> list)
        {
            List<HitObjects> objs = new List<HitObjects>();

            for (int i = 0; i < list.Count; i++)
            {
                var index = list[i].IndexOf(",", StringComparison.Ordinal);
                var lastindex = list[i].LastIndexOf(",", StringComparison.Ordinal);

                var time = double.Parse(list[i][(index + 2)..lastindex]);
                var speed = float.Parse(list[i][(lastindex + 2)..^1]);

                var dirStr = list[i][..index];
                var dir = EnumParser<HitObjects.Direction>(dirStr[(dirStr.IndexOf(".", StringComparison.Ordinal) + 1)..]);

                objs.Add(new HitObjects
                    {
                        Time = time,
                        Speed = speed,
                        _direction = dir
                    }
                );
            }

            return objs.ToArray();
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
