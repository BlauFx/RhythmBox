using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace RhythmBox.Window.Animation
{
    public abstract class MusicVisualization : Container
    {
        public bool AudioHasLoaded { get; set; } = false;

        private readonly IBindable<Track> bindableTrack = null;

        protected virtual VisualBox ConstructVisualBox() => new VisualBox();

        private float barWidth;

        public float BarWidth
        {
            get => barWidth;
            set
            {
                barWidth = value;

                if (!IsLoaded)
                    return;

                foreach (var Bar in Bars)
                {
                    Bar.Width = value;
                }
            }
        }

        private int amountOfBars;

        public int AmountOfBars
        {
            get => amountOfBars;
            set
            {
                amountOfBars = value;

                if (!IsLoaded)
                    return;

                RestoreBars();
            }
        }

        private bool isReversed;

        public bool IsReversed
        {
            get => isReversed;
            set
            {
                isReversed = value;

                RestoreBars();
            }
        }

        public float Intensivity { get; set; } = 400;

        private readonly int Duration;

        protected VisualBox[] Bars;

        protected MusicVisualization(IBindable<Track> bindableTrack, int Duration = 200)
        {
            if (bindableTrack != null)
                AudioHasLoaded = true;

            this.bindableTrack = bindableTrack;
            this.Duration = Duration;
        }

        public void RestoreBars()
        {
            Clear(true);

            Bars = new VisualBox[AmountOfBars];
            for (int i = 0; i < AmountOfBars; i++)
            {
                Bars[i] = ConstructVisualBox();
                Bars[i].Width = BarWidth;
            }

            AddBars();
        }

        protected override void LoadComplete()
        {
            RestoreBars();
            Scheduler.AddDelayed(() =>
            {
                if (bindableTrack == null || bindableTrack.Value == null)
                {
                    foreach (VisualBox bar in Bars)
                        bar.FadeOut();

                    return;
                }

                float[] amplitudes = bindableTrack?.Value?.CurrentAmplitudes.FrequencyAmplitudes;

                if (!(bindableTrack?.Value?.IsRunning).Value || amplitudes == null || Bars == null) return;

                for (int i = 0; i < AmountOfBars; i++)
                {
                    if (i > amplitudes.Length - 1) break;

                    Bars[i].Resize(amplitudes[i], Intensivity, Duration);
                }
            }, 25, true);

            base.LoadComplete();
        }

        protected virtual void AddBars() => Bars.ForEach(Add);
    }

    public class VisualBox : Container
    {
        private readonly bool CircularMode;

        private float LastOne;

        public VisualBox(bool CircularMode = false)
        {
            Masking = true;
            this.CircularMode = CircularMode;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = new Box
            {
                EdgeSmoothness = new Vector2(2f),
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
        }

        protected override void LoadAsyncComplete()
        {
            if (CircularMode)
                CornerRadius = Width / 2;

            base.LoadAsyncComplete();
        }

        public void Resize(float amplitude, float intensivity, int duration)
        {
            float newHeight = amplitude * intensivity;

            if (CircularMode)
                newHeight += Width;

            if (amplitude >= 0.25f)
            {
                if (LastOne >= 0.25f)
                    newHeight = osu.Framework.Utils.RNG.NextSingle(0.01f, 0.25f) * intensivity;

                LastOne = newHeight;
            }

            this.ResizeHeightTo(newHeight, duration / 4).OnComplete((x) => x.ResizeHeightTo(0, duration - (duration / 4)));
        }
    }
}
