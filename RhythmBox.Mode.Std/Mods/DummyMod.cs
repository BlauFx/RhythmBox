using osuTK.Graphics;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public class DummyMod : Mod
    {
        public override string NAME => "DummyMod";

        public override string SkinElement => "DummyMod";

        public override void AppyToHitObj(RBox obj)
        {
            obj.obj.bx.Colour = Color4.Red;

            base.AppyToHitObj(obj);
        }
    }
}
