using osu.Framework;
using osu.Framework.Platform;
using RhythmBox.Window;

namespace RhythmBox.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            using GameHost host = Host.GetSuitableHost(@"RhythmBox", false, true);
            using Game game = new RhythmBoxWindow();
            host.Run(game);
        }
    }
}
