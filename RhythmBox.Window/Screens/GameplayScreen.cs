using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using RhythmBox.Mode.Std.Interfaces;
using RhythmBox.Mode.Std.Maps;
using RhythmBox.Window.pending_files;

namespace RhythmBox.Window.Screens
{
    public class GameplayScreen : Screen
    {
        private int Score { get; set; } = 0;

        private int Combo { get; set; } = 0;

        private double Accuracy { get; set; } = 100;

        private int hit300 { get; set; } = 0;

        private int hit100 { get; set; } = 0;

        private int hit50 { get; set; } = 0;

        private int hitx { get; set; } = 0;

        private TextFlowContainer DispayCombo;

        private TextFlowContainer DispayScore;
        
        private Map _map;

        private RbPlayfield _testSceneRbPlayfield;

        [BackgroundDependencyLoader]
        private void Load()
        {
            _map = new Map
            {
                AFileName = "null",
                BGFile = "none",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Objects = 10,
                AutoMap = false,
                Mode = RhythmBox.Mode.Std.Interfaces.GameMode.STD,
                Title = "Test Title",
                Artist = "Test Artist",
                Creator = "Test Creator",
                DifficultyName = "Test DifficultyName",
                
            };

            //TODO:  note: this is temporary
            _map.HitObjects = new Mode.Std.Interfaces.HitObjects[4];
            
            _map.HitObjects[0] = new HitObjects();
            _map.HitObjects[1] = new HitObjects();
            _map.HitObjects[2] = new HitObjects();
            _map.HitObjects[3] = new HitObjects();
            
            _map.HitObjects[0]._direction = RhythmBox.Mode.Std.Interfaces.HitObjects.Direction.Up;
            _map.HitObjects[1]._direction = RhythmBox.Mode.Std.Interfaces.HitObjects.Direction.Right;
            _map.HitObjects[2]._direction = RhythmBox.Mode.Std.Interfaces.HitObjects.Direction.Left;
            _map.HitObjects[3]._direction = RhythmBox.Mode.Std.Interfaces.HitObjects.Direction.Down;
            
            _map.HitObjects[0].Speed = 1f;
            _map.HitObjects[1].Speed = 1f;
            _map.HitObjects[2].Speed = 1f;
            _map.HitObjects[3].Speed = 1f;

            _map.HitObjects[0].Time = 200;
            _map.HitObjects[1].Time = 400;
            _map.HitObjects[2].Time = 700;
            _map.HitObjects[3].Time = 780;
            
            InternalChildren = new Drawable[]
            {
                DispayCombo = new TextFlowContainer
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.BottomLeft,
                },
                DispayScore = new TextFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.1f),
                    TextAnchor = Anchor.TopRight,
                    X = -0.01f
                },
                _testSceneRbPlayfield = new RbPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.6f, 1f),
                    Map = _map,
                },
            };
            DispayCombo.AddText("0x", x => x.Font = new FontUsage("Roboto", 40));
            DispayScore.AddText("000000", x => x.Font = new FontUsage("Roboto", 40));
        }
    }
}
