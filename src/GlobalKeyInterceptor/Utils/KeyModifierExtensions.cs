using System;

namespace GlobalKeyInterceptor.Utils;

public static class KeyModifierExtensions
{
    extension(KeyModifier modifier)
    {
        /// <summary> Check if the specified modifier contains Ctrl. </summary>
        public bool HasCtrl => modifier.HasFlag(KeyModifier.Ctrl);

        /// <summary> Check if the specified modifier contains Shift. </summary>
        public bool HasShift => modifier.HasFlag(KeyModifier.Shift);

        /// <summary> Check if the specified modifier contains Alt. </summary>
        public bool HasAlt => modifier.HasFlag(KeyModifier.Alt);

        /// <summary> Check if the specified modifier contains Windows. </summary>
        public bool HasWin => modifier.HasFlag(KeyModifier.Win);

        /// <summary> Converts the string representation of a key modifier to the <see cref="KeyModifier"/> value. </summary>
        /// <param name="modifierStr">A string representation of a key modifier</param>
        /// <returns> The result value of the conversion </returns>
        /// <exception cref="ArgumentException"/>
        public static KeyModifier FormattedParse(string modifierStr)
        {
            if (!TryFormattedParse(modifierStr, out var value))
                throw new ArgumentException($"Failed to parse {modifierStr}");

            return value;
        }

        /// <summary> Try to convert the string representation of a key modifier to the <see cref="KeyModifier"/> value. </summary>
        /// <param name="modifierStr"> A string representation of the modifier. </param>
        /// <param name="value"> The result value of the conversion. </param>
        /// <returns> true if <paramref name="modifierStr"/> was converted successfully; otherwise, false. </returns>
        public static bool TryFormattedParse(string modifierStr, out KeyModifier value)
        {
            if (Enum.TryParse(modifierStr, true, out value) && value != KeyModifier.None)
                return true;

            value = modifierStr.ToLowerInvariant() switch
            {
                "control" => KeyModifier.Ctrl,
                "menu" => KeyModifier.Alt,
                "windows" => KeyModifier.Win,
                _ => KeyModifier.None
            };

            return value != KeyModifier.None;
        }
    }
}
