using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using RhythmBox.Window.Interfaces;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Mods
{
    public class DummyMod2 : Mod, IApplyToHitobject
    {
        public override string Name => "DummyMod2";

        public override string SkinElement => "DummyMod2";

        public void ApplyToHitObj(HitBox obj)
        {
            obj.bx.Colour = Color4.Gray.Opacity(0.7f);
        }
    }
}
