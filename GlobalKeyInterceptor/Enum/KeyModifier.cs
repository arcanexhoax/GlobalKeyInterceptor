using System;

namespace GlobalKeyInterceptor.Enum
{
    [Flags]
    public enum KeyModifier
    {
        None = 0,
        Ctrl = 1,
        Alt = 2,
        Shift = 4,
        Win = 8
    }
}
