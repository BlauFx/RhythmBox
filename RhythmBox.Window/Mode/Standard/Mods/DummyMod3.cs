using RhythmBox.Window.Mode.Standard.Animations;
using RhythmBox.Window.Mode.Standard.Mods.Interfaces;

namespace RhythmBox.Window.Mode.Standard.Mods
{
    public class DummyMod3 : Mod, IApplyToHP
    {
        public override string NAME => "Disable HPBar";

        public override string SkinElement => "DummyMod3";

        public void ApplyToHP(HPBar hp)
        {
            hp.Enabled.Value = false;
        }
    }
}
