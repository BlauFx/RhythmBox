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
        private IBindable<Track> ITrack;

        private Container CurrentValue;

        [Resolved]
        private Gameini gameini { get; set; }

        public Volume(IBindable<Track> bindableTrack)
        {
            ITrack = bindableTrack;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new Sprite
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f, 1f),
                    Texture = store.Get("Game/Volume"),
                },
                CurrentValue = new Container
                {
                    Depth = float.MinValue + 1,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 1f),
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Hollow = false,
                        Type = EdgeEffectType.Glow,
                        Radius = 1,
                        Colour = Color4.Red.Opacity(1f),
                    },

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Depth = float.MinValue,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Size = new Vector2(1f),
                            Colour = Color4.Red.Opacity(0.7f),
                            Alpha = 0.3f,
                        },
                        new Box
                        {
                            Depth = float.MinValue + 1,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Size = new Vector2(1f),
                            Colour = Color4.Gray.Opacity(.1f),
                            Alpha = 1f,
                        },
                    }
                },

            };

            ChangeVolume(false);
        }

        public void ChangeVolume(bool CalledFromOnScroll, ScrollEvent e = null)
        {
            if (ITrack.Value == null) return;

            var VolValue = ITrack.Value.Volume.Value;

            if (CalledFromOnScroll)
            {
                if (e.ScrollDelta.Y > 0f)
                    VolValue = VolValue != 1d ? ITrack.Value.Volume.Value += 0.25d : VolValue;
                else
                    VolValue = VolValue != 0d ? ITrack.Value.Volume.Value -= 0.25d : VolValue;

                gameini.Set(SettingsConfig.Volume, VolValue);
                gameini.Save();
            }

            CurrentValue.Height = (float)VolValue * 1f + 0.01f;

            switch (VolValue)
            {
                case 1d:
                    CurrentValue.Height -= .01f;
                    break;
                case 0d:
                    CurrentValue.Height = -.1f;
                    break;
            }
        }
    }
}
