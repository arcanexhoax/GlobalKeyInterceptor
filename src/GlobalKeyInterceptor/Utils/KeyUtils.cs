using GlobalKeyInterceptor.Native;

namespace GlobalKeyInterceptor.Utils;

public static class KeyUtils
{
    /// <summary>
    /// Check if Ctrl (left of right) key is pressed
    /// </summary>
    public static bool IsCtrlPressed => IsKeyPressed(Key.Ctrl);

    /// <summary>
    /// Check if Shift (left of right) key is pressed
    /// </summary>
    public static bool IsShiftPressed => IsKeyPressed(Key.Shift);

    /// <summary>
    /// Check if Alt (left of right) key is pressed
    /// </summary>
    public static bool IsAltPressed => IsKeyPressed(Key.Alt);

    /// <summary>
    /// Check if Windows (left of right) key is pressed
    /// </summary>
    public static bool IsWinPressed => IsKeyPressed(Key.LeftWindows) || IsKeyPressed(Key.RightWindows);

    /// <summary>
    /// Check if the specified key is pressed
    /// </summary>
    public static bool IsKeyPressed(Key key) => (NativeMethods.GetAsyncKeyState((uint)key) & 0x8000) > 0;
}

internal interface IKeyUtilsService
{
    bool IsCtrlPressed { get; }
    bool IsShiftPressed { get; }
    bool IsAltPressed { get; }
    bool IsWinPressed { get; }
}

internal class KeyUtilsService : IKeyUtilsService
{
    public bool IsCtrlPressed => KeyUtils.IsCtrlPressed;
    public bool IsShiftPressed => KeyUtils.IsShiftPressed;
    public bool IsAltPressed => KeyUtils.IsAltPressed;
    public bool IsWinPressed => KeyUtils.IsWinPressed;
}