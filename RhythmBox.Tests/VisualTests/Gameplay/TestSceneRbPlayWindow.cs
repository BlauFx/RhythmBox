using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using RhythmBox.Mode.Std.Tests.Animations;
using RhythmBox.Mode.Std.Tests.Maps;
using RhythmBox.Tests.pending_files;
using HitObjects = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects;

namespace RhythmBox.Tests.VisualTests.Gameplay
{
    [TestFixture]
    public class TestSceneRbPlayWindow : TestScene
    {
        private int Score { get; set; } = 0;

        private int Combo { get; set; } = 0;

        private double Accuracy { get; set; } = 100;

        private TextFlowContainer DispayCombo;

        private TextFlowContainer DispayScore;

        private TestSceneMap _map;

        private TestSceneRbPlayfield _testSceneRbPlayfield;

        private Mode.Std.Tests.Animations.TestSceneHpBar _hpBar;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            _map = new TestSceneMap
            {
                AFileName = "null",
                BGFile = "none",
                MapId = 0,
                MapSetId = 0,
                BPM = 150,
                Objects = 10,
                AutoMap = false,
                Mode = RhythmBox.Mode.Std.Tests.Interfaces.GameMode.STD,
                Title = "Test Title",
                Artist = "Test Artist",
                Creator = "Test Creator",
                DifficultyName = "Test DifficultyName",
                
            };

            //TODO:  note: this is temporary
            _map.HitObjects = new Mode.Std.Tests.Interfaces.HitObjects[4];
            
            _map.HitObjects[0] = new HitObjects();
            _map.HitObjects[1] = new HitObjects();
            _map.HitObjects[2] = new HitObjects();
            _map.HitObjects[3] = new HitObjects();
            
            _map.HitObjects[0]._direction = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects.Direction.Up;
            _map.HitObjects[1]._direction = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects.Direction.Right;
            _map.HitObjects[2]._direction = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects.Direction.Left;
            _map.HitObjects[3]._direction = RhythmBox.Mode.Std.Tests.Interfaces.HitObjects.Direction.Down;
            
            _map.HitObjects[0].Speed = 1f;
            _map.HitObjects[1].Speed = 1f;
            _map.HitObjects[2].Speed = 1f;
            _map.HitObjects[3].Speed = 1f;

            _map.HitObjects[0].Time = 200;
            _map.HitObjects[1].Time = 400;
            _map.HitObjects[2].Time = 700;
            _map.HitObjects[3].Time = 780;
            
            Children = new Drawable[]
            {
                _hpBar = new Mode.Std.Tests.Animations.TestSceneHpBar
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                    BoxMaxValue = 0.1f,
                    colour = Color4.AliceBlue,
                },
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
                _testSceneRbPlayfield = new TestSceneRbPlayfield
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

        protected override void Update()
        {
            if (_testSceneRbPlayfield.HasFinished)
            {
                if (_testSceneRbPlayfield.HasStarted)
                {
                    //this.Push(new SongSelction());
                    Logger.Log(_testSceneRbPlayfield.HasFinished.ToString());
                }
            }
            
            _hpBar.ResizeBox(CalcHpBarValue(_hpBar._box.Width,_hpBar.BoxMaxValue,0f, Hit.Hit100, true),10000, Easing.OutCirc);
            
            Combo = _testSceneRbPlayfield.ComboCounter;
            DispayCombo.Text = string.Empty;
            DispayCombo.AddText($"{Combo}x", x => x.Font = new FontUsage("Roboto", 40));

            Score = _testSceneRbPlayfield.ScoreCounter;
            DispayScore.Text = string.Empty;
            DispayScore.AddText($"{Score}", x => x.Font = new FontUsage("Roboto", 40));
            base.Update();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            _hpBar.ResizeBox(CalcHpBarValue(_hpBar._box.Width,_hpBar.BoxMaxValue,0f,_testSceneRbPlayfield.currentHit),1000, Easing.OutCirc);
            return base.OnKeyDown(e);
        }

        private float CalcHpBarValue(float currentvalue, float maxvalue, float minvalue, Hit hit, bool auto = false)
        {
            if (!auto)
            {
                float result = 0;
                switch (hit)
                {
                    case Hit.Hit300:
                        result = currentvalue * 1.5f;
                        break;
                    case Hit.Hit100:
                        result = currentvalue * 0.8f;
                        break;
                    case Hit.Hit50:
                        result = currentvalue * 0.7f;
                        break;
                    case Hit.Hitx:
                        result = currentvalue * 0.3f;
                        break;
                }

                if (result < maxvalue && result > minvalue)
                {
                    return result;
                }
                else if (result > maxvalue)
                {
                    return maxvalue;
                }
                else if (result < minvalue)
                {
                    return minvalue;
                }
            }
            
            return currentvalue * 0.995f;
        }
    }
}
