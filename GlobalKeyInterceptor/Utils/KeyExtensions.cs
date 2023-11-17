using GlobalKeyInterceptor.Enums;

namespace GlobalKeyInterceptor.Utils
{
    public static class KeyExtensions
    {
        /// <summary>
        /// Check if the specified key is Ctrl (<see cref="Key.Ctrl"/> or <see cref="Key.LeftCtrl"/> of <see cref="Key.RightCtrl"/>).
        /// </summary>
        public static bool IsCtrl(this Key key) => key == Key.Ctrl || key == Key.LeftCtrl || key == Key.RightCtrl;

        /// <summary>
        /// Check if the specified key is Shift (<see cref="Key.Shift"/> or <see cref="Key.LeftShift"/> of <see cref="Key.RightShift"/>).
        /// </summary>
        public static bool IsShift(this Key key) => key == Key.Shift || key == Key.LeftShift || key == Key.RightShift;

        /// <summary>
        /// Check if the specified key is Alt (<see cref="Key.Alt"/> or <see cref="Key.LeftAlt"/> of <see cref="Key.RightAlt"/>).
        /// </summary>
        public static bool IsAlt(this Key key) => key == Key.Alt || key == Key.LeftAlt || key == Key.RightAlt;

        /// <summary>
        /// Check if the specified key is Windows (<see cref="Key.LeftWindows"/> or <see cref="Key.RightWindows"/>).
        /// </summary>
        public static bool IsWin(this Key key) => key == Key.LeftWindows || key == Key.RightWindows;

        internal static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
            nativeKeyState == NativeKeyState.KeyDown || nativeKeyState == NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
    }
}
