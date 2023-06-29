using GlobalKeyInterceptor.Enum;
using System;

namespace GlobalKeyInterceptor.Model
{
    public class KeyHookedEventArgs : EventArgs
    {
        /// <summary>Intercepted key.</summary>
        /// <remarks>To get VK code of the key use <see cref="int"/> casting.</remarks>
        public ConsoleKey Key { get; }

        /// <summary>A modifier of the intercepted shortcut.</summary>
        /// <remarks>
        /// If modifier isn't present, the value <see cref="KeyModifier.None"/> is set.
        /// <br/>The value can contain multiple modifiers, to check if modifier is intercepted, use <see cref="System.Enum.HasFlag(System.Enum)"/>.
        /// <code>bool isCtrlIntercepted = e.Modifier.HasFlag(KeyModifier.Ctrl);</code>
        /// </remarks>
        public KeyModifier Modifier { get; }

        /// <summary>Specifies that the intercepted key will be "eaten", so no other applications will receive it.</summary>
        /// <remarks>It won't work for a shortcut as a whole, modifiers CANNOT be handled.</remarks>
        public bool IsHandled { get; set; }

        public KeyHookedEventArgs(ConsoleKey key, KeyModifier modifier)
        {
            Key = key;
            Modifier = modifier;
        }
    }
}
