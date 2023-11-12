using GlobalKeyInterceptor.Enum;
using GlobalKeyInterceptor.Native;

namespace GlobalKeyInterceptor.Util
{
    internal static class KeyExtensions
    {
        public static bool IsCtrl(this Key key) => key == Key.Ctrl || key == Key.LeftCtrl || key == Key.RightCtrl;

        public static bool IsShift(this Key key) => key == Key.Shift || key == Key.LeftShift || key == Key.RightShift;

        public static bool IsAlt(this Key key) => key == Key.Alt || key == Key.LeftAlt || key == Key.RightAlt;

        public static bool IsWin(this Key key) => key == Key.LeftWindows || key == Key.RightWindows;

        public static bool IsCtrlPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftCtrl) > 1 ||
                NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightCtrl) > 1;

        public static bool IsShiftPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftShift) > 1 ||
                NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightShift) > 1;

        public static bool IsAltPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftAlt) > 1 ||
                NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightAlt) > 1;

        public static bool IsWinPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftWin) > 1 ||
                NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightWin) > 1;

        public static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
            nativeKeyState == NativeKeyState.KeyDown || nativeKeyState == NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
    }
}
