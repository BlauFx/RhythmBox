using System.Linq;
using osu.Framework;
using osu.Framework.Platform;

namespace RhythmBox.Window
{
    class Program
    {
        static void Main(string[] args)
        {
            using GameHost host = Host.GetSuitableHost(@"RhythmBox", false, true);
            using Game game = args.Contains("--tests") ? new Tests() : new RhythmBoxWindow();
            host.Run(game);
        }
    }
}
