using GlobalKeyInterceptor.Enum;
using System.ComponentModel;

namespace GlobalKeyInterceptor.Model
{
    internal class NativeKeyHookedEventArgs : HandledEventArgs
    {
        public KeyState KeyState { get; private set; }
        public LowLevelKeyboardInputEvent KeyData { get; private set; }

        public NativeKeyHookedEventArgs(LowLevelKeyboardInputEvent keyData, KeyState keyState)
        {
            KeyData = keyData;
            KeyState = keyState;
        }
    }
}
