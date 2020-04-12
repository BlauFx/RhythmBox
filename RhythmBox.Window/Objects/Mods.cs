using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Mods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhythmBox.Window.Objects
{
    public class Mods : Container
    {
        public IEnumerable<Mod> Modlist = new Mod[]
        {
            new DummyMod(),
            new DummyMod2(),
            new DummyMod3()
        };

        /// <summary>
        /// Background color
        /// </summary>
        public new Color4 Colour { get; set; }

        private FillFlowContainer flowContainer;

        public List<Mod> ToApplyMods = new List<Mod>();

        [BackgroundDependencyLoader]
        private void Load()
        {
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

            for (int i = 0; i < Modlist.Count(); i++)
            {
                flowContainer.Add(new DrawMod(Modlist.ToList()[i], ToApplyMods, color)
                {
                    Depth = -2f,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 0.3f),
                    Alpha = 1f,
                });

                color = color == Color4.Blue ? Color4.Red : Color4.Blue;
            }
        }
    }

    internal class DrawMod : Container
    {
        private Mod Mod;

        private ClickBox CckBx;

        private Action action;

        public DrawMod(Mod mod, List<Mod> mods, Color4 Colour)
        {
            this.Mod = mod;

            action = () =>
            {
                if (CckBx.Rotation == 0f)
                    mods.Add(Mod);
                else
                    mods.Remove(Mod);

                CckBx.Colour = CckBx.Colour == Colour ? Color4.Yellow.Opacity(0.7f) : Colour;
            };
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            Children = new Drawable[]
            {
                CckBx = new ClickBox
                {
                    Depth = 1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 1f,
                    Colour = Colour,
                    EdgeSmoothness = new Vector2(2f),
                    EditorMode2 = true,
                    ClickAction = action,
                },
                new Sprite
                {
                    Depth = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Alpha = 1f,
                    EdgeSmoothness = new Vector2(2f),
                    Texture = store.Get($"Skin/{this.Mod.SkinElement}"),
                },
            };

            CckBx.OnMouseClick += () =>
            {
                Children.ForEach(drawable => drawable.RotateTo(drawable.Rotation == 0f ? 20f : 0f));
            };

            action.Invoke();
        }
    }
}
