using System;

namespace PuyoPuyo.Toolbox
{
    public static class OrientationHandler
    {
        public static int Count = Enum.GetValues(typeof(Orientation)).Length;

        public static Orientation Next(Orientation orientation, Rotation rotation)
        {
            return NewOrientation(orientation, rotation == Rotation.Clockwise ? 1 : -1);
        }

        public static Orientation NewOrientation(Orientation orientation, int n)
        {
            n = n % Count;

            int o = (int)orientation;
            int next_o = o + n;
            int index_o = (next_o + Count) % Count; // handle negative values
            return (Orientation)(index_o);
        }

        public static Orientation GetMirrorOrientation(Orientation orientation)
        {
            return NewOrientation(orientation, 2);
        }
    }
}
