namespace RhythmBox.Window.Mode.Standard.Maps
{
    public interface IMap
    {
        string AFileName { get; set; }

        string BGFile { get; set; }
        
        int MapId { get; set; } 
        
        int MapSetId { get; set; }

        int BPM { get; set; }
        
        GameMode Mode { get; set; }
        
        string Title { get; set; }
        
        string Artist { get; set; }
        
        string Creator { get; set; }
        
        string DifficultyName { get; set; }
        
        HitObject[] HitObjects { get; set; }

        int StartTime { get; set; }

        int EndTime { get; set; }

        string Path { get; set; }
    }
    
    public enum GameMode
    {
        STD
    }
    
    public record HitObject
    {
        public Direction _direction { get; } = Direction.Down;

        public double Time { get; } = 1d;

        public float Speed { get; } = 1f;

        public HitObject(Direction direction, double time, float speed)
        {
            _direction = direction;
            Time = time;
            Speed = speed;
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
