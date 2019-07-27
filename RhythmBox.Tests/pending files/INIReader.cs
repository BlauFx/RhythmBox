using System;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using osu.Framework.Logging;

namespace RhythmBox.Tests.pending_files
{
    public class INIReader
    {
        public INIReader()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(@"C:\Users\Oxygen\Desktop\CXD.ini");

            string directValue = data["General"]["AFileName"];
            Logger.Log(directValue);
        }
    }
}

//v1

//AFileName: null.mp3
//BGFile: bg.png

//MapId: 4
//MapSetId: 4

//BPM: 0
//Objects: 709

//AutoMap: yes

//Mode: STD
//Title: TEST Title
//Artist: test
//Creator: BlauFx
//DifficultyName: BlauFx's diff

//HitObjects:
//Up, 50, 1f
//Down, 500, 1.4f
//Direction.Left, 5200000000, 1.9f
//Direction.Right, 150, 1.7f