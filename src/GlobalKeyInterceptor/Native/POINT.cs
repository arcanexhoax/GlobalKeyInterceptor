using System.Drawing;
using System.Runtime.InteropServices;

namespace GlobalKeyInterceptor.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct POINT(int x, int y)
{
    public int X = x;
    public int Y = y;

    public static implicit operator Point(POINT p)
    {
        return new Point(p.X, p.Y);
    }

    public static implicit operator POINT(Point p)
    {
        return new POINT(p.X, p.Y);
    }

    public override readonly string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
}
