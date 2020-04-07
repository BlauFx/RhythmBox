using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace RhythmBox.Window.Animation
{
    public class Volume : Container
    {
        private IBindable<Track> ITrack;

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
                new Box
                {
                    Depth = float.MinValue + 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(.1f, .03f),
                    Colour = Color4.Red,
                },
            };

            ChangeVolume(false);
        }

        public void ChangeVolume(bool CalledFromOnScroll, ScrollEvent e = null)
        {
            if (CalledFromOnScroll)
            {
                if (e.ScrollDelta.Y > 0f)
                {
                    if (ITrack.Value.Volume.Value == 1d) return;

                    ITrack.Value.Volume.Value += 0.25d;
                }
                else
                {
                    if (ITrack.Value.Volume.Value == 0d) return;

                    ITrack.Value.Volume.Value -= 0.25d;
                }
            }

            Children.Where(x => x.GetType() == typeof(Box)).FirstOrDefault().MoveTo(new Vector2(0f, ITrack.Value.Volume.Value switch
            {
                1d => -0.405f,
                0.75d => -0.1265f,
                0.5d => 0.012f,
                0.25d => 0.142f,
                0d => 0.37f,
                _ => throw new System.Exception(),
            }), 100);
        }
    }
}
