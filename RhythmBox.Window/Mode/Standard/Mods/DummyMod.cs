using osuTK.Graphics;
using RhythmBox.Window.Mode.Standard.Mods.Interfaces;
using RhythmBox.Window.Mode.Standard.Objects;

namespace RhythmBox.Window.Mode.Standard.Mods
{
    public class DummyMod : Mod, IApplyToHitobject
    {
        public override string NAME => "DummyMod";

        public override string SkinElement => "DummyMod";

        public void ApplyToHitObj(HitBox obj)
        {
            obj.bx.Colour = Color4.Red;
        }
    }
}
