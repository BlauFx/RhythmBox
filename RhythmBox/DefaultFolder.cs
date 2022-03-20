using System;
using System.Collections.Generic;
using RhythmBox.Window.Maps;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RhythmBox.Window
{
    public class DefaultFolder
    {
        private static readonly string TestMapFolder = $"{Songs.SongPath}TestMap";
        public static readonly string TestMapFile = $"{TestMapFolder}{Path.DirectorySeparatorChar}Difficulty1.ini";
        
        public DefaultFolder()
        {
            if (!Directory.Exists(Songs.SongPath))
                Directory.CreateDirectory(Songs.SongPath);

            //TODO: This is only temporary
            if (File.Exists(TestMapFile))
                return;
            
            GenerateMapFromResources(TestMapFile);
        }

        private void GenerateMapFromResources(string file)
        {
            Assembly assembly = Assembly.LoadFrom("RhythmBox.Resources.dll");
            
            if (assembly == null)
                throw new Exception($"{nameof(assembly)} can not be null!");

            var lines = ReadLines(() => assembly.GetManifestResourceStream("RhythmBox.Resources.Difficulty1.ini"), Encoding.UTF8).ToList();
            
            Directory.CreateDirectory(file);
            new Map(lines).WriteToNewMap(file);
        }

        //https://stackoverflow.com/a/13312954
        private IEnumerable<string> ReadLines(Func<Stream?> stream, Encoding encoding)
        {
            using var reader = new StreamReader(stream(), encoding);
            
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
    }
}
