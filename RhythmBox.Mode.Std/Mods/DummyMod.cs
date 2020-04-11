using osuTK.Graphics;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public class DummyMod : Mod, IApplyToHitobject
    {
        public override string NAME => "DummyMod";

        public override string SkinElement => "DummyMod";

        public void ApplyToHitObj(RBox obj)
        {
            obj.obj.bx.Colour = Color4.Red;
        }
    }
}
