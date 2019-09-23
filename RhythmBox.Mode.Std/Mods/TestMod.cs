using osuTK.Graphics;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public class TestMod : Mod
    {
        public override string NAME => "TEST-NAME";

        public override void AppyToHitObj(RBox obj)
        {
            obj.obj.bx.Colour = Color4.Red;

            base.AppyToHitObj(obj);
        }
    }
}
