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

namespace RhythmBox.Tests.Objects
{
    public class TestSceneCheckbox : CircularContainer
    {
        protected Box Flash;
        public Action ClickAction;
        public string Texture = string.Empty;
        public Sprite sp;

        public Texture sptex;
        private bool running = false;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Masking = true;
            Flash = new Box
            {
                Depth = -1,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.WhiteSmoke,
                Alpha = 0
            };
            InternalChildren = new Drawable[]
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
                        sp = new Sprite
                        {
                            Size = new Vector2(0.7F),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 1F,
                            Texture = store.Get("Game/" + Texture),
                           // Size = new Vector2(25),
                        }
                    }
                },
                Flash
            };
            sptex = store.Get("Game/" + Texture); //f optimize this
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Logger.Log("MouseButton: " + e.Button.ToString(), LoggingTarget.Information, LogLevel.Debug);
            return base.OnMouseDown(e);
        }
        protected override bool OnClick(ClickEvent e)
        {
            if (sp.Texture == sptex && running == false)
            {
                running = true;
                sp.FadeOutFromOne(100, Easing.Out);
                Scheduler.AddDelayed(() => sp.Texture = null, 101);
            }
            else
            {
                if (running == false)
                {
                    running = true;
                    sp.Texture = sptex;
                    sp.FadeInFromZero(131, Easing.In);
                }
            }

            if (ClickAction != null)
            {
                ClickAction.Invoke();
            }

            running = false;

            return base.OnClick(e);
        }
    }
}