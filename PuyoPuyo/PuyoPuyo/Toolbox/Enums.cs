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
        None,
        // Center
        Home,
        Start,
        Back,
        // D-Pad
        Up,
        Left,
        Down,
        Right,
        // Face buttons
        Y,
        X,
        A,
        B,
        // Shoulder buttons
        LeftShoulder,
        RightShoulder,
        // Triggers
        LeftTrigger,
        RightTrigger,
        // Left Stick
        LeftStick,
        LeftStickUp,
        LeftStickLeft,
        LeftStickDown,
        LeftStickRight,
        // Right Stick
        RightStick,
        RightStickUp,
        RightStickLeft,
        RightStickDown,
        RightStickRight,
        //Pad
        PadUp,
        PadLeft,
        PadDown,
        PadRight,
        // Control
        Enter,
        Escape
    }

    public enum PuyoColor
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
