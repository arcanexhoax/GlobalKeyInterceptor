using GlobalKeyInterceptor.Enum;
using GlobalKeyInterceptor.Model;
using GlobalKeyInterceptor.Native;
using System;

namespace GlobalKeyInterceptor
{
    public class KeyHooker : IDisposable
    {
        private readonly KeyHookerNative _hooker;
        private readonly ConsoleKey[] _hookingKeys;

        /// <summary>
        /// An event that invokes when any of the specified keys was pressed.
        /// </summary>
        public event EventHandler<KeyHookedEventArgs> KeyHooked;

        /// <summary>
        /// A class that intercept specified keys. To receive intercepted keys, use <see cref="KeyHooked"/> event.
        /// </summary>
        /// <param name="hookingKeys">A list of keys that will be intercepted. If parameter is empty, every key will be intercepted.</param>
        /// <remarks> Disposable</remarks>
        public KeyHooker(params ConsoleKey[] hookingKeys)
        {
            _hooker = new KeyHookerNative();
            _hooker.KeyPressed += OnKeyPressed;
            _hookingKeys = hookingKeys;
        }

        private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
        {
            if (e.KeyState != KeyState.KeyDown && e.KeyState != KeyState.SysKeyDown)
                return;

            ConsoleKey key = (ConsoleKey)e.KeyData.VirtualCode;
            
            if (_hookingKeys.Length == 0)
            {
                InvokeKeyHooked(key);
                return;
            }

            foreach (var hook in _hookingKeys)
            {
                if (hook == key)
                {
                    InvokeKeyHooked(hook);
                    break;
                }
            }
        }

        private void InvokeKeyHooked(ConsoleKey key)
        {
            KeyModifier ctrlModifier = KeyModifier.None;
            KeyModifier shiftModifier = KeyModifier.None;
            KeyModifier altModifier = KeyModifier.None;

            bool ctrlPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftCtrl) > 1 ||
                        NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightCtrl) > 1;
            bool shiftPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftShift) > 1 ||
                NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightShift) > 1;
            bool altPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftAlt) > 1 ||
                NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightAlt) > 1;

            if (ctrlPressed) ctrlModifier = KeyModifier.Ctrl;
            if (shiftPressed) shiftModifier = KeyModifier.Shift;
            if (altPressed) altModifier = KeyModifier.Alt;

            KeyHooked?.Invoke(this, new KeyHookedEventArgs(key, ctrlModifier | shiftModifier | altModifier));
            //e.Handled = true;
        }

        public void Dispose()
        {
            _hooker?.Dispose();
        }
    }
}