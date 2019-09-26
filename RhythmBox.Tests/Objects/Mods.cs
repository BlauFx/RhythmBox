﻿using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Mods;
using System.Linq;
using System.Collections.Generic;

namespace RhythmBox.Tests.Objects
{
    public class Mods : Container
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
                flowContainer.Add(new DrawMod
                {
                    Depth = -2f,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.1f, 0.3f),
                    Alpha = 1f,
                    Colour = color,
                    ToApplyMods = ToApplyMods,
                    Mod = Modlist.ToList()[i],
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

    class DrawMod : Container
    {
        public List<Mod> ToApplyMods { get; set; }

        public Mod Mod { get; set; }

        public new Color4 Colour { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Child = new ThisBox(Mod)
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f),
                Alpha = 1f,
                Colour = Colour,
                ToApplyMods = ToApplyMods,
            };
        }
    }

    internal class ThisBox : Box
    {
        public List<Mod> ToApplyMods { get; set; }

        private bool Applied = false;

        private Color4 orgColor;

        public Mod Mod { get; set; }

        public ThisBox(Mod mod)
        {
            this.Mod = mod;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!Applied)
            {
                Applied = true;
                ToApplyMods.Add(Mod);

                orgColor = this.Colour;
                this.Colour = Color4.Yellow.Opacity(0.7f);
                this.Rotation += 20f;
            }
            else
            {
                Applied = false;
                ToApplyMods.Remove(Mod);

                this.Colour = orgColor;
                this.Rotation -= 20f;
            }

            return base.OnMouseDown(e);
        }
    }
}