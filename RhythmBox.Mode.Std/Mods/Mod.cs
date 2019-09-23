using RhythmBox.Mode.Std.Objects;

namespace RhythmBox.Mode.Std.Mods
{
    public abstract class Mod
    {
        public abstract string NAME { get; }

        public virtual void AppyToHitObj(RBox obj)
        {
        }
    }
}
