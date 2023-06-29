using GlobalKeyInterceptor.Enum;
using GlobalKeyInterceptor.Model;
using GlobalKeyInterceptor.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalKeyInterceptor
{
    public class KeyHooker : IDisposable
    {
        private readonly KeyHookerNative _hooker;
        private readonly IEnumerable<Shortcut> _hookingShortcuts;

        /// <summary>
        /// An event that invokes when any of the specified keys was pressed.
        /// </summary>
        public event EventHandler<KeyHookedEventArgs> KeyHooked;

        /// <summary>
        /// A class that intercept specified keys. To receive intercepted keys, use <see cref="KeyHooked"/> event.
        /// </summary>
        public KeyHooker() : this(Enumerable.Empty<Shortcut>()) { }

        /// <summary>
        /// A class that intercept specified keys. To receive intercepted keys, use <see cref="KeyHooked"/> event.
        /// </summary>
        /// <param name="hookingShortcuts">A list of keys that will be intercepted. If parameter is empty, every key will be intercepted.</param>
        public KeyHooker(IEnumerable<Shortcut> hookingShortcuts)
        {
            _hooker = new KeyHookerNative();
            _hooker.KeyPressed += OnKeyPressed;
            _hookingShortcuts = hookingShortcuts ?? Enumerable.Empty<Shortcut>();
        }

        private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
        {
            if (e.KeyState != KeyState.KeyDown && e.KeyState != KeyState.SysKeyDown)
                return;

            ConsoleKey key = (ConsoleKey)e.KeyData.VirtualCode;
            Shortcut shortcut = null;

            bool ctrlPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftCtrl) > 1 ||
                NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightCtrl) > 1;
            bool shiftPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftShift) > 1 ||
                NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightShift) > 1;
            bool altPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftAlt) > 1 ||
                NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightAlt) > 1;

            if (!_hookingShortcuts.Any())
            {
                KeyModifier ctrlModifier = ctrlPressed ? KeyModifier.Ctrl : KeyModifier.None;
                KeyModifier shiftModifier = shiftPressed ? KeyModifier.Shift : KeyModifier.None;
                KeyModifier altModifier = altPressed ? KeyModifier.Alt : KeyModifier.None;

                shortcut = new Shortcut(key, ctrlModifier | shiftModifier | altModifier);
            }
            else
            {
                foreach (var sc in _hookingShortcuts)
                {
                    if (sc.Key != key)
                        continue;

                    bool isCtrlHooking = sc.Modifier.HasFlag(KeyModifier.Ctrl);
                    bool isShiftHooking = sc.Modifier.HasFlag(KeyModifier.Shift);
                    bool isAltHooking = sc.Modifier.HasFlag(KeyModifier.Alt);

                    if (isCtrlHooking == ctrlPressed && isShiftHooking == shiftPressed && isAltHooking == altPressed)
                    {
                        shortcut = sc;
                        break;
                    }
                }
            }

            if (shortcut != null)
            {
                var keyHookedEventArgs = new KeyHookedEventArgs(shortcut);
                KeyHooked?.Invoke(this, keyHookedEventArgs);
                e.Handled = keyHookedEventArgs.IsHandled;
            }
        }

        public void Dispose()
        {
            if (_hooker != null)
            {
                _hooker.KeyPressed -= OnKeyPressed;
                _hooker.Dispose();
            }
        }

        ~KeyHooker() => Dispose();
    }
}