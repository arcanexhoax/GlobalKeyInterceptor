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

        /// <summary>
        /// Check if the specified key is a modifier key (Ctrl, Shift, Alt, or Windows).
        /// </summary>
        public static bool IsModifier(this Key key) => key.IsCtrl() || key.IsShift() || key.IsAlt() || key.IsWin();

        /// <summary>
        /// Check if the specified key is a digit (0-9).
        /// </summary>
        public static bool IsDigit(this Key key) => key >= Key.D0 && key <= Key.D9;

        /// <summary>
        /// Check if the specified key is a function key (F1-F24).
        /// </summary>
        public static bool IsFunctionKey(this Key key) => key >= Key.F1 && key <= Key.F24;

        /// <summary>
        /// Check if the specified key is a numpad digit (Num0-Num9).
        /// </summary>
        public static bool IsNumpadDigit(this Key key) => key >= Key.Num0 && key <= Key.Num9;

        /// <summary>
        /// Check if the specified key is a numpad key (Num0-Num9, NumDecimal, NumMultiply, NumAdd, NumSubtract, NumDivide, NumEnter).
        /// </summary>
        public static bool IsNumpadKey(this Key key) => key.IsNumpadDigit() ||
            key == Key.NumDecimal || key == Key.NumMultiply || key == Key.NumAdd ||
            key == Key.NumSubtract || key == Key.NumDivide;

        /// <summary>
        /// Check if the specified key is an arrow key (UpArrow, DownArrow, LeftArrow, RightArrow).
        /// </summary>
        public static bool IsArrowKey(this Key key) => key == Key.UpArrow || key == Key.DownArrow || key == Key.LeftArrow || key == Key.RightArrow;

        /// <summary>
        /// Check if the specified key is a navigation key (Home, End, Insert, Delete, PageUp, PageDown).
        /// </summary>
        public static bool IsNavigationKey(this Key key) => key == Key.Home || key == Key.End || 
            key == Key.Insert || key == Key.Delete || key == Key.PageUp || key == Key.PageDown;

        /// <summary>
        /// Check if the specified key is a letter (A-Z).
        /// </summary>
        public static bool IsLetter(this Key key) => key >= Key.A && key <= Key.Z;

        /// <summary>
        /// Check if the specified key is a character key (letters, digits, numpad keys, and some punctuation).
        /// </summary>
        public static bool IsCharacterKey(this Key key) =>
            key.IsLetter() || key.IsDigit() || key.IsNumpadKey() || 
            key == Key.Space || key == Key.Minus || key == Key.Plus ||
            key == Key.Period || key == Key.Comma || key == Key.Colon ||
            key == Key.Slash || key == Key.Tilde || key == Key.OpenBracket ||
            key == Key.BackSlash || key == Key.ClosingBracket || key == Key.Quote;

        internal static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
            nativeKeyState == NativeKeyState.KeyDown || nativeKeyState == NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
    }
}
