﻿using RhythmBox.Mode.Std.Interfaces;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.pending_files
{
    public class MapWriter : IMap
    {
        public string AFileName { get; set; } = string.Empty;

        public string BGFile { get; set; } = string.Empty;

        public int MapId { get; set; } = 0;

        public int MapSetId { get; set; } = 0;

        public int BPM { get; set; } = 0;

        public int Objects { get; set; } = 0;

        public bool AutoMap { get; set; } = false;

        public GameMode Mode { get; set; } = GameMode.STD;

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string Creator { get; set; } = string.Empty;

        public string DifficultyName { get; set; } = string.Empty;

        public HitObjects[] HitObjects { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public void WriteToNewMap(string path)
        {
            WriteToFile(path, "v1", true);
            WriteToFile(path, "AFileName", AFileName);
            WriteToFile(path, "BGFile", BGFile, true);
            WriteToFile(path, "MapId", MapId.ToString());
            WriteToFile(path, "MapSetId", MapSetId.ToString(), true);
            WriteToFile(path, "BPM", BPM.ToString(), true);
            WriteToFile(path, "Objects", Objects.ToString(), true);
            WriteToFile(path, "AutoMap", AutoMap.ToString(), true);
            WriteToFile(path, "Mode", Mode.ToString());
            WriteToFile(path, "Title", Title);
            WriteToFile(path, "Artist", Artist);
            WriteToFile(path, "Creator", Creator);
            WriteToFile(path, "DifficultyName", DifficultyName, true);
            WriteToFile(path, "Timings", $"{StartTime},{EndTime}", true);

            WriteToFile(path, "HitObjects:");
            WriteToFile(path, HitObjects);
        }

        public void WriteToExistingMap(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File does not exist!", path);
            }
            if (!Directory.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\SongsOLD"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\SongsOLD");
            }

            int num = path.LastIndexOf("\\")+1;
            string filename = path.Substring(num, path.Length - num);

            File.Move(path, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\SongsOLD\\{filename}.OLD");

            WriteToNewMap(path);

            File.Delete(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\SongsOLD\\{filename}.OLD");
        }

        private void WriteToFile(string path, HitObjects[] hitObjects)
        {
            for (int i = 0; i < hitObjects.Length; i++)
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine($"Direction.{hitObjects[i]._direction}, {hitObjects[i].Time}, {hitObjects[i].Speed}f");
                }
            }
        }


        private void WriteToFile(string path, string leftside, string value, bool extraEmptyLine = false)
        {
            using (StreamWriter strm = new StreamWriter(path, true))
            {
                strm.WriteLine($"{leftside}: {value}");
                if (extraEmptyLine)
                {
                    strm.WriteLine(string.Empty);
                }
            }
        }

        private void WriteToFile(string path, string value, bool extraEmptyLine = false)
        {
            using (StreamWriter strm = new StreamWriter(path, true))
            {
                strm.WriteLine(value);
                if (extraEmptyLine)
                {
                    strm.WriteLine(string.Empty);
                }
            }
        }
    }
}