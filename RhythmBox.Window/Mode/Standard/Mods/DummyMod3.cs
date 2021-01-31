using RhythmBox.Mode.Std.Animations;
using RhythmBox.Mode.Std.Mods.Interfaces;

namespace RhythmBox.Mode.Std.Mods
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
