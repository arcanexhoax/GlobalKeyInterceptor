﻿using GlobalKeyInterceptor.Utils;
using System.Text;

namespace GlobalKeyInterceptor
{
    /// <summary>
    /// A class that represents an intercepted keystroke/shortcut in the system
    /// </summary>
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
        /// The value can contain multiple modifiers, to check if modifier is intercepted, use <see cref="System.Enum.HasFlag(System.Enum)"/>.
        /// <code>bool isCtrlIntercepted = e.Modifier.HasFlag(KeyModifier.Ctrl);</code>
        public KeyModifier Modifier { get; }

        /// <summary>
        /// Intercepted state of the specified key.
        /// </summary>
        /// <remarks>
        /// It does not apply to modifiers. Default value is <see cref="KeyState.Up"/>
        /// </remarks>
        public KeyState State { get; }

        /// <summary>
        /// A name of the shortcut.
        /// </summary>
        public string Name { get; }

        /// <param name="key">Intercepted key. If the key specified as Ctrl/Shift/Alt/Win, then the corresponding modifier will be ignored.</param>
        /// <param name="modifier">A modifier of the intercepted shortcut. Use "|" to set multiple modifiers.</param>
        /// <param name="state">Intercepted state of the specified key.</param>
        /// <param name="name">A name of the shortcut.</param>
        public Shortcut(Key key, KeyModifier modifier = KeyModifier.None, KeyState state = KeyState.Up, string name = null)
        {
            if (key.IsCtrl() && modifier.HasFlag(KeyModifier.Ctrl))
                modifier -= KeyModifier.Ctrl;
            if (key.IsShift() && modifier.HasFlag(KeyModifier.Shift))
                modifier -= KeyModifier.Shift;
            if (key.IsAlt() && modifier.HasFlag(KeyModifier.Alt))
                modifier -= KeyModifier.Alt;
            if (key.IsWin() && modifier.HasFlag(KeyModifier.Win))
                modifier -= KeyModifier.Win;

            Key = key;
            Modifier = modifier;
            State = state;
            Name = name;
        }

        public override string ToString()
        {
            StringBuilder modifiersBuilder = new();

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

        public override bool Equals(object obj)
        {
            if (obj is Shortcut shortcut)
            {
                return Key == shortcut.Key &&
                       Modifier == shortcut.Modifier &&
                       State == shortcut.State;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Key.GetHashCode();
                hash = hash * 23 + Modifier.GetHashCode();
                hash = hash * 23 + State.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Shortcut left, Shortcut right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Shortcut left, Shortcut right)
        {
            return !(left == right);
        }
    }
}
