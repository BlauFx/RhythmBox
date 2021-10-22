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
using RhythmBox.Window.Animation;
using RhythmBox.Window.Maps;

namespace RhythmBox.Window.Screens
{
    public class Settings : Screen
    {
        private readonly SpriteText[] key = new SpriteText[4];

        private RBfocusedOverlayContainer focusedOverlayContainer;

        private bool overlayActive;

        private SettingsConfig lookupKey;

        private Volume volume;

        [Resolved] 
        private Gameini Gameini { get; set; }

        [Resolved]
        private CachedMap CachedMap { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            CachedMap.Play();

            var sliderBarValue = (BindableDouble)Gameini.GetBindable<double>(SettingsConfig.Volume);
            sliderBarValue.ValueChanged += e =>
            {
                if (CachedMap.BindableTrack.Value != null)
                    CachedMap.BindableTrack.Value.Volume.Value = e.NewValue;
            };

            InternalChildren = new Drawable[]
            {
                volume = new Volume(CachedMap.BindableTrack)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(1f, 0.3f),
                    X = 0.4f,
                    Y = 0.2f,
                    Alpha = 0f,
                },
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
                key[0] = GetSprite(0.05f, 0.03f, $"{Gameini.GetBindable<string>(SettingsConfig.KeyBindingUp).Value}"),
                key[1] = GetSprite(0.14f, 0.03f, $"{Gameini.GetBindable<string>(SettingsConfig.KeyBindingLeft).Value}"),
                key[2] = GetSprite(0.23f, 0.03f, $"{Gameini.GetBindable<string>(SettingsConfig.KeyBindingDown).Value}"),
                key[3] = GetSprite(0.32f, 0.03f, $"{Gameini.GetBindable<string>(SettingsConfig.KeyBindingRight).Value}"),
                GetClickBox(0.01f, 0.03f, SettingsConfig.KeyBindingUp),
                GetClickBox(0.1f, 0.03f, SettingsConfig.KeyBindingLeft),
                GetClickBox(0.19f, 0.03f, SettingsConfig.KeyBindingDown),
                GetClickBox(0.28f, 0.03f, SettingsConfig.KeyBindingRight),
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
            else if (overlayActive)
            {
                var keyStr = e.Key.ToString();
                overlayActive = false;

                for (var i = 0; i < key.Length; i++)
                {
                    var x = Gameini.Get<string>((SettingsConfig)i);
                    if (!string.Equals(x, keyStr, StringComparison.OrdinalIgnoreCase))
                        continue;
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                    return base.OnKeyDown(e);
                }

                Gameini.SetValue<string>(lookupKey, keyStr);
                Gameini.Save();

                focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Hidden;
                key[(int)lookupKey].Text = keyStr;
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(300, Easing.In);
            base.OnEntering(last);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            volume.ChangeVolume(e);
            volume.Fade(100, 100, 600);

            return base.OnScroll(e);
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

        private ClickBox GetClickBox(float xPos, float yPos, SettingsConfig lookupKey) =>
            new()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Size = new Vector2(0.08f),
                X = xPos,
                Y = yPos,
                Colour = Color4.Gray.Opacity(0.9f),
                ClickAction = () =>
                {
                    focusedOverlayContainer.State.Value = osu.Framework.Graphics.Containers.Visibility.Visible;
                    overlayActive = true;
                    this.lookupKey = lookupKey;
                },
            };
    }
}