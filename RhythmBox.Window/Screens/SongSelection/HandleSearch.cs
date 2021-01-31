using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Window.Maps;
using RhythmBox.Window.Mode.Standard.Maps;

namespace RhythmBox.Window.Screens.SongSelection
{
    public class HandleSearch : Container
    {
        public SearchContainer search;

        private HeaderContainer head;

        private BasicTextBox textBox;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;

            Children = new Drawable[]
            {
                textBox = new BasicTextBox
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Size = new Vector2(200, 40),
                },
                search = new SearchContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,

                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1f),

                    AutoSizeAxes = Axes.Y,

                    Child = head = new HeaderContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(1f),
                        AutoSizeAxes = Axes.Y,
                    },
                }
            };

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +"\\Songs\\TestMap\\Difficulty1.ini";
            
            MapPack mapPack = new MapPack(new []
            {
                new Map(path, "random text"),
                new Map(path, "title of this test map")
            });

            var mapPackReversed = mapPack.Maps.Reverse().ToArray();
            
            List<MapPackDrawer> mapPackDrawer = new List<MapPackDrawer>();
            List<MapPack> mapPacksGot = new List<MapPack>()
            {
                mapPack, new MapPack(mapPackReversed)
            };
            
            mapPacksGot.ForEach((x) =>
            {
                mapPackDrawer.Add(new MapPackDrawer(x.Maps, x.Maps[0].Title)
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Colour = Color4.Blue,
                });
            });

            head.AddRange(mapPackDrawer);

            textBox.Current.ValueChanged += e => search.SearchTerm = e.NewValue;
        }
    }
}
