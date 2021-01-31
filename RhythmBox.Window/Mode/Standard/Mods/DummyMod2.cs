﻿using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Mods.Interfaces;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public class DummyMod2 : Mod, IApplyToHitobject
    {
        public override string NAME => "DummyMod2";

        public override string SkinElement => "DummyMod2";

        public void ApplyToHitObj(HitBox obj)
        {
            obj.bx.Colour = Color4.Gray.Opacity(0.7f);
        }
    }
}