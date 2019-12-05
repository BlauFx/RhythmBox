using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osuTK;
using RhythmBox.Window.Overlays;

namespace RhythmBox.Tests.VisualTests.Overlays
{
    [TestFixture]
    public class TestSceneNotificationOverlay : TestScene
    {
        private NotificationOverlay _overlay;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                _overlay = new NotificationOverlay
                {
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.3f),
                }
            };

            AddStep("Pop In", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.Default;
                _overlay.State.Value = Visibility.Visible;
            });
            
            AddStep("Pop Out", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.Default;
                _overlay.State.Value = Visibility.Hidden;
            });

            AddStep("Pop In | YesNO", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.YesNo;
                _overlay.State.Value = Visibility.Visible;
            });

            AddStep("Pop Out| YesNO", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.YesNo;
                _overlay.State.Value = Visibility.Hidden;
            });

            AddStep("Pop In | YesCancel", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.YesCancel;
                _overlay.State.Value = Visibility.Visible;
            });

            AddStep("Pop Out| YesCancel", () =>
            {
                _overlay.typeOfOverlay = NotificationOverlay.TypeOfOverlay.YesCancel;
                _overlay.State.Value = Visibility.Hidden;
            });
        }
    }
}
