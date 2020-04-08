using RhythmBox.Mode.Std.Interfaces;
using System;
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

        private int startHitObjects = 0;

        private string[] storageTemp;

        public MapReader(string path)
        {
            this.Path = path;

            int lineCount = File.ReadLines(path).Count();

            storageTemp = new string[lineCount];

            using (StreamReader strm = new StreamReader(path))
            {
                for (int i = 0; i < lineCount; i++)
                {
                    string input = strm.ReadLine();
                    storageTemp[i] = input;

                    if (input != null && input.Contains("HitObjects"))
                    {
                        startHitObjects = i + 1;
                    }
                }
            }

            if (SearchThis(storageTemp, "v1") == "v1")
            {
                AFileName = SearchThis(storageTemp, "AFileName");
                BGFile = SearchThis(storageTemp, "BGFile");
                MapId = int.Parse(SearchThis(storageTemp, "MapId"));
                MapSetId = int.Parse(SearchThis(storageTemp, "MapSetId"));
                BPM = int.Parse(SearchThis(storageTemp, "BPM"));
                Mode = GameModeParser(SearchThis(storageTemp, "Mode"));
                Title = SearchThis(storageTemp, "Title");
                Artist = SearchThis(storageTemp, "Artist");
                Creator = SearchThis(storageTemp, "Creator");
                DifficultyName = SearchThis(storageTemp, "DifficultyName");
                StartTime = TimeCutter(true);
                EndTime = TimeCutter(false);
                HitObjects = HitObjectsParser(HitObjects, path);
            }
        }

        private string SearchThis(string[] storage, string searchStr)
        {
            int num = 0;

            for (int i = 0; i < storage.Length; i++)
            {
                if (storage[i].Contains(searchStr))
                {
                    num = i;
                    break;
                }
            }

            return Cutter(storage[num]);
        }

        private string Cutter(string cutThis)
        {
            if (!cutThis.Contains(":"))
            {
                return cutThis;
            }
            int x = cutThis.IndexOf(":", StringComparison.Ordinal) + 2;
            return cutThis.Substring(x, cutThis.Length - x);
        }

        private GameMode GameModeParser(string parse)
        {
            if (parse != null)
                return (GameMode)Enum.Parse(typeof(GameMode), parse, true);
            throw new NullReferenceException("GameMode can not be null");
        }

        private int TimeCutter(bool Start = false)
        {
            string x = SearchThis(storageTemp, "Timings");

            if (Start)
            {
                int num = x.IndexOf(",");
                return int.Parse(x.Substring(0, num));
            }
            else
            {
                int num = x.IndexOf(",") + 1;
                return int.Parse(x.Substring(num, x.Length - num));
            }
        }

        private HitObjects[] HitObjectsParser(HitObjects[] obj, string path)
        {
            int lineCount = File.ReadLines(path).Count();

            int counter = 0;

            string[] storageTmp = new string[lineCount - startHitObjects];

            obj = new HitObjects[storageTmp.Length];

            for (int i = startHitObjects; i < lineCount; i++)
            {
                storageTmp[counter] = storageTemp[i];
                counter++;
            }

            for (int i = 0; i < storageTmp.Length; i++)
            {
                obj[i] = new HitObjects();

                var dir1_1 = storageTmp[i].IndexOf(",", StringComparison.Ordinal);
                var dir1_2_Dir = storageTmp[i].Substring(0, dir1_1);

                var dir2_1 = storageTmp[i].IndexOf(",", StringComparison.Ordinal) + 2;
                var dir2_2 = storageTmp[i].LastIndexOf(",", StringComparison.Ordinal) + 0;
                var dir2_3_Time = storageTmp[i].Substring(dir2_1, dir2_2 - dir2_1);

                var dir3_1 = storageTmp[i].LastIndexOf(",", StringComparison.Ordinal) + 2;
                var dir3_2_Speed = storageTmp[i].Substring(dir3_1, storageTmp[i].Length - (dir3_1 + 1));

                obj[i]._direction = parseDirection(dir1_2_Dir.Substring(dir1_2_Dir.IndexOf(".")+1, dir1_2_Dir.Length - dir1_2_Dir.IndexOf(".")-1));
                obj[i].Time = double.Parse(dir2_3_Time);
                obj[i].Speed = float.Parse(dir3_2_Speed);
            }

            return obj;

        }

        private HitObjects.Direction parseDirection(string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                throw new NullReferenceException("Direction can not be null");
            }
            
            return (HitObjects.Direction)Enum.Parse(typeof(HitObjects.Direction), direction, true); ;
        }
    }
}
