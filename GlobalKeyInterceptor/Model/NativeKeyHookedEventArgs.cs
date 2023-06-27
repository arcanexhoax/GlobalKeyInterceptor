using GlobalKeyInterceptor.Enum;
using System.ComponentModel;

namespace GlobalKeyInterceptor.Model
{
    internal class NativeKeyHookedEventArgs : HandledEventArgs
    {
        public KeyState KeyboardState { get; private set; }
        public LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public NativeKeyHookedEventArgs(LowLevelKeyboardInputEvent keyboardData, KeyState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}
