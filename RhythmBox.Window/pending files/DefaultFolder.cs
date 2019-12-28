using RhythmBox.Mode.Std.Interfaces;
using System.IO;
using System.Reflection;

namespace RhythmBox.Window.pending_files
{
    public class DefaultFolder
    {
        public DefaultFolder(bool CreateSongs = true, bool CreateSkins = false)
        {
            string CurrentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (CreateSongs)
            {
                if (!Directory.Exists(CurrentFolder + "\\Songs"))
                {
                    Directory.CreateDirectory(CurrentFolder + "\\Songs");
                }
            }
            else if (CreateSkins)
            {
                if (!Directory.Exists(CurrentFolder + "\\Skins"))
                {
                    Directory.CreateDirectory(CurrentFolder + "\\Skins");
                }
            }

            //TODO: This is only temporary
            //Side note: maybe add our own fileformat? 
            if (!File.Exists(CurrentFolder + "\\Songs\\TestMap\\Difficulty1.ini"))
            {
                TempMap(CurrentFolder + "\\Songs\\TestMap\\Difficulty1.ini");
            }
        }

        private void TempMap(string path)
        {
            var HitObjects = new HitObjects[4];

            HitObjects[0] = new HitObjects();
            HitObjects[1] = new HitObjects();
            HitObjects[2] = new HitObjects();
            HitObjects[3] = new HitObjects();

            HitObjects[0]._direction = Mode.Std.Interfaces.HitObjects.Direction.Up;
            HitObjects[1]._direction = Mode.Std.Interfaces.HitObjects.Direction.Right;
            HitObjects[2]._direction = Mode.Std.Interfaces.HitObjects.Direction.Left;
            HitObjects[3]._direction = Mode.Std.Interfaces.HitObjects.Direction.Down;

            HitObjects[0].Speed = 1f;
            HitObjects[1].Speed = 1f;
            HitObjects[2].Speed = 1f;
            HitObjects[3].Speed = 1f;

            HitObjects[0].Time = 200;
            HitObjects[1].Time = 400;
            HitObjects[2].Time = 700;
            HitObjects[3].Time = 780;

            var _map = new MapWriter
            {
                AFileName = "null.mp3",
                BGFile = "bg.png",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Objects = HitObjects.Length,
                AutoMap = false,
                Mode = GameMode.STD,
                Title = "TEST Title",
                Artist = "test",
                Creator = "BlauFx",
                DifficultyName = "BlauFx's diff",
                StartTime = 100,
                EndTime = 5000,
                HitObjects = HitObjects,
            };

            if (File.Exists(path))
            {
                _map.WriteToExistingMap(path);
            }
            else
            {
                _map.WriteToNewMap(path);
            }
        }
    }
}
