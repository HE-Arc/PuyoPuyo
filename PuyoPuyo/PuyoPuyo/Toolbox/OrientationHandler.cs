using System;

namespace PuyoPuyo.Toolbox
{
    public static class OrientationHandler
    {
        public static int Count = Enum.GetValues(typeof(Orientation)).Length;

        public static Orientation NewOrientation(Orientation orientation, int n)
        {
            return (Orientation)((((int)orientation) + n) % Count);
        }

        public static Orientation GetMirrorOrientation(Orientation orientation)
        {
            return NewOrientation(orientation, 2);
        }
    }
}
