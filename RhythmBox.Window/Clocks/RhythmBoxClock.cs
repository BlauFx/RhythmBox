using osu.Framework.Bindables;
using osu.Framework.Timing;

namespace RhythmBox.Window.Clocks
{
    public class RhythmBoxClock : IFrameBasedClock
    {
        private readonly IFrameBasedClock frameBasedClock;

        public readonly BindableBool IsPaused = new BindableBool();

        public RhythmBoxClock(IFrameBasedClock underlyingClock)
        {
            this.frameBasedClock = underlyingClock;
        }

        public double CurrentTime => frameBasedClock.CurrentTime;

        public double Rate => frameBasedClock.Rate;

        public bool IsRunning => frameBasedClock.IsRunning;

        public void ProcessFrame() { }

        public double ElapsedFrameTime => frameBasedClock.ElapsedFrameTime;

        public double FramesPerSecond => frameBasedClock.FramesPerSecond;

        public FrameTimeInfo TimeInfo => frameBasedClock.TimeInfo;
    }
}
