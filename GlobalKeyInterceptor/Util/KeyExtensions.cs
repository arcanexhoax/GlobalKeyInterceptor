using GlobalKeyInterceptor.Enum;

namespace GlobalKeyInterceptor.Util
{
    public static class KeyExtensions
    {
        public static bool IsCtrl(this Key key) => key == Key.Ctrl || key == Key.LeftCtrl || key == Key.RightCtrl;

        public static bool IsShift(this Key key) => key == Key.Shift || key == Key.LeftShift || key == Key.RightShift;

        public static bool IsAlt(this Key key) => key == Key.Alt || key == Key.LeftAlt || key == Key.RightAlt;

        public static bool IsWin(this Key key) => key == Key.LeftWindows || key == Key.RightWindows;

        internal static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
            nativeKeyState == NativeKeyState.KeyDown || nativeKeyState == NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
    }
}
