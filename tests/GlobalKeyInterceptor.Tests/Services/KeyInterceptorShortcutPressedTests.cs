using GlobalKeyInterceptor.Enums;
using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests.Services;

public class KeyInterceptorShortcutPressedTests
{
    [Theory]
    [InlineData(0x9, NativeKeyState.KeyUp, Key.Tab)]
    [InlineData(0xD, NativeKeyState.KeyUp, Key.Enter)]
    [InlineData(0x10, NativeKeyState.SysKeyDown, Key.Shift)]
    [InlineData(0x24, NativeKeyState.SysKeyUp, Key.Home)]
    [InlineData(0x27, NativeKeyState.KeyDown, Key.RightArrow)]
    [InlineData(0x31, NativeKeyState.KeyUp, Key.D1)]
    [InlineData(0x41, NativeKeyState.KeyDown, Key.A)]
    [InlineData(0x5B, NativeKeyState.SysKeyDown, Key.LeftWindows)]
    [InlineData(0x62, NativeKeyState.SysKeyUp, Key.Num2)]
    [InlineData(0x73, NativeKeyState.KeyDown, Key.F4)]
    [InlineData(0xA1, NativeKeyState.KeyUp, Key.RightShift)]
    [InlineData(0xA2, NativeKeyState.SysKeyDown, Key.LeftCtrl)]
    [InlineData(0xA5, NativeKeyState.SysKeyUp, Key.RightAlt)]
    [InlineData(0xBD, NativeKeyState.KeyDown, Key.Minus)]
    internal void KeyStrokeIntercepts(int vkCode, NativeKeyState keyState, Key result)
    {
        Shortcut? shortcut = null;

        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService();

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);
        keyInterceptor.ShortcutPressed += (_, e) => shortcut = e.Shortcut;

        nativeKeyInterceptor.RaiseKeyPressed(vkCode, keyState);

        Assert.Equal(result, shortcut?.Key.BaseKey);
        Assert.Equal(keyState.ToKeyState(), shortcut?.State);
    }

    [Theory]
    [InlineData(0x0D, 0x00, NativeKeyState.KeyDown, Key.StandardEnter)]
    [InlineData(0x0D, 0x01, NativeKeyState.KeyUp, Key.NumEnter)]
    [InlineData(0x2E, 0x01, NativeKeyState.SysKeyDown, Key.StandardDelete)]
    [InlineData(0x2E, 0x00, NativeKeyState.SysKeyUp, Key.NumDelete)]
    [InlineData(0x2D, 0x01, NativeKeyState.SysKeyDown, Key.StandardInsert)]
    [InlineData(0x2D, 0x00, NativeKeyState.SysKeyUp, Key.NumInsert)]
    [InlineData(0x24, 0x01, NativeKeyState.KeyDown, Key.StandardHome)]
    [InlineData(0x24, 0x00, NativeKeyState.KeyUp, Key.NumHome)]
    [InlineData(0x23, 0x01, NativeKeyState.SysKeyDown, Key.StandardEnd)]
    [InlineData(0x23, 0x00, NativeKeyState.SysKeyUp, Key.NumEnd)]
    [InlineData(0x21, 0x01, NativeKeyState.KeyDown, Key.StandardPageUp)]
    [InlineData(0x21, 0x00, NativeKeyState.KeyUp, Key.NumPageUp)]
    [InlineData(0x22, 0x01, NativeKeyState.SysKeyDown, Key.StandardPageDown)]
    [InlineData(0x22, 0x00, NativeKeyState.SysKeyUp, Key.NumPageDown)]
    [InlineData(0x25, 0x01, NativeKeyState.KeyDown, Key.StandardLeftArrow)]
    [InlineData(0x25, 0x00, NativeKeyState.KeyUp, Key.NumLeftArrow)]
    [InlineData(0x26, 0x01, NativeKeyState.SysKeyDown, Key.StandardUpArrow)]
    [InlineData(0x26, 0x00, NativeKeyState.SysKeyUp, Key.NumUpArrow)]
    [InlineData(0x27, 0x01, NativeKeyState.KeyDown, Key.StandardRightArrow)]
    [InlineData(0x27, 0x00, NativeKeyState.KeyUp, Key.NumRightArrow)]
    [InlineData(0x28, 0x01, NativeKeyState.SysKeyDown, Key.StandardDownArrow)]
    [InlineData(0x28, 0x00, NativeKeyState.SysKeyUp, Key.NumDownArrow)]
    internal void CustomKeyIntercepts(int vkCode, int flag, NativeKeyState keyState, Key expectedKey)
    {
        Shortcut? shortcut = null;

        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService();

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);
        keyInterceptor.ShortcutPressed += (_, e) => shortcut = e.Shortcut;

        nativeKeyInterceptor.RaiseKeyPressed(vkCode, keyState, flag);

        Assert.Equal(expectedKey, shortcut?.Key);
        Assert.Equal(keyState.ToKeyState(), shortcut?.State);
    }

    [Theory]
    [InlineData(0x41, false, false, false, false, Key.A, KeyModifier.None)]
    [InlineData(0x5B, true, false, false, false, Key.LeftWindows, KeyModifier.None)]
    [InlineData(0x5B, false, true, false, false, Key.LeftWindows, KeyModifier.Ctrl)]
    [InlineData(0x11, true, true, false, false, Key.Ctrl, KeyModifier.Win)]
    [InlineData(0xA1, true, true, true, false, Key.RightShift, KeyModifier.Win | KeyModifier.Ctrl)]
    [InlineData(0xA4, true, true, true, true, Key.LeftAlt, KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift)]
    [InlineData(0x10D, true, true, true, true, Key.StandardEnter, KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt)]
    [InlineData(0x123, true, true, true, true, Key.NumEnd, KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt)]
    [InlineData(0x26, true, true, true, true, Key.UpArrow, KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt)]
    internal void ModifierKeysPressed(int vkCode, bool win, bool ctrl, bool shift, bool alt, Key expectedKey, KeyModifier expectedModifier)
    {
        Shortcut? shortcut = null;

        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService
        {
            IsCtrlPressed = ctrl,
            IsShiftPressed = shift,
            IsAltPressed = alt,
            IsWinPressed = win
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);
        keyInterceptor.ShortcutPressed += (_, e) => shortcut = e.Shortcut;

        nativeKeyInterceptor.RaiseKeyPressed(vkCode, NativeKeyState.KeyDown);

        Assert.Equal(expectedKey, shortcut?.Key);
        Assert.Equal(expectedModifier, shortcut?.Modifier);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void IsHandledWorks(bool handled)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService();

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);
        keyInterceptor.ShortcutPressed += (_, e) => e.IsHandled = handled;

        nativeKeyInterceptor.RaiseKeyPressed((int)Key.A, NativeKeyState.SysKeyDown);

        Assert.Equal(handled, nativeKeyInterceptor.IsLastKeyHandled);
    }
}
