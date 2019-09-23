using osuTK.Graphics;
using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Mode.Std.Tests.Mods
{
    public class TestMod : Mod
    {
        public override string NAME => "TEST-NAME";

        public override void AppyToHitObj(TestSceneRBox obj)
        {
            obj.obj.bx.Colour = Color4.Red;

            base.AppyToHitObj(obj);
        }
    }
}
