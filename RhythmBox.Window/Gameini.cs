using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace RhythmBox.Window
{
    public class Gameini : IniConfigManager<SettingsConfig>
    {
        protected override string Filename => @"game.ini";

        protected override void InitialiseDefaults()
        {
            Set(SettingsConfig.KeyBindingUp, "W");
            Set(SettingsConfig.KeyBindingDown, "S");
            Set(SettingsConfig.KeyBindingLeft, "A");
            Set(SettingsConfig.KeyBindingRight, "D");
            Set(SettingsConfig.Volume, 0.5d, 0d, 1d, 0.25d);
        }

        public Gameini(Storage storage)
            : base(storage)
        {
            PerformSave();
        }
    }

    public enum SettingsConfig
    {
        KeyBindingUp,
        KeyBindingLeft,
        KeyBindingDown,
        KeyBindingRight,
        Volume
    }
}
