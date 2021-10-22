using osuTK.Graphics;
using RhythmBox.Window.Interfaces;
using RhythmBox.Window.Objects;

namespace RhythmBox.Window.Mods
{
    public class DummyMod : Mod, IApplyToHitobject
    {
        public override string Name => "DummyMod";

        public override string SkinElement => "DummyMod";

        public void ApplyToHitObj(HitBox obj)
        {
            obj.Bx.Colour = Color4.Red;
        }
    }
}
