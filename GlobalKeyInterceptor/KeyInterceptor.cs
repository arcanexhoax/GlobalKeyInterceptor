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
        private readonly Dictionary<Shortcut, HashSet<Func<bool>>> _shortcuts;

        private bool _disposed;
        // TODO remove after 2.0 release
        private bool _usedObsoleteConstructor;

        /// <summary>
        /// An event that invokes when any keys/shortcuts was pressed.
        /// </summary>
        public event EventHandler<ShortcutPressedEventArgs> ShortcutPressed;

        /// <summary>
        /// Creates an interceptor instance. To intercept specific key/shortcuts use <see cref="RegisterShortcut(Shortcut, Func{bool})"/>. 
        /// To receive all keys/shortcuts, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        public KeyInterceptor()
        {
            _interceptor = new NativeKeyInterceptor();
            _interceptor.KeyPressed += OnKeyPressed;
            _shortcuts = [];
        }

        /// <summary>
        /// Creates an instance that intercepts specified keys/shortcuts. To receive intercepted keys/shortcuts, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        /// <param name="interceptingShortcuts">A list of keys/shortcuts that will be intercepted.</param>
        [Obsolete("Use empty constructor with RegisterShortcut() method instead. Starting from version 2.0 this constructor will be removed.", false)]
        public KeyInterceptor(IEnumerable<Shortcut> interceptingShortcuts)
        {
            _usedObsoleteConstructor = true;
            _interceptor = new NativeKeyInterceptor();
            _interceptor.KeyPressed += OnKeyPressed;
            _shortcuts = interceptingShortcuts.ToDictionary(
                s => s,
                s => new HashSet<Func<bool>>()
                {
                    () =>
                    {
                        var args = new ShortcutPressedEventArgs(s);
                        ShortcutPressed?.Invoke(this, args);
                        return args.IsHandled;
                    }
                });
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

        /// <summary>
        /// Registers a shortcut to intercept. If the shortcut already registered, the handler will be added to the existing one.
        /// <param name="shortcut">An intercepting shortcut</param>
        /// <param name="handler">A callback that will be invoked when the shortcut is pressed. Return true if you want to "eat" the pressed key.</param>"
        /// </summary>
        public void RegisterShortcut(Shortcut shortcut, Func<bool> handler)
        {
            if (_shortcuts.TryGetValue(shortcut, out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                _shortcuts[shortcut] = [handler];
            }
        }

        /// <summary>
        /// Registers a shortcut to intercept. If the shortcut already registered, the handler will be added to the existing one.
        /// <param name="shortcut">An intercepting shortcut</param>
        /// <param name="handler">A callback that will be invoked when the shortcut is pressed.</param>
        /// <param name="handled">If true, the pressed shortcut will be "eaten" and not passed to the system.</param>
        /// </summary>
        public void RegisterShortcut(Shortcut shortcut, Action handler, bool handled = false)
        {
            RegisterShortcut(shortcut, () =>
            {
                handler();
                return handled;
            });
        }

        /// <summary>
        /// Unregisters a specific shortcut handler.
        /// <param name="shortcut">The shortcut to unregister.</param>
        /// <param name="handler">The specific callback that was registered for the shortcut.</param>
        /// </summary>
        public void UnregisterShortcut(Shortcut shortcut, Func<bool> handler)
        {
            if (_shortcuts.TryGetValue(shortcut, out var handlers))
            {
                handlers.Remove(handler);

                if (handlers.Count == 0)
                    _shortcuts.Remove(shortcut);
            }
        }

        /// <summary>
        /// Unregister all handlers for the specified shortcut.
        /// <param name="shortcut">The shortcut to unregister with all registered handlers</param>
        /// </summary>
        public void UnregisterShortcut(Shortcut shortcut) => _shortcuts.Remove(shortcut);

        /// <summary>
        /// Unregisters all shortcuts and their handlers.
        /// </summary>
        public void UnregisterAllShortcuts() => _shortcuts.Clear();

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

            foreach (var scKeyValue in _shortcuts)
            {
                var sc = scKeyValue.Key;

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

                        foreach (var handler in scKeyValue.Value)
                            e.Handled = handler();

                        break;
                    }
                }
            }

            if (_usedObsoleteConstructor)
                return;

            if (shortcut == null)
            {
                KeyModifier ctrlModifier = ctrlModifierPressed ? KeyModifier.Ctrl : KeyModifier.None;
                KeyModifier shiftModifier = shiftModifierPressed ? KeyModifier.Shift : KeyModifier.None;
                KeyModifier altModifier = altModifierPressed ? KeyModifier.Alt : KeyModifier.None;
                KeyModifier winModifier = winModifierPressed ? KeyModifier.Win : KeyModifier.None;

                shortcut = new Shortcut(pressedKey, ctrlModifier | shiftModifier | altModifier | winModifier, state);
            }

            var keyHookedEventArgs = new ShortcutPressedEventArgs(shortcut);
            ShortcutPressed?.Invoke(this, keyHookedEventArgs);
            e.Handled = keyHookedEventArgs.IsHandled;
        }

        public void Dispose()
        {
            if (_interceptor != null && !_disposed)
            {
                _interceptor.KeyPressed -= OnKeyPressed;
                _interceptor.Dispose();
                _disposed = true;

                ShortcutPressed = null;
            }
        }

        ~KeyInterceptor() => Dispose();
    }
}