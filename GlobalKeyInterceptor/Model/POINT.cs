﻿using System.Drawing;
using System.Runtime.InteropServices;

namespace GlobalKeyInterceptor.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT(Point p)
        {
            return new POINT(p.X, p.Y);
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
