using GlobalKeyInterceptor.Enum;
using System;

namespace GlobalKeyInterceptor.Model
{
    public class KeyHookedEventArgs : EventArgs
    {
        public ConsoleKey Key { get; }
        public KeyModifier Modifier { get; }

        public KeyHookedEventArgs(ConsoleKey key, KeyModifier modifier)
        {
            Key = key;
            Modifier = modifier;
        }
    }
}
