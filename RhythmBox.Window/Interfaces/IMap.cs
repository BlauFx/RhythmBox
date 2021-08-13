using RhythmBox.Window.Maps;

namespace RhythmBox.Window.Interfaces
{
    public interface IMap
    {
        string AFileName { get; set; }

        string BgFile { get; set; }
        
        int MapId { get; set; }
        
        int MapSetId { get; set; }

        int BPM { get; set; }
        
        string Title { get; set; }
        
        string Artist { get; set; }
        
        string Creator { get; set; }
        
        string DifficultyName { get; set; }
        
        HitObject[] HitObjects { get; set; }

        int StartTime { get; set; }

        int EndTime { get; set; }

        string Path { get; set; }
    }
    
    public record HitObject
    {
        public DirectionEnum Direction { get; } = DirectionEnum.Down;

        public double Time { get; } = 1d;

        public float Speed { get; } = 1f;

        public HitObject(DirectionEnum direction, double time, float speed)
        {
            Direction = direction;
            Time = time;
            Speed = speed;
        }

        public enum DirectionEnum
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
