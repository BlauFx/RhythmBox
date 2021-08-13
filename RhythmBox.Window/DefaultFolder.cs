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
        public DefaultFolder(bool createSongs = true, bool createSkins = false)
        {
            string currentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            string skinsFolder = $"{currentFolder}{Path.DirectorySeparatorChar}Skins";
            
            if (createSongs && !Directory.Exists(Songs.SongPath))
                Directory.CreateDirectory(Songs.SongPath);
            else if (createSkins && !Directory.Exists(skinsFolder))
                Directory.CreateDirectory(skinsFolder);

            var file = $"{Songs.SongPath}{Path.DirectorySeparatorChar}TestMap{Path.DirectorySeparatorChar}Difficulty1.ini";
            
            //TODO: This is only temporary
            //Side note: maybe add our own fileformat? 
            if (!File.Exists(file))
            {
                Directory.CreateDirectory($"{Songs.SongPath}{Path.DirectorySeparatorChar}TestMap");
                ImprovedTemp(file);
            }
        }

        private void ImprovedTemp(string file)
        {
            Assembly assembly = Assembly.LoadFrom("RhythmBox.Window.Resources.dll");
            
            if (assembly == null)
                throw new Exception($"{nameof(assembly)} can not be null!");

            var lines = ReadLines(() => assembly.GetManifestResourceStream("RhythmBox.Window.Resources.Difficulty1.ini")!, Encoding.UTF8).ToList();

            var map = new Map(lines);
            map.WriteToNewMap(file);
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
