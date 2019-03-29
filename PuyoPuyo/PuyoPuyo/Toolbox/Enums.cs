using System;
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

    public enum Puyo
    {
        Undefined = -1,
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Purple = 4
    }
}
