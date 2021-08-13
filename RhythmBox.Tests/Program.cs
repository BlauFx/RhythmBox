using osu.Framework;
using osu.Framework.Platform;
using System;

namespace RhythmBox.Tests
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using GameHost host = Host.GetSuitableHost(@"RhythmBox", false, true);
            using Game game = new Tests();
            host.Run(game);
        }
    }
}
