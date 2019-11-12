using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace RhythmBox.Window.pending_files
{
    public class Gameini : IniConfigManager<SettingsConfig>
    {
        //TODO:
        protected override string Filename => @"game.ini";

        protected override void InitialiseDefaults()
        {
            Set(SettingsConfig.KeyBindingUp, "W");
            Set(SettingsConfig.KeyBindingDown, "S");
            Set(SettingsConfig.KeyBindingLeft, "A");
            Set(SettingsConfig.KeyBindingRight, "D");
        }

        public Gameini(Storage storage)
            : base(storage)
        {
        }
    }

    public enum SettingsConfig
    {
        KeyBindingUp,
        KeyBindingDown,
        KeyBindingLeft,
        KeyBindingRight,
    }
}
