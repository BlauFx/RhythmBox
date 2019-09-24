namespace RhythmBox.Mode.Std.Interfaces
{
    public interface IMap
    {
        string AFileName { get; set; }

        string BGFile { get; set; }
        
        int MapId { get; set; } 
        
        int MapSetId { get; set; }

        int BPM { get; set; }
        
        int Objects { get; set; }

        bool AutoMap { get; set; }

        GameMode Mode { get; set; }
        
        string Title { get; set; }
        
        string Artist { get; set; }
        
        string Creator { get; set; }
        
        string DifficultyName { get; set; }
        
        HitObjects[] HitObjects { get; set; }

        int StartTime { get; set; }

        int EndTime { get; set; }

        string Path { get; set; }
    }
    
    public enum GameMode
    {
        STD,
    }
    
    public class HitObjects
    {
        public Direction _direction { get; set; }

        public double Time { get; set; }

        public float Speed { get; set; }

        public HitObjects()
        {
            this._direction = Direction.Down;
            this.Time = 1;
            this.Speed = 1f;
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
