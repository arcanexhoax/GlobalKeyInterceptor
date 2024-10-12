using GlobalKeyInterceptor.Model;
using GlobalKeyInterceptor.Native;
using GlobalKeyInterceptor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GlobalKeyInterceptor
{
    /// <summary>
    /// A class that allows you to intercept the specified or every keystroke/shortcut in the system
    /// </summary>
    public class KeyInterceptor : IDisposable
    {
        private readonly NativeKeyInterceptor _interceptor;
        private readonly IEnumerable<Shortcut> _interceptingShortcuts;

        private bool _disposed;

        /// <summary>
        /// An event that invokes when any of the specified keys/shortcuts was pressed.
        /// </summary>
        public event EventHandler<ShortcutPressedEventArgs> ShortcutPressed;

        /// <summary>
        /// Creates an instance that intercepts all keys/shortcuts. To receive intercepted keys/shortcuts, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        public KeyInterceptor() : this(Enumerable.Empty<Shortcut>()) { }

        /// <summary>
        /// Creates an instance that intercepts specified keys/shortcuts. To receive intercepted keys/shortcuts, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        /// <param name="interceptingShortcuts">A list of keys/shortcuts that will be intercepted. If parameter is empty, every key/shortcut will be intercepted.</param>
        public KeyInterceptor(IEnumerable<Shortcut> interceptingShortcuts)
        {
            _interceptor = new NativeKeyInterceptor();
            _interceptor.KeyPressed += OnKeyPressed;
            _interceptingShortcuts = interceptingShortcuts ?? Enumerable.Empty<Shortcut>();
        }

        /// <summary>
        /// Run a message loop that allows you to intercept keys in Console applications.
        /// <br/>WPF/WinForms applications have their own message loop, so there is no need to use this method in such applications.
        /// </summary>
        public void RunMessageLoop()
        {
            while (true)
            {
                var result = NativeMethods.GetMessage(out var msg, IntPtr.Zero, 0, 0);

                if (result > 0)
                {
                    NativeMethods.TranslateMessage(ref msg);
                    NativeMethods.DispatchMessage(ref msg);
                }
            }
        }

        private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
        {
            KeyState state = e.KeyState.ToKeyState();
            Key pressedKey = (Key)e.KeyData.VirtualCode;
            Shortcut shortcut = null;

            Debug.WriteLine($"Key {pressedKey}. State: {state}");

            // If a modifier specified as a key, then we ignore it as a modifier
            bool ctrlModifierPressed = !pressedKey.IsCtrl() && KeyUtils.IsCtrlPressed();
            bool shiftModifierPressed = !pressedKey.IsShift() && KeyUtils.IsShiftPressed();
            bool altModifierPressed = !pressedKey.IsAlt() && KeyUtils.IsAltPressed();
            bool winModifierPressed = !pressedKey.IsWin() && KeyUtils.IsWinPressed();

            if (!_interceptingShortcuts.Any())
            {
                KeyModifier ctrlModifier = ctrlModifierPressed ? KeyModifier.Ctrl : KeyModifier.None;
                KeyModifier shiftModifier = shiftModifierPressed ? KeyModifier.Shift : KeyModifier.None;
                KeyModifier altModifier = altModifierPressed ? KeyModifier.Alt : KeyModifier.None;
                KeyModifier winModifier = winModifierPressed ? KeyModifier.Win : KeyModifier.None;

                shortcut = new Shortcut(pressedKey, ctrlModifier | shiftModifier | altModifier | winModifier);
            }
            else
            {
                foreach (var sc in _interceptingShortcuts)
                {
                    if ((sc.Key == Key.Ctrl && pressedKey.IsCtrl() ||
                        sc.Key == Key.Shift && pressedKey.IsShift() ||
                        sc.Key == Key.Alt && pressedKey.IsAlt() ||
                        sc.Key == pressedKey) && 
                        sc.State == state)
                    {
                        bool isCtrlHooking = sc.Modifier.HasFlag(KeyModifier.Ctrl);
                        bool isShiftHooking = sc.Modifier.HasFlag(KeyModifier.Shift);
                        bool isAltHooking = sc.Modifier.HasFlag(KeyModifier.Alt);
                        bool isWinHooking = sc.Modifier.HasFlag(KeyModifier.Win);

                        if (isCtrlHooking == ctrlModifierPressed && 
                            isShiftHooking == shiftModifierPressed && 
                            isAltHooking == altModifierPressed && 
                            isWinHooking == winModifierPressed)
                        {
                            shortcut = sc;
                            break;
                        }
                    }
                }
            }

            if (shortcut != null)
            {
                var keyHookedEventArgs = new ShortcutPressedEventArgs(shortcut);
                ShortcutPressed?.Invoke(this, keyHookedEventArgs);
                e.Handled = keyHookedEventArgs.IsHandled;
            }
        }

        public void Dispose()
        {
            if (_interceptor != null && !_disposed)
            {
                _interceptor.KeyPressed -= OnKeyPressed;
                _interceptor.Dispose();
                _disposed = true;
            }
        }

        ~KeyInterceptor() => Dispose();
    }
}