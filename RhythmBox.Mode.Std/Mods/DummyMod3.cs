﻿using RhythmBox.Mode.Std.Animations;

namespace RhythmBox.Mode.Std.Mods
{
    public class DummyMod3 : Mod, IApplyToHP
    {
        public override string NAME => "Disable HPBar";

        public override string SkinElement => "DummyMod3";

        public void ApplyToHP(HpBar hp)
        {
            hp.HPBarEnabled = false;
        }
    }
}