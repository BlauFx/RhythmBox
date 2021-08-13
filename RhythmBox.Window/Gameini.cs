using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace RhythmBox.Window
{
    public sealed class Gameini : IniConfigManager<SettingsConfig>
    {
        protected override string Filename => @"game.ini";

        protected override void InitialiseDefaults()
        {
            SetDefault(SettingsConfig.KeyBindingUp, "W");
            SetDefault(SettingsConfig.KeyBindingDown, "S");
            SetDefault(SettingsConfig.KeyBindingLeft, "A");
            SetDefault(SettingsConfig.KeyBindingRight, "D");
            SetDefault(SettingsConfig.Volume, 0.5d, 0d, 1d, 0.1d);
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
