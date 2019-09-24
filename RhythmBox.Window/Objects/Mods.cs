using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Mods;

using System.Collections.Generic;

namespace RhythmBox.Window.Objects
{
    class Mods : Container
    {
        public IEnumerable<Mod> Modlist = new Mod[]
        {
            new DummyMod(),
        };

        private int IEnumerableLength;

        public new Color4 Colour { get; set; }

        private FillFlowContainer flowContainer;

        public List<Mod> ToApplyMods = new List<Mod>();

        [BackgroundDependencyLoader]
        private void Load()
        {
            foreach (var x in Modlist)
            {
                IEnumerableLength++;
            }

            Color4 color = Color4.Red;

            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 0f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 1f,
                    Colour = Colour,
                },
                flowContainer = new FillFlowContainer
                {
                    Depth = -1f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 1f,
                },
            };

            for (int i = 0; i < IEnumerableLength; i++)
            {
                flowContainer.Add(new TestSceneDrawMod
                {
                    Depth = -2f,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 0.3f),
                    Alpha = 1f,
                    Colour = color,
                    ToApplyMods = ToApplyMods,
                    AddThisInt = i,
                });

                if (color == Color4.Blue)
                {
                    color = Color4.Red;
                }
                else
                {
                    color = Color4.Blue;
                }
            }
        }
    }

    class TestSceneDrawMod : Container
    {
        public List<Mod> ToApplyMods;

        public int AddThisInt;

        public new Color4 Colour { get; set; }

        private ThisBox box;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                box = new ThisBox
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 1f,
                    Colour = Colour,
                    ToApplyMods = ToApplyMods,
                    AddThisInt = AddThisInt,
                },
            };
        }
    }

    internal class ThisBox : Box
    {
        public List<Mod> ToApplyMods;

        public int AddThisInt;

        private bool Applied = false;

        private Color4 orgColor;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!Applied)
            {
                Applied = true;
                ToApplyMods.Add(new DummyMod());
                orgColor = this.Colour;
                this.Colour = Color4.Yellow.Opacity(0.7f);
            }
            else
            {
                //TODO: Remove AddThisInt from ToApplyMods
                Applied = false;
                this.Colour = orgColor;
            }

            return base.OnMouseDown(e);
        }
    }
}
