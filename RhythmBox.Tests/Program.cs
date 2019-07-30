using osu.Framework;
using osu.Framework.Platform;

namespace RhythmBox.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using(GameHost host = Host.GetSuitableHost(@"RhythmBox", false, true))
            {
                using (Game game = new Tests())
                {
                    host.Run(game);
                }
            }
        }
    }
}
