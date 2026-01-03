using System;

namespace GlobalKeyInterceptor
{
    public class ShortcutPressedEventArgs : EventArgs
    {
        /// <summary>
        /// An intercepted shortcut.
        /// </summary>
        public Shortcut Shortcut { get; }

        /// <summary>
        /// Specifies that the intercepted key will be "eaten", so no other applications will receive it.
        /// </summary>
        /// <remarks>
        /// It won't work for a shortcut as a whole, modifiers CANNOT be handled.
        /// </remarks>
        public bool IsHandled { get; set; }

        public ShortcutPressedEventArgs(Shortcut shortcut)
        {
            Shortcut = shortcut;
        }
    }
}
