using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window.Objects
{
    public class Checkbox : CircularContainer
    {
        private Box flash;
        private Action clickAction;
        private readonly string texture = string.Empty;
        private Sprite sprite;

        private Texture spriteTexture;
        private bool running ;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Masking = true;
            flash = new Box
            {
                Depth = -1,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.WhiteSmoke,
                Alpha = 0
            };
            Children = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Depth  = 1,
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 0F,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Depth  = 1,
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                            Alpha = 1F
                        },
                    }
                },
                new Container
                {
                    Size = new Vector2(0.95F),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Depth  = 0,
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 5F,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Depth  = 3,
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                            Alpha = 0.925F
                        },
                        sprite = new Sprite
                        {
                            Size = new Vector2(0.7F),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 1F,
                            Texture = store.Get("Game/" + texture),
                           // Size = new Vector2(25),
                        }
                    }
                },
                flash
            };
            spriteTexture = store.Get("Game/" + texture); //f optimize this
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Logger.Log("MouseButton: " + e.Button, LoggingTarget.Information, LogLevel.Debug);
            return base.OnMouseDown(e);
        }
        protected override bool OnClick(ClickEvent e)
        {
            if (sprite.Texture == spriteTexture && running == false)
            {
                running = true;
                sprite.FadeOutFromOne(100, Easing.Out);
                Scheduler.AddDelayed(() => sprite.Texture = null, 101);
            }
            else if (running == false)
            {
                    running = true;
                    sprite.Texture = spriteTexture;
                    sprite.FadeInFromZero(131, Easing.In);
            }

            clickAction?.Invoke();

            running = false;
            return base.OnClick(e);
        }
    }
}
