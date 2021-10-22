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
        public DefaultFolder()
        {
            if (!Directory.Exists(Songs.SongPath))
                Directory.CreateDirectory(Songs.SongPath);

            var testMapFolder = $"{Songs.SongPath}{Path.DirectorySeparatorChar}TestMap";
            var testMapFile = $"{testMapFolder}{Path.DirectorySeparatorChar}Difficulty1.ini";
            
            //TODO: This is only temporary
            if (File.Exists(testMapFile))
                return;
            
            GenerateMapFromResources(testMapFile);
        }

        private void GenerateMapFromResources(string file)
        {
            Assembly assembly = Assembly.LoadFrom("RhythmBox.Window.Resources.dll");
            
            if (assembly == null)
                throw new Exception($"{nameof(assembly)} can not be null!");

            var lines = ReadLines(() => assembly.GetManifestResourceStream("RhythmBox.Window.Resources.Difficulty1.ini"), Encoding.UTF8).ToList();
            
            Directory.CreateDirectory(file);
            new Map(lines).WriteToNewMap(file);
        }

        //https://stackoverflow.com/a/13312954
        private IEnumerable<string> ReadLines(Func<Stream> streamProvider, Encoding encoding)
        {
            using var stream = streamProvider();
            using var reader = new StreamReader(stream, encoding);
            
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
    }
}
