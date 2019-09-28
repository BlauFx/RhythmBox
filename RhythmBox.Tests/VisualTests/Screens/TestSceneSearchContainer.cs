using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.pending_files;
using System.IO;
using System.Reflection;

namespace RhythmBox.Tests.VisualTests.Screens
{
    [TestFixture]
    public class TestSceneSearchContainer : TestScene
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

                    Children = new Drawable[]
                    {
                        head = new HeaderContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1f),

                            AutoSizeAxes = Axes.Y,
                        },
                    }
                }
            };

            int MapsCount = 2;

            var testSceneMapReader = new MapReader(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Songs\\TestMap\\Difficulty1.ini");

            Map[,] Map = new Map[MapsCount, short.MaxValue];

            Map[0, 0] = new Map
            {
                AFileName = testSceneMapReader.AFileName,
                BGFile = testSceneMapReader.BGFile,
                MapId = testSceneMapReader.MapId,
                MapSetId = testSceneMapReader.MapSetId,
                BPM = testSceneMapReader.BPM,
                Objects = testSceneMapReader.Objects,
                AutoMap = testSceneMapReader.AutoMap,
                Mode = testSceneMapReader.Mode,
                Title = "random text",//testSceneMapReader.Title,
                Artist = testSceneMapReader.Artist,
                Creator = testSceneMapReader.Creator,
                DifficultyName = testSceneMapReader.DifficultyName,
                StartTime = testSceneMapReader.StartTime,
                EndTime = testSceneMapReader.EndTime,
                HitObjects = testSceneMapReader.HitObjects,
                Path = testSceneMapReader.Path,
            };

            Map[1, 0] = new Map
            {
                AFileName = testSceneMapReader.AFileName,
                BGFile = testSceneMapReader.BGFile,
                MapId = testSceneMapReader.MapId,
                MapSetId = testSceneMapReader.MapSetId,
                BPM = testSceneMapReader.BPM,
                Objects = testSceneMapReader.Objects,
                AutoMap = testSceneMapReader.AutoMap,
                Mode = testSceneMapReader.Mode,
                Title = "title of this test map",//testSceneMapReader.Title,
                Artist = testSceneMapReader.Artist,
                Creator = testSceneMapReader.Creator,
                DifficultyName = testSceneMapReader.DifficultyName,
                StartTime = testSceneMapReader.StartTime,
                EndTime = testSceneMapReader.EndTime,
                HitObjects = testSceneMapReader.HitObjects,
                Path = testSceneMapReader.Path,
            };

            MapPack[] mapPackTests = new MapPack[MapsCount];

            for (int i = 0; i < mapPackTests.Length; i++)
            {
                mapPackTests[i] = new MapPack
                {
                    Maps = 1,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Colour = Color4.LightYellow,
                    Map = Map,
                    MapPos = i,
                    Search = Map[i, 0].Title,
                };
            }

            head.AddRange(mapPackTests);

            textBox.Current.ValueChanged += e => search.SearchTerm = e.NewValue;
        }
    }
}
