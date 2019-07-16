using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace RhythmBox.Window.pending_files
{
    public class Gameini : IniConfigManager<SettingsConfig>
    {
        protected override string Filename => @"game.ini";

        protected override void InitialiseDefaults()
        {

        }

        public Gameini(Storage storage)
            : base(storage)
        {
        }
    }
    public enum SettingsConfig
    {

    }
}
