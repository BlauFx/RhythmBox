using osuTK.Graphics;
using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Mode.Std.Tests.Mods
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
