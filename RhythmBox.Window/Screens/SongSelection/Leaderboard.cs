using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace RhythmBox.Window.Screens.SongSelection
{
    public class Leaderboard : Container
    {
        private FillFlowContainer _fillFlowContainer;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new ScrollContainer
                {
                    ScrollbarVisible = true,
                    Depth = -1,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    DistanceDecayScroll = 0.005d,
                    DistanceDecayDrag = 0.004d,
                    ScrollDistance = 120f,
                    ClampExtension = 30f,

                    Child = _fillFlowContainer = new FillFlowContainer()
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(1f),
                        Direction = FillDirection.Vertical,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Y,
                    }
                },
            };

            const int YSize = 100;

            var boxes = new UserWrapper[1000];

            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new UserWrapper(new User($"Username{i}", osu.Framework.Utils.RNG.Next(0, int.MaxValue), DateTime.Now))
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1f, YSize),
                    Colour = Color4.Beige.Opacity(0.7f),
                    Margin = new MarginPadding { Bottom = 10 }
                };
            }

            List<UserWrapper> list = boxes.ToList();
            var orderByDescending = list.OrderByDescending(c => c.User.Score).ThenBy(c => c.User.Time).ThenBy(c => c.User.Username);
            _fillFlowContainer.AddRange(orderByDescending);
        }
    }

    internal record User(string Username, int Score, DateTime Time);

    internal class UserWrapper : Container
    {
        public User User { get; }

        public UserWrapper(User user) => this.User = user;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                    Colour = Color4.White,
                    Alpha = 0.5f,
                    X = 0f,
                    Y = 0f,
                },
                new SpriteText
                {
                    AllowMultiline = true,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = $"User: {User.Username}",
                    Font = new FontUsage("Roboto", 50f),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2((float) (User.Username.Length*0.5) / User.Username.Length, 0.5f),
                    Margin = new MarginPadding { Left = 10 }
                },
                new SpriteText
                {
                    AllowMultiline = true,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Text = User.Score.ToString(),
                    Font = new FontUsage("Roboto", 60f),
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2((float) (User.Score.ToString().Length*0.5) / User.Score.ToString().Length, 1),
                    Scale = new Vector2(0.7f),
                    Margin = new MarginPadding { Right = 10 }
                },
            };
        }
    }
}
