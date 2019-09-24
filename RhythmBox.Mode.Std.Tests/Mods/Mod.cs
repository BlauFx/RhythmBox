using RhythmBox.Mode.Std.Tests.Objects;

namespace RhythmBox.Mode.Std.Tests.Mods
{
    public abstract class Mod
    {
        public abstract string NAME { get; }

        public virtual void AppyToHitObj(RBox obj)
        {
        }
    }
}
