using GlobalKeyInterceptor.Native;

namespace GlobalKeyInterceptor.Util
{
    public static class KeyUtils
    {
        public static bool IsCtrlPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftCtrl) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightCtrl) > 1;

        public static bool IsShiftPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftShift) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightShift) > 1;

        public static bool IsAltPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftAlt) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightAlt) > 1;

        public static bool IsWinPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftWin) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightWin) > 1;

        public static bool IsKeyPressed(Key key) => NativeMethods.GetAsyncKeyState((uint)key) > 1;
    }
}
