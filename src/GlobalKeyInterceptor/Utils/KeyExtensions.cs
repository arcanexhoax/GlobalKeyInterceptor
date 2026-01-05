using GlobalKeyInterceptor.Enums;
using System;

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

        /// <summary> Check if the specified key is a modifier key (Ctrl, Shift, Alt, or Windows). </summary>
        public bool IsModifier => key.IsCtrl || key.IsShift || key.IsAlt || key.IsWin;

        /// <summary> Check if the specified key is a digit (0-9). </summary>
        public bool IsDigit => key is >= Key.D0 and <= Key.D9;

        /// <summary> Check if the specified key is a function key (F1-F24). </summary>
        public bool IsFunctionKey => key is >= Key.F1 and <= Key.F24;

        /// <summary> Check if the specified key is a numpad digit (Num0-Num9). </summary>
        public bool IsNumpadDigit => key is >= Key.Num0 and <= Key.Num9;

        /// <summary>
        /// Check if the specified key is a numpad key (Num0-Num9, NumDecimal, NumEnter etc).
        /// </summary>
        public bool IsNumpadKey => key.IsNumpadDigit ||
            key is Key.NumLock or Key.NumDecimal or Key.NumMultiply or Key.NumAdd or Key.NumSubtract or Key.NumDivide
            or Key.NumEnter or Key.NumDelete or Key.NumInsert or Key.NumHome or Key.NumEnd or Key.NumPageDown
            or Key.NumPageUp or Key.NumLeftArrow or Key.NumUpArrow or Key.NumRightArrow or Key.NumDownArrow;

        /// <summary> Check if the specified key is an arrow key (UpArrow, DownArrow, LeftArrow, RightArrow). </summary>
        public bool IsArrowKey => key.BaseKey is >= Key.LeftArrow and <= Key.DownArrow;

        /// <summary> Check if the specified key is a navigation key (Home, End, Insert, Delete, PageUp, PageDown). </summary>
        public bool IsNavigationKey => key.BaseKey is Key.Home or Key.End or Key.Insert or Key.Delete or Key.PageUp or Key.PageDown;

        /// <summary> Check if the specified key is a letter (A-Z). </summary>
        public bool IsLetter => key is >= Key.A and <= Key.Z;

        /// <summary> Check if the specified key is a character key (letters, digits, numpad keys, and some punctuation). </summary>
        public bool IsCharacterKey => key.IsLetter || key.IsDigit || key.IsNumpadKey
            || key is Key.Space or Key.Minus or Key.Plus or Key.Period or Key.Comma or Key.Colon
            or Key.Slash or Key.Tilde or Key.OpenBracket or Key.BackSlash or Key.ClosingBracket or Key.Quote;

        /// <summary> 
        /// Gets the base virtual key code by stripping any custom bit offsets for <b>Standard-</b> and some <b>Num-</b> keys. 
        /// <br/>For example base key of <see cref="Key.StandardEnter"/> and <see cref="Key.NumEnter"/> is <see cref="Key.Enter"/>
        /// </summary>
        /// <remarks> It has no effect on system defined keys like <see cref="Key.Enter"/> including all modifier keys (<see cref="Key.Ctrl"/>, 
        /// <see cref="Key.LeftAlt"/>, <see cref="Key.RightWindows"/> etc)
        /// </remarks>
        public Key BaseKey => (Key)((int)key & 0xFF);

        /// <summary> Get the formatted string representation of the key. </summary>
        public string ToFormattedString()
        {
            var keyStr = key.ToString();

            if (key.IsDigit)
                return keyStr.Substring(1); // D0 -> 0

            return key switch
            {
                Key.Colon => ";",
                Key.Plus => "=",
                Key.Comma => ",",
                Key.Minus => "-",
                Key.Period => ".",
                Key.Slash => "/",
                Key.Tilde => "`",
                Key.OpenBracket => "[",
                Key.BackSlash => "\\",
                Key.ClosingBracket => "]",
                Key.Quote => "'",
                Key.StandardEnter => "Enter",
                Key.StandardDelete => "Delete",
                Key.StandardInsert => "Insert",
                Key.StandardHome => "Home",
                Key.StandardEnd => "End",
                Key.StandardPageDown => "PageDown",
                Key.StandardPageUp => "PageUp",
                Key.StandardLeftArrow => "LeftArrow",
                Key.StandardUpArrow => "UpArrow",
                Key.StandardRightArrow => "RightArrow",
                Key.StandardDownArrow => "DownArrow",
                _ => keyStr
            };
        }

        /// <summary> Converts the string representation of a key to the <see cref="Key"/> value. </summary>
        /// <param name="keyStr">A string representation of a key</param>
        /// <returns> The result value of the conversion </returns>
        /// <exception cref="ArgumentException"/>
        public static Key FormattedParse(string keyStr)
        {
            if (!TryFormattedParse(keyStr, out var value))
                throw new ArgumentException($"Failed to parse {keyStr}");

            return value;
        }

        /// <summary> Try to convert the string representation of a key to the <see cref="Key"/> value. </summary>
        /// <param name="keyStr"> A string representation of the key. </param>
        /// <param name="value"> The result value of the conversion. </param>
        /// <returns> true if <paramref name="keyStr"/> was converted successfully; otherwise, false. </returns>
        public static bool TryFormattedParse(string keyStr, out Key value)
        {
            value = keyStr switch
            {
                "1" => Key.D1,
                "2" => Key.D2,
                "3" => Key.D3,
                "4" => Key.D4,
                "5" => Key.D5,
                "6" => Key.D6,
                "7" => Key.D7,
                "8" => Key.D8,
                "9" => Key.D9,
                "0" => Key.D0,
                ";" or ":" => Key.Colon,
                "=" or "+" => Key.Plus,
                "," or "<" => Key.Comma,
                "-" or "_" => Key.Minus,
                "." or ">" => Key.Period,
                "/" or "?" => Key.Slash,
                "`" or "~" => Key.Tilde,
                "[" or "{" => Key.OpenBracket,
                "\\" or "|" => Key.BackSlash,
                "]" or "}" => Key.ClosingBracket,
                "'" or "\"" => Key.Quote,
                _ => default
            };  

            if (value != default)
                return true;
            if (Enum.TryParse(keyStr, true, out value))
                return true;

            return false;
        }
    }

    internal static KeyState ToKeyState(this NativeKeyState nativeKeyState) =>
        nativeKeyState is NativeKeyState.KeyDown or NativeKeyState.SysKeyDown ? KeyState.Down : KeyState.Up;
}
