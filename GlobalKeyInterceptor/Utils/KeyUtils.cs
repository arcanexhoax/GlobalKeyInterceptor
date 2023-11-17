using GlobalKeyInterceptor.Native;

namespace GlobalKeyInterceptor.Utils
{
    public static class KeyUtils
    {
        /// <summary>
        /// Check if Ctrl (left of right) key is pressed
        /// </summary>
        public static bool IsCtrlPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftCtrl) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightCtrl) > 1;

        /// <summary>
        /// Check if Shift (left of right) key is pressed
        /// </summary>
        public static bool IsShiftPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftShift) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightShift) > 1;

        /// <summary>
        /// Check if Alt (left of right) key is pressed
        /// </summary>
        public static bool IsAltPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftAlt) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightAlt) > 1;

        /// <summary>
        /// Check if Windows (left of right) key is pressed
        /// </summary>
        public static bool IsWinPressed() => NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkLeftWin) > 1 ||
            NativeMethods.GetAsyncKeyState(NativeKeyInterceptor.VkRightWin) > 1;

        /// <summary>
        /// Check if the specified key is pressed
        /// </summary>
        public static bool IsKeyPressed(Key key) => NativeMethods.GetAsyncKeyState((uint)key) > 1;
    }
}
