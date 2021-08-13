using System;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window.Animation
{
    public class Volume : Container
    {
        private readonly IBindable<Track> track;

        private Box currentValue;

        private TextFlowContainer volumeInPercent;

        [Resolved]
        private Gameini gameini { get; set; }

        public Volume(IBindable<Track> bindableTrack)
        {
            track = bindableTrack;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                volumeInPercent = new TextFlowContainer
                {
                    Depth = -1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 1f),
                    Text = "",
                    TextAnchor = Anchor.Centre,
                },
                new Container
                {
                    Alpha = 1f,
                    Depth = 0,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 1f),
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Hollow = true,
                        Type = EdgeEffectType.Glow,
                        Radius = 5,
                        Colour = Color4.AliceBlue.Opacity(1f),
                    },
                    Children = new Drawable[]
                    {
                        currentValue = new Box
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Size = new Vector2(1f),
                            Colour = Color4.Red.Opacity(100),
                            Alpha = 1f,
                        },
                    }
                }
            };

            ChangeVolume(false);
        }

        public void ChangeVolume(bool calledFromOnScroll, ScrollEvent e = null)
        {
            var volValue = track?.Value?.Volume?.Value ?? gameini.Get<double>(SettingsConfig.Volume);
            if (calledFromOnScroll)
            {
                var precision = ((BindableDouble)gameini.GetBindable<double>(SettingsConfig.Volume)).Precision;

                if (e is { ScrollDelta: { Y: > 0f } })
                    volValue = volValue <= 1d - precision ? volValue + precision : volValue;
                else
                    volValue = volValue >= 0d + precision ? volValue - precision : volValue;

                gameini.SetValue(SettingsConfig.Volume, volValue);
                gameini.Save();

                track?.Value?.Volume?.Set(volValue);
            }
            currentValue.Height = (float)volValue;
            volumeInPercent.Text = "";
            volumeInPercent.AddText($"{volValue * 100:0}%", x => x.Font = new FontUsage("Roboto", 50));
        }
    }
}
