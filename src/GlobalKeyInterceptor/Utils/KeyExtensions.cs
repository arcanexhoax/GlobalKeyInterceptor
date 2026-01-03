using GlobalKeyInterceptor.Enums;

namespace GlobalKeyInterceptor.Utils;

public static class KeyExtensions
{
    extension(Key key)
    {
        /// <summary>
        /// Check if the specified key is Ctrl (<see cref="Key.Ctrl"/> or <see cref="Key.LeftCtrl"/> of <see cref="Key.RightCtrl"/>).
        /// </summary>
        public bool IsCtrl => key is Key.Ctrl or Key.LeftCtrl or Key.RightCtrl;

        /// <summary>
        /// Check if the specified key is Shift (<see cref="Key.Shift"/> or <see cref="Key.LeftShift"/> of <see cref="Key.RightShift"/>).
        /// </summary>
        public bool IsShift => key is Key.Shift or Key.LeftShift or Key.RightShift;

        /// <summary>
        /// Check if the specified key is Alt (<see cref="Key.Alt"/> or <see cref="Key.LeftAlt"/> of <see cref="Key.RightAlt"/>).
        /// </summary>
        public bool IsAlt => key is Key.Alt or Key.LeftAlt or Key.RightAlt;

        /// <summary>
        /// Check if the specified key is Windows (<see cref="Key.LeftWindows"/> or <see cref="Key.RightWindows"/>).
        /// </summary>
        public bool IsWin => key is Key.LeftWindows or Key.RightWindows;

        /// <summary>
        /// Check if the specified key is a modifier key (Ctrl, Shift, Alt, or Windows).
        /// </summary>
        public bool IsModifier => key.IsCtrl || key.IsShift || key.IsAlt || key.IsWin;

        /// <summary>
        /// Check if the specified key is a digit (0-9).
        /// </summary>
        public bool IsDigit => key is >= Key.D0 and <= Key.D9;

        /// <summary>
        /// Check if the specified key is a function key (F1-F24).
        /// </summary>
        public bool IsFunctionKey => key is >= Key.F1 and <= Key.F24;

        /// <summary>
        /// Check if the specified key is a numpad digit (Num0-Num9).
        /// </summary>
        public bool IsNumpadDigit => key is >= Key.Num0 and <= Key.Num9;

        /// <summary>
        /// Check if the specified key is a numpad key (Num0-Num9, NumDecimal, NumMultiply, NumAdd, NumSubtract, NumDivide, NumEnter).
        /// </summary>
        public bool IsNumpadKey => key.IsNumpadDigit ||
            key is Key.NumDecimal or Key.NumMultiply or Key.NumAdd or Key.NumSubtract or Key.NumDivide;

        /// <summary>
        /// Check if the specified key is an arrow key (UpArrow, DownArrow, LeftArrow, RightArrow).
        /// </summary>
        public bool IsArrowKey => key is Key.UpArrow or Key.DownArrow or Key.LeftArrow or Key.RightArrow;

        /// <summary>
        /// Check if the specified key is a navigation key (Home, End, Insert, Delete, PageUp, PageDown).
        /// </summary>
        public bool IsNavigationKey => key is Key.Home or Key.End or Key.Insert or Key.Delete or Key.PageUp or Key.PageDown;

        /// <summary>
        /// Check if the specified key is a letter (A-Z).
        /// </summary>
        public bool IsLetter => key is >= Key.A and <= Key.Z;

        /// <summary>
        /// Check if the specified key is a character key (letters, digits, numpad keys, and some punctuation).
        /// </summary>
        public bool IsCharacterKey => key.IsLetter || key.IsDigit || key.IsNumpadKey
            || key is Key.Space or Key.Minus or Key.Plus or Key.Period or Key.Comma or Key.Colon
            or Key.Slash or Key.Tilde or Key.OpenBracket or Key.BackSlash or Key.ClosingBracket or Key.Quote;
    }

    extension (KeyModifier modifier)
    {
        /// <summary>
        /// Check if the specified modifier contains Ctrl.
        /// </summary>
        public bool HasCtrl => modifier.HasFlag(KeyModifier.Ctrl);

        /// <summary>
        /// Check if the specified modifier contains Shift.
        /// </summary>
        public bool HasShift => modifier.HasFlag(KeyModifier.Shift);

        /// <summary>
        /// Check if the specified modifier contains Alt.
        /// </summary>
        public bool HasAlt => modifier.HasFlag(KeyModifier.Alt);

        /// <summary>
        /// Check if the specified modifier contains Windows.
        /// </summary>
        public bool HasWin => modifier.HasFlag(KeyModifier.Win);
    }

    internal static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
        nativeKeyState is NativeKeyState.KeyDown or NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
}
