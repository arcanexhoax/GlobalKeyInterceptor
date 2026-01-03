using System;
using System.Runtime.InteropServices;

namespace GlobalKeyInterceptor.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct MSG
{
    IntPtr hwnd;
    uint message;
    UIntPtr wParam;
    IntPtr lParam;
    int time;
    POINT pt;
    int lPrivate;
}
