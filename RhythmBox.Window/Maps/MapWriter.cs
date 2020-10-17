using RhythmBox.Mode.Std.Maps;
using System.IO;
using System.Reflection;

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

        public HitObjects[] HitObjects { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Path { get; set; }

        public void WriteToNewMap(string path)
        {
            this.Path = path;
            
            string Folder = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));
            var FolderPath = Songs.SongPath + Folder;

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            WriteToFile(path, "v1", true);
            WriteToFile(path, "AFileName", AFileName);
            WriteToFile(path, "BGFile", BGFile, true);
            WriteToFile(path, "MapId", MapId.ToString());
            WriteToFile(path, "MapSetId", MapSetId.ToString(), true);
            WriteToFile(path, "BPM", BPM.ToString(), true);
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
                throw new FileNotFoundException("File does not exist!", path);

            if (!Directory.Exists(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\SongsOLD"))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\SongsOLD");

            int num = path.LastIndexOf("\\") + 1;
            string tmp = path.Substring(num, path.Length - num);
            int num1 = tmp.LastIndexOf(".");
            string filename = tmp.Substring(0, num1);

            int num2 = path.LastIndexOf("\\");
            string temp = path.Substring(0, num2);
            int num3 = temp.LastIndexOf("\\") + 1;
            string str = temp.Substring(num3, temp.Length - num3);

            if (!Directory.Exists(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $@"\SongsOLD\{str}"))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $@"\SongsOLD\{str}");
            }

            File.Move(path, CheckIfFilenameIsAvailable(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\SongsOLD\\{str}\\{filename}.OLD"));

            WriteToNewMap(path);
            File.Delete(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\SongsOLD\\{str}\\{filename}.OLD");
            Directory.Delete(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\SongsOLD\\{str}", true);
        }

        private string CheckIfFilenameIsAvailable(string orignalPath)
        {
            if (File.Exists(orignalPath))
            {
                int num = 1;
                string tmp = string.Empty;
                
                for (int i = 0; i < num; i++)
                {
                    tmp = orignalPath + num;
                    if (File.Exists(tmp))
                        num++;
                }
                return tmp;
            }
            return orignalPath;
        }

        private void WriteToFile(string path, HitObjects[] hitObjects)
        {
            using StreamWriter streamWriter = new StreamWriter(path, true);

            for (int i = 0; i < hitObjects.Length; i++)
                streamWriter.WriteLine($"{hitObjects[i]._direction}, {hitObjects[i].Time}, {hitObjects[i].Speed}f");
        }

        private void WriteToFile(string path, string leftside, string value, bool extraEmptyLine = false) 
            => Write(path, $"{leftside}: {value}", extraEmptyLine);

        private void WriteToFile(string path, string value, bool extraEmptyLine = false) 
            => Write(path, value, extraEmptyLine);

        private void Write(string path, string value, bool extraEmptyLine = false)
        {
            using var strm = new StreamWriter(path, true);
            strm.WriteLine(value);

            if (extraEmptyLine)
                strm.WriteLine(string.Empty);
        }
    }
}
