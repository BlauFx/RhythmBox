using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace RhythmBox.Window.Animation
{
    public class MusicVisualizationLinear : MusicVisualization
    {
        public float spacing;

        public float Spacing
        { 
            get => spacing; 
            set
            {
                spacing = value;

                if (!IsLoaded)
                    return;

                RestoreBars();
            } 
        }

        public MusicVisualizationLinear(float BarWidth, int AmountOfBars, float spacing, IBindable<Track> bindableTrack) : base(bindableTrack)
        {
            this.BarWidth = BarWidth;
            this.AmountOfBars = AmountOfBars;
            this.Spacing = spacing;

            RelativeSizeAxes = Axes.Both;
        }

        protected override void AddBars()
        {
            for (int i = 0; i < AmountOfBars; i++)
            {
                var bar = Bars[i];

                bar.Origin = Anchor.BottomCentre;

                if (IsReversed)
                    bar.Origin = Anchor.TopLeft;

                if (i >= 1)
                    bar.X = Bars[i - 1].X + Bars[i - 1].Width + Spacing;
            }

            base.AddBars();
        }
    }
}
