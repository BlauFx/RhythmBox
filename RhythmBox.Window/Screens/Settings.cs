using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Objects;
using RhythmBox.Window.Overlays;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class Settings : Screen
    {
        private SpriteText[] key = new SpriteText[4];

        private RBfocusedOverlayContainer focusedOverlayContainer;

        private bool OverlayActive = false;

        private int CurrentKey;

        [Resolved]
        private Gameini gameini { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            InternalChildren = new Drawable[]
            {
                focusedOverlayContainer = new RBfocusedOverlayContainer(Color4.Black.Opacity(0.9f), true)
                {
                    Depth = float.MinValue,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
                new Box
                {
                    Depth = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    Colour = Color4.Gray.Opacity(0.6f),
                },
                new SpriteText
                {
                    Depth = 0,
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
                    Depth = 0,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(160f, 100f),
                    Colour = Color4.Gray.Opacity(0.9f),
                    Text = "Keybindings:",
                    X = 10f,
                    Font = new FontUsage("Roboto", 40f)
                },
                key[0] = new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.05f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key[1] = new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.14f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key[2] = new SpriteText
                {
                    Depth = -1,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativePositionAxes = Axes.Both,
                    X = 0.23f,
                    Y = 0.03f,
                    Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value}",
                    Font = new FontUsage("Roboto", 40f)
                },
                key[3] = new SpriteText
                {
                    Depth = -1,
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
                    ClickAction = () =>
                    {
                        focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                        OverlayActive = true;
                        CurrentKey = 0;
                    },
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
                    ClickAction = () =>
                    {
                        focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                        OverlayActive = true;
                        CurrentKey = 1;
                    },
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
                    ClickAction = () =>
                    {
                        focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                        OverlayActive = true;
                        CurrentKey = 2;
                    },
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
                    ClickAction = () =>
                    {
                        focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                        OverlayActive = true;
                        CurrentKey = 3;
                    },
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                if (focusedOverlayContainer.State.Value == osu.Framework.Graphics.Containers.Visibility.Visible)
                {
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                }
                else
                {
                    this.Exit();
                }
            }

            if (OverlayActive)
            {
                _ = CurrentKey switch
                {
                    0 => gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value = e.Key.ToString(),
                    1 => gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value = e.Key.ToString(),
                    2 => gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value = e.Key.ToString(),
                    3 => gameini.GetBindable<string>(SettingsConfig.KeyBindingRight).Value = e.Key.ToString(),
                    _ => throw new System.Exception(),
                };

                key[0].Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value}";
                key[1].Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value}";
                key[2].Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value}";
                key[3].Text = $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingRight).Value}";

                focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(300, Easing.In);
            base.OnEntering(last);
        }
    }
}
