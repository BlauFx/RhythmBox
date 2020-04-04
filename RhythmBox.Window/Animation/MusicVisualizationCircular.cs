using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;

namespace RhythmBox.Window.Animation
{
    public class MusicVisualizationCircular : MusicVisualization
    {
        private float circleSize;

        public float CircleSize
        {
            get => circleSize;
            set
            {
                circleSize = value;

                if (!IsLoaded)
                    return;

                foreach (var bar in Bars)
                    bar.Position = CalcBarPos(bar.Rotation);
            }
        }

        private float degree;

        public float Degree
        {
            get => degree;
            set
            {
                degree = value;

                if (!IsLoaded)
                    return;

                AdjustBars();
            }
        }

        protected override VisualBox ConstructVisualBox() => new VisualBox(true);

        public MusicVisualizationCircular(int Degree, float BarWidth, int AmountOfBars, int Radius, IBindable<Track> bindableTrack) : base (bindableTrack)
        {
            this.Degree = Degree;
            this.BarWidth = BarWidth;
            this.AmountOfBars = AmountOfBars;
            this.CircleSize = Radius;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        private void AdjustBars()
        {
            for (int i = 0; i < AmountOfBars; i++)
            {
                var bar = Bars[i];

                bar.Origin = Anchor.BottomCentre;

                if (IsReversed)
                    bar.Origin = Anchor.TopLeft;

                float rotationValue = i * (Degree / AmountOfBars);

                bar.Rotation = rotationValue;
                bar.Position = CalcBarPos(rotationValue);
            }
        }

        private Vector2 CalcBarPos(float rotationValue)
        {
            float rotation = MathHelper.DegreesToRadians(rotationValue);
            float rotationCos = (float)Math.Cos(rotation);
            float rotationSin = (float)Math.Sin(rotation);
            return new Vector2(rotationSin / 2, -rotationCos / 2) * circleSize;
        }

        protected override void AddBars()
        {
            AdjustBars();
            base.AddBars();
        }
    }
}
