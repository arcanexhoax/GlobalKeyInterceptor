using GlobalKeyInterceptor.Enums;
using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests.Services;

public class KeyInterceptorRegisterShortcutTests
{
    [Theory]
    [InlineData(Key.A, KeyModifier.None, KeyState.Down, false)]
    [InlineData(Key.D2, KeyModifier.Ctrl, KeyState.Up, true)]
    [InlineData(Key.LeftCtrl, KeyModifier.Shift | KeyModifier.Alt, KeyState.Down, true)]
    [InlineData(Key.RightAlt, KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win, KeyState.Up, false)]
    [InlineData(Key.Enter, KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win | KeyModifier.Ctrl, KeyState.Down, false)]
    public void RegisterShortcut_FuncBool_HandlerIsCalledAndReturnsIsHandled(Key key, KeyModifier keyModifier, KeyState state, bool isHandled)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = keyModifier.HasWin,
            IsCtrlPressed = keyModifier.HasCtrl,
            IsShiftPressed = keyModifier.HasShift,
            IsAltPressed = keyModifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(key, keyModifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () => 
        { 
            called = true; 
            return isHandled; 
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)key.BaseKey, nativeKeyState);

        Assert.True(called);
        Assert.Equal(isHandled, nativeKeyInterceptor.IsLastKeyHandled);
    }

    [Theory]
    [InlineData(Key.B, KeyModifier.None, KeyState.Down, false)]
    [InlineData(Key.A, KeyModifier.Ctrl, KeyState.Down, false)]
    [InlineData(Key.A, KeyModifier.None, KeyState.Up, false)]
    public void RegisterShortcut_FuncBool_HandlerIsNotCalledOnMismatch(Key key, KeyModifier keyModifier, KeyState state, bool isHandled)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = keyModifier.HasWin,
            IsCtrlPressed = keyModifier.HasCtrl,
            IsShiftPressed = keyModifier.HasShift,
            IsAltPressed = keyModifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(Key.A, KeyModifier.None, KeyState.Down);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () =>
        {
            called = true;
            return isHandled;
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)key.BaseKey, nativeKeyState);

        Assert.False(called);
        Assert.False(nativeKeyInterceptor.IsLastKeyHandled);
    }

    [Theory]
    [InlineData(Key.A, KeyModifier.None, KeyState.Down, false)]
    [InlineData(Key.D2, KeyModifier.Ctrl, KeyState.Up, true)]
    [InlineData(Key.LeftCtrl, KeyModifier.Shift | KeyModifier.Alt, KeyState.Down, true)]
    [InlineData(Key.RightAlt, KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win, KeyState.Up, false)]
    [InlineData(Key.Enter, KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win | KeyModifier.Ctrl, KeyState.Down, false)]
    public void RegisterShortcut_Action_HandlerIsCalledAndSetsIsHandled(Key key, KeyModifier keyModifier, KeyState state, bool isHandled)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = keyModifier.HasWin,
            IsCtrlPressed = keyModifier.HasCtrl,
            IsShiftPressed = keyModifier.HasShift,
            IsAltPressed = keyModifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(key, keyModifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () => called = true, isHandled);

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)key.BaseKey, nativeKeyState);

        Assert.True(called);
        Assert.Equal(isHandled, nativeKeyInterceptor.IsLastKeyHandled);
    }

    [Theory]
    [InlineData(Key.B, KeyModifier.None, KeyState.Down, false)]
    [InlineData(Key.A, KeyModifier.Ctrl, KeyState.Down, false)]
    [InlineData(Key.A, KeyModifier.None, KeyState.Up, false)]
    public void RegisterShortcut_Action_HandlerIsNotCalledOnMismatch(Key key, KeyModifier keyModifier, KeyState state, bool isHandled)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = keyModifier.HasWin,
            IsCtrlPressed = keyModifier.HasCtrl,
            IsShiftPressed = keyModifier.HasShift,
            IsAltPressed = keyModifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(Key.A, KeyModifier.None, KeyState.Down);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () => called = true, isHandled);

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)key.BaseKey, nativeKeyState);

        Assert.False(called);
        Assert.False(nativeKeyInterceptor.IsLastKeyHandled);
    }

    [Theory]
    [InlineData(Key.LeftCtrl, KeyModifier.None, KeyState.Down, Key.Ctrl)]
    [InlineData(Key.RightShift, KeyModifier.Ctrl, KeyState.Up, Key.Shift)]
    [InlineData(Key.LeftAlt, KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, KeyState.Up, Key.Alt)]
    public void RegisterShortcut_FuncBool_HandlerIsCalledWithGenericKeys(Key raiseKey, KeyModifier modifier, KeyState state, Key registerKey)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = modifier.HasWin,
            IsCtrlPressed = modifier.HasCtrl,
            IsShiftPressed = modifier.HasShift,
            IsAltPressed = modifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(registerKey, modifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () =>
        {
            called = true;
            return false;
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)raiseKey.BaseKey, nativeKeyState);

        Assert.True(called);
    }

    [Theory]
    [InlineData(Key.Ctrl, KeyModifier.None, KeyState.Down, Key.LeftCtrl)]
    [InlineData(Key.Shift, KeyModifier.Ctrl, KeyState.Up, Key.RightShift)]
    [InlineData(Key.Alt, KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, KeyState.Up, Key.LeftAlt)]
    public void RegisterShortcut_FuncBool_HandlerIsNotCalledOnMismatchWithGenericKeys(Key raiseKey, KeyModifier modifier, KeyState state, Key registerKey)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = modifier.HasWin,
            IsCtrlPressed = modifier.HasCtrl,
            IsShiftPressed = modifier.HasShift,
            IsAltPressed = modifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(registerKey, modifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () =>
        {
            called = true;
            return false;
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)raiseKey.BaseKey, nativeKeyState);

        Assert.False(called);
    }

    [Theory]
    [InlineData(Key.Enter, KeyModifier.None, KeyState.Down, 0x00, Key.StandardEnter)]
    [InlineData(Key.Enter, KeyModifier.None, KeyState.Down, 0x00, Key.Enter)]
    [InlineData(Key.Enter, KeyModifier.Ctrl, KeyState.Up, 0x01, Key.Enter)]
    [InlineData(Key.Enter, KeyModifier.Ctrl, KeyState.Up, 0x01, Key.NumEnter)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win, KeyState.Down, 0x01, Key.PageDown)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win, KeyState.Down, 0x01, Key.StandardPageDown)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, KeyState.Up, 0x00, Key.NumPageDown)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, KeyState.Up, 0x00, Key.PageDown)]
    public void RegisterShortcut_FuncBool_HandlerIsCalledWithGenericExtendedKeys(Key raiseKey, KeyModifier modifier, KeyState state, int raiseKeyFlags, Key registerKey)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = modifier.HasWin,
            IsCtrlPressed = modifier.HasCtrl,
            IsShiftPressed = modifier.HasShift,
            IsAltPressed = modifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(registerKey, modifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () =>
        {
            called = true;
            return false;
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)raiseKey.BaseKey, nativeKeyState, raiseKeyFlags);

        Assert.True(called);
    }

    [Theory]
    [InlineData(Key.Enter, KeyModifier.None, KeyState.Down, 0x00, Key.NumEnter)]
    [InlineData(Key.Enter, KeyModifier.Ctrl, KeyState.Up, 0x01, Key.StandardEnter)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win, KeyState.Down, 0x01, Key.NumPageDown)]
    [InlineData(Key.PageDown, KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, KeyState.Up, 0x00, Key.StandardPageDown)]
    public void RegisterShortcut_FuncBool_HandlerIsNotCalledOnMismatchWithGenericExtendedKeys(Key raiseKey, KeyModifier modifier, KeyState state, int raiseKeyFlags, Key registerKey)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService()
        {
            IsWinPressed = modifier.HasWin,
            IsCtrlPressed = modifier.HasCtrl,
            IsShiftPressed = modifier.HasShift,
            IsAltPressed = modifier.HasAlt,
        };

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        var shortcut = new Shortcut(registerKey, modifier, state);
        var called = false;

        keyInterceptor.RegisterShortcut(shortcut, () =>
        {
            called = true;
            return false;
        });

        var nativeKeyState = state == KeyState.Down ? NativeKeyState.KeyDown : NativeKeyState.KeyUp;
        nativeKeyInterceptor.RaiseKeyPressed((int)raiseKey.BaseKey, nativeKeyState, raiseKeyFlags);

        Assert.False(called);
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(true, false, false, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, false, true, true)]
    [InlineData(true, true, false, true)]
    [InlineData(false, true, true, true)]
    [InlineData(true, true, true, true)]
    public void RegisterShortcut_Combined_IsHandledSetOnce(bool handledSpecific, bool handledGeneric, bool handledEvent, bool expected)
    {
        var nativeKeyInterceptor = new TestNativeKeyInterceptor();
        var keyUtilsService = new TestKeyUtilsService();

        using var keyInterceptor = new KeyInterceptor(nativeKeyInterceptor, keyUtilsService);

        keyInterceptor.RegisterShortcut(new Shortcut(Key.LeftCtrl), () => handledSpecific);
        keyInterceptor.RegisterShortcut(new Shortcut(Key.Ctrl), () => handledGeneric);
        keyInterceptor.ShortcutPressed += (_, e) => e.IsHandled = handledEvent;

        nativeKeyInterceptor.RaiseKeyPressed((int)Key.LeftCtrl, NativeKeyState.KeyUp);

        Assert.Equal(expected, nativeKeyInterceptor.IsLastKeyHandled);
    }
}
