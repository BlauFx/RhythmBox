using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.Maps;
using System.IO;
using System.Reflection;
using HitObjects = RhythmBox.Mode.Std.Maps.HitObjects;

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
            var hitObjects = new HitObjects[4];

            hitObjects[0] = new HitObjects();
            hitObjects[1] = new HitObjects();
            hitObjects[2] = new HitObjects();
            hitObjects[3] = new HitObjects();

            hitObjects[0]._direction = HitObjects.Direction.Up;
            hitObjects[1]._direction = HitObjects.Direction.Right;
            hitObjects[2]._direction = HitObjects.Direction.Left;
            hitObjects[3]._direction = HitObjects.Direction.Down;

            hitObjects[0].Speed = 1f;
            hitObjects[1].Speed = 1f;
            hitObjects[2].Speed = 1f;
            hitObjects[3].Speed = 1f;

            hitObjects[0].Time = 200;
            hitObjects[1].Time = 400;
            hitObjects[2].Time = 700;
            hitObjects[3].Time = 780;

            var _map = new MapWriter
            {
                AFileName = "null.mp3",
                BGFile = "bg.png",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Mode = GameMode.STD,
                Title = "TEST Title",
                Artist = "test",
                Creator = "BlauFx",
                DifficultyName = "BlauFx's diff",
                StartTime = 100,
                EndTime = 5000,
                HitObjects = hitObjects,
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
