using GlobalKeyInterceptor.Enums;
using GlobalKeyInterceptor.Models;
using GlobalKeyInterceptor.Native;

namespace GlobalKeyInterceptor.Tests.Services;

internal class TestNativeKeyInterceptor : INativeKeyInterceptor
{
    public bool IsLastKeyHandled { get; private set; }

    public event EventHandler<NativeKeyHookedEventArgs>? KeyPressed;

    public void RaiseKeyPressed(int vkCode, NativeKeyState keyState) => RaiseKeyPressed(vkCode, keyState, 0);

    public void RaiseKeyPressed(int vkCode, NativeKeyState keyState, int flags)
    {
        var lowLevelKeyboardInputEvent = new LowLevelKeyboardInputEvent
        {
            VirtualCode = vkCode,
            HardwareScanCode = 0,
            Flags = flags,
            TimeStamp = 0,
            AdditionalInformation = IntPtr.Zero
        };

        var e = new NativeKeyHookedEventArgs(lowLevelKeyboardInputEvent, keyState);
        KeyPressed?.Invoke(this, e);

        IsLastKeyHandled = e.Handled;
    }

    public void Dispose() { }
}
