﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Top = 1,
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

        // Control
        Enter,
        Escape
    }
}
