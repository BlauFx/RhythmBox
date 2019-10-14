using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;

namespace RhythmBox.Window.Clocks
{
    public class RhythmBoxClockContainer : Container
    {
        private readonly IAdjustableClock adjustableClock;

        public readonly BindableBool IsPaused = new BindableBool();

        private readonly DecoupleableInterpolatingFramedClock decoupleableClock;

        private readonly double GamplayStartTime;

        public readonly Bindable<double> UserPlaybackRate = new BindableDouble(1) { Default = 1, MinValue = 0.1, MaxValue = 3, Precision = 0.1 };

        //TODO:
        //[Cached]
        public readonly RhythmBoxClock RhythmBoxClock;

        private readonly FramedOffsetClock userOffsetClock;

        private readonly FramedOffsetClock platformOffsetClock;

        private readonly BindableDouble pauseFreqAdjust = new BindableDouble(1);

        private const float pauseFreqDuration = 250;

        private double totalOffset => userOffsetClock.Offset + platformOffsetClock.Offset;

        public RhythmBoxClockContainer(double GamplayStartTime)
        {
            this.GamplayStartTime = GamplayStartTime;

            adjustableClock = new StopwatchClock();
            (adjustableClock as IAdjustableAudioComponent)?.AddAdjustment(AdjustableProperty.Frequency, pauseFreqAdjust);

            //TODO:
            decoupleableClock = new DecoupleableInterpolatingFramedClock { IsCoupled = true, AllowableErrorMilliseconds = 0 };

            decoupleableClock.ChangeSource(adjustableClock);

            //TODO:
            platformOffsetClock = new FramedOffsetClock(decoupleableClock) { Offset = 0 };

            userOffsetClock = new FramedOffsetClock(platformOffsetClock) { Offset = 0 };

            RhythmBoxClock = new RhythmBoxClock(userOffsetClock);

            RhythmBoxClock.IsPaused.BindTo(IsPaused);
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            UserPlaybackRate.ValueChanged += delegate
            {
                updateRate();
            };

            Seek(GamplayStartTime);
        }

        public void Start()
        {
            Seek(RhythmBoxClock.CurrentTime);
            decoupleableClock.Start();
            IsPaused.Value = false;

            this.TransformBindableTo(pauseFreqAdjust, 1, pauseFreqDuration, Easing.In);
        }

        public void Seek(double time)
        {
            decoupleableClock.Seek(time - totalOffset);
            userOffsetClock.ProcessFrame();
        }

        public void Stop()
        {
            this.TransformBindableTo(pauseFreqAdjust, 0, pauseFreqDuration, Easing.Out).OnComplete(x =>
            {
                decoupleableClock.Stop();
                adjustableClock.Stop();
            });

            IsPaused.Value = true;
        }

        protected override void Update()
        {
            if (!IsPaused.Value)
            {
                userOffsetClock.ProcessFrame();
            }

            base.Update();
        }

        private void updateRate()
        {
            if (adjustableClock == null)
            {
                return;
            }

            adjustableClock.ResetSpeedAdjustments();

            if (adjustableClock is IHasTempoAdjust tempo)
            {
                tempo.TempoAdjust = UserPlaybackRate.Value;
            }
            else
            {
                adjustableClock.Rate = UserPlaybackRate.Value;
            }
        }
    }
}
