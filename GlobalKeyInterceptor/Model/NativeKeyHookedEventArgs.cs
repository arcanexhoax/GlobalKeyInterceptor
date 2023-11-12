using GlobalKeyInterceptor.Enum;
using System.ComponentModel;

namespace GlobalKeyInterceptor.Model
{
    internal class NativeKeyHookedEventArgs : HandledEventArgs
    {
        public NativeKeyState KeyState { get; private set; }
        public LowLevelKeyboardInputEvent KeyData { get; private set; }

        public NativeKeyHookedEventArgs(LowLevelKeyboardInputEvent keyData, NativeKeyState keyState)
        {
            KeyData = keyData;
            KeyState = keyState;
        }
    }
}
