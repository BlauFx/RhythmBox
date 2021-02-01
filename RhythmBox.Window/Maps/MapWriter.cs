using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RhythmBox.Window.Mode.Standard.Maps;

namespace RhythmBox.Window.Maps
{
    public class MapWriter : IMap
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

        public void WriteToExistingMap(string path)
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
}
