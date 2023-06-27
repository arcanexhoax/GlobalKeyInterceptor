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

        public event EventHandler<KeyHookedEventArgs> KeyHooked;

        public KeyHooker(params ConsoleKey[] hookingKeys)
        {
            _hooker = new KeyHookerNative();
            _hooker.KeyPressed += OnKeyPressed;
            _hookingKeys = hookingKeys;
        }

        private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
        {
            if (e.KeyboardState != KeyState.KeyDown)
                return;

            ConsoleKey key = (ConsoleKey)e.KeyboardData.VirtualCode;
            KeyModifier ctrlModifier = KeyModifier.None;
            KeyModifier shiftModifier = KeyModifier.None;
            KeyModifier altModifier = KeyModifier.None;

            foreach (var hook in _hookingKeys)
            {
                if (hook == key)
                {
                    bool ctrlPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftCtrl) > 1 ||
                        NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightCtrl) > 1;
                    bool shiftPressed = NativeMethods.GetAsyncKeyState(KeyHookerNative.VkLeftShift) > 1 ||
                        NativeMethods.GetAsyncKeyState(KeyHookerNative.VkRightShift) > 1;
                    bool altPressed = e.KeyboardState == KeyState.SysKeyDown && e.KeyboardData.Flags == KeyHookerNative.AltDown;

                    if (ctrlPressed) ctrlModifier = KeyModifier.Ctrl;
                    if (shiftPressed) shiftModifier = KeyModifier.Shift;
                    if (altPressed) altModifier = KeyModifier.Alt;

                    KeyHooked?.Invoke(this, new KeyHookedEventArgs(key, ctrlModifier | shiftModifier | altModifier));
                    //e.Handled = true;
                    break;
                }
            }
        }

        public void Dispose()
        {
            _hooker?.Dispose();
        }
    }
}