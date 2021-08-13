using RhythmBox.Window.Animation;
using RhythmBox.Window.Interfaces;

namespace RhythmBox.Window.Mods
{
    public class DummyMod3 : Mod, IApplyToHP
    {
        public override string Name => "Disable HPBar";

        public override string SkinElement => "DummyMod3";

        public void ApplyToHP(HPBar hp)
        {
            hp.Enabled.Value = false;
        }
    }
}
