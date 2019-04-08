namespace PuyoPuyo.Toolbox
{
    public enum Rotation
    {
        Clockwise,
        Counterclockwise
    }

    public enum Orientation
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3
    }

    public enum Input
    {
        Up,
        Left,
        Down,
        Right,
        Pause,
        Validate,
        Cancel,
        CounterclockwiseRotation,
        ClockwiseRotation,
    }

    public enum Puyo
    {
        Undefined = -1,
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Purple = 4
    }

    public enum GameboardState
    {
        PAUSED, 
        RUNNING,
    }
}
