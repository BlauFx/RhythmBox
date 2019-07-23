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

            //foreach (SectionData section in data.Sections)
            //{
            //    Logger.Log("[" + section.SectionName + "]");
            //    //Console.WriteLine("[" + section.Name + "]");

            //    //Iterate through all the keys in the current section
            //    //printing the values
            //    foreach (KeyData key in section.Keys)
            //        //Console.WriteLine(key.Name + " = " + key.Value);
            //    Logger.Log(key.KeyName + " = " + key.Value);
            //}

            string directValue = data["General"]["AFileName"];
            Logger.Log(directValue);
        }
    }
}

//INI TEMPLATE

//;v1

//[General]
//AFileName: null
//Name: null
//BPM= 0
//Objects= 0

//[HitObjects]
//;Direction, Time, Speed
//Direction.Up, 50, 1