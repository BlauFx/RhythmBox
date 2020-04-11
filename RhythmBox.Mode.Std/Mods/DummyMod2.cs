using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public class DummyMod2 : Mod
    {
        public override string NAME => "DummyMod2";

        public override string SkinElement => "DummyMod2";

        public override void ApplyToHitObj(RBox obj)
        {
            obj.obj.bx.Colour = Color4.Gray.Opacity(0.7f);

            base.ApplyToHitObj(obj);
        }
    }
}
