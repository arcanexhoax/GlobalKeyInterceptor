using GlobalKeyInterceptor.Enums;
using System.ComponentModel;

namespace GlobalKeyInterceptor.Models;

internal class NativeKeyHookedEventArgs(LowLevelKeyboardInputEvent keyData, NativeKeyState keyState) : HandledEventArgs
{
    public NativeKeyState KeyState { get; private set; } = keyState;
    public LowLevelKeyboardInputEvent KeyData { get; private set; } = keyData;
}
