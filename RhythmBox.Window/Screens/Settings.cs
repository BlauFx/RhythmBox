using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using RhythmBox.Window.Objects;
using RhythmBox.Window.Overlays;
using System;
using RhythmBox.Window.Maps;

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

        [Resolved]
        private CachedMap cachedMap { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            cachedMap.Play();

            var sliderBarValue = new BindableDouble(cachedMap.BindableTrack.Value?.Volume.Value ?? gameini.Get<double>(SettingsConfig.Volume)) { MinValue = 0, MaxValue = 1, Precision = 0.25d };
            sliderBarValue.ValueChanged += (e) =>
            {
                if (cachedMap.BindableTrack.Value != null)
                    cachedMap.BindableTrack.Value.Volume.Value = e.NewValue;

                gameini.GetBindable<double>(SettingsConfig.Volume).Value = e.NewValue;
                gameini.Save();
            };

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
                new SpriteText
                {
                    Depth = 0,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(160f, 100f),
                    RelativePositionAxes = Axes.Y,
                    Colour = Color4.Gray.Opacity(0.9f),
                    Text = "Audio:",
                    X = 10f,
                    Y = -0.3f,
                    Font = new FontUsage("Roboto", 40f)
                },
                key[0] = GetSprite(0.05f, 0.03f, $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value}"),
                key[1] = GetSprite(0.14f, 0.03f, $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value}"),
                key[2] = GetSprite(0.23f, 0.03f, $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value}"),
                key[3] = GetSprite(0.32f, 0.03f, $"{gameini.GetBindable<string>(SettingsConfig.KeyBindingRight).Value}"),
                GetClickBox(0.01f, 0.03f, 0),
                GetClickBox(0.1f, 0.03f, 1),
                GetClickBox(0.19f, 0.03f, 2),
                GetClickBox(0.28f, 0.03f, 3),
                new BasicSliderBar<double>
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.2f, 0.02f),
                    Y = -0.2f,
                    BackgroundColour = Color4.White,
                    SelectionColour = Color4.Pink,
                    KeyboardStep = 1,
                    TransferValueOnCommit = false,
                    Current = sliderBarValue
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
            {
                if (focusedOverlayContainer.State.Value == osu.Framework.Graphics.Containers.Visibility.Visible)
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                else
                    this.Exit();
            }
            else if (OverlayActive)
            {
                OverlayActive = false;

                if (CheckIfAlreadyInUse(e.Key))
                {
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                    return base.OnKeyDown(e);
                }

                var settingsconfig = CurrentKey switch
                {
                    0 => SettingsConfig.KeyBindingUp,
                    1 => SettingsConfig.KeyBindingLeft,
                    2 => SettingsConfig.KeyBindingDown,
                    3 => SettingsConfig.KeyBindingRight,
                    _ => throw new Exception($"CurrentKey cannot be {CurrentKey}")
                };
                
                gameini.SetValue<string>(settingsconfig, e.Key.ToString());
                focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                gameini.Save();
            }

            return base.OnKeyDown(e);
        }

        private bool CheckIfAlreadyInUse(Key e)
        {
            Enum.TryParse(gameini.Get<string>(SettingsConfig.KeyBindingUp), out Key KeyUp);
            Enum.TryParse(gameini.Get<string>(SettingsConfig.KeyBindingLeft), out Key KeyLeft);
            Enum.TryParse(gameini.Get<string>(SettingsConfig.KeyBindingDown), out Key KeyDown);
            Enum.TryParse(gameini.Get<string>(SettingsConfig.KeyBindingRight), out Key KeyRight);

            return (e == KeyUp || e == KeyLeft || e == KeyDown || e == KeyRight);
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(300, Easing.In);
            base.OnEntering(last);
        }

        private SpriteText GetSprite(float XPos, float YPos, string text) =>
            new()
            {
                Depth = -1,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                X = XPos,
                Y = YPos,
                Text = text,
                Font = new FontUsage("Roboto", 40f)
            };

        private ClickBox GetClickBox(float XPos, float YPos, int currentKey) =>
            new()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Size = new Vector2(0.08f),
                X = XPos,
                Y = YPos,
                Colour = Color4.Gray.Opacity(0.9f),
                EditorMode2 = true,
                ClickAction = () =>
                {
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                    OverlayActive = true;
                    CurrentKey = currentKey;
                },
            };
    }
}