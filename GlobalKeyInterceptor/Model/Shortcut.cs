﻿using GlobalKeyInterceptor.Enum;
using GlobalKeyInterceptor.Util;
using System.Text;

namespace GlobalKeyInterceptor.Model
{
    public class Shortcut
    {
        /// <summary>
        /// Intercepted key.
        /// </summary>
        /// <remarks>
        /// To get VK code of the key use <see cref="int"/> casting.
        /// </remarks>
        public Key Key { get; }

        /// <summary>
        /// A modifier of the intercepted shortcut.
        /// </summary>
        /// <remarks>
        /// If modifier isn't present, the value <see cref="KeyModifier.None"/> is set.
        /// <br/>The value can contain multiple modifiers, to check if modifier is intercepted, use <see cref="System.Enum.HasFlag(System.Enum)"/>.
        /// <code>bool isCtrlIntercepted = e.Modifier.HasFlag(KeyModifier.Ctrl);</code>
        public KeyModifier Modifier { get; }

        /// <summary>
        /// A name of the shortcut.
        /// </summary>
        public string Name { get; }

        /// <param name="key">Intercepted key. If the key specified as Ctrl/Shift/Alt/Win, then the corresponding modifier will be ignored.</param>
        /// <param name="modifier">A modifier of the intercepted shortcut. Use "|" to set multiple modifiers.</param>
        /// <param name="name">A name of the shortcut.</param>
        public Shortcut(Key key, KeyModifier modifier = KeyModifier.None, string name = null)
        {
            if (KeyUtil.IsCtrl(key) && modifier.HasFlag(KeyModifier.Ctrl))
                modifier -= KeyModifier.Ctrl;
            if (KeyUtil.IsShift(key) && modifier.HasFlag(KeyModifier.Shift))
                modifier -= KeyModifier.Shift;
            if (KeyUtil.IsAlt(key) && modifier.HasFlag(KeyModifier.Alt))
                modifier -= KeyModifier.Alt;
            if (KeyUtil.IsWin(key) && modifier.HasFlag(KeyModifier.Win))
                modifier -= KeyModifier.Win;

            Key = key;
            Modifier = modifier;
            Name = name;
        }

        public override string ToString()
        {
            StringBuilder modifiersBuilder = new StringBuilder();

            if (Modifier.HasFlag(KeyModifier.Ctrl))
                modifiersBuilder.Append("Ctrl + ");
            if (Modifier.HasFlag(KeyModifier.Shift))
                modifiersBuilder.Append("Shift + ");
            if (Modifier.HasFlag(KeyModifier.Alt))
                modifiersBuilder.Append("Alt + ");
            if (Modifier.HasFlag(KeyModifier.Win))
                modifiersBuilder.Append("Win + ");

            modifiersBuilder.Append(Key.ToString());

            return string.IsNullOrEmpty(Name) ? modifiersBuilder.ToString() : $"{Name} ({modifiersBuilder})";
        }
    }
}
