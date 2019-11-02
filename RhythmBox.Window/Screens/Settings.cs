using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Objects;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class Settings : Screen
    {
        private SpriteText key1, key2, key3, key4;

        [BackgroundDependencyLoader]
        private void Load(Gameini gameini)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Gray.Opacity(0.6f),
                },
                new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(120f),
                    Colour = Color4.Gray.Opacity(0.9f),
                    Text = "Settings",
                    Font = new FontUsage("Roboto", 40f)
                },
                //TODO:
                new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(160f, 100f),
                    Colour = Color4.Gray.Opacity(0.9f),
                    Text = "Keybindings:",
                    X = 10f,
                    Font = new FontUsage("Roboto", 40f)
                },
                key1 = new SpriteText
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.05f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key2 = new SpriteText
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.14f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key3 = new SpriteText
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.23f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key4 = new SpriteText
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.32f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingRight).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                new ClickBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.08f),
                    X = 0.01f,
                    Y = 0.03f,
                    Colour = Color4.Gray.Opacity(0.9f),
                    EditorMode2 = true,
                },
                new ClickBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.08f),
                    X = 0.1f,
                    Y = 0.03f,
                    Colour = Color4.Gray.Opacity(0.9f),
                    EditorMode2 = true,
                },
                new ClickBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.08f),
                    X = 0.19f,
                    Y = 0.03f,
                    Colour = Color4.Gray.Opacity(0.9f),
                    EditorMode2 = true,
                },
                new ClickBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.08f),
                    X = 0.28f,
                    Y = 0.03f,
                    Colour = Color4.Gray.Opacity(0.9f),
                    EditorMode2 = true,
                }
            };
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(300, Easing.In);
            base.OnEntering(last);
        }
    }
}
