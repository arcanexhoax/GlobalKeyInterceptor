using GlobalKeyInterceptor.Models;
using GlobalKeyInterceptor.Native;
using GlobalKeyInterceptor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKeyInterceptor;

public interface IKeyInterceptor
{
    /// <summary>
    /// An event that invokes when any keys/shortcuts was pressed.
    /// <br/><b>Warning:</b> Using long-running operations in the event handler may cause <see cref="ShortcutPressedEventArgs.IsHandled"/> to not work properly.
    /// Use another thread or task to perform long-running operations inside the event handler.
    /// <code language="csharp">
    /// private void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
    /// {
    ///    Task.Run(LongRunningOperation);
    ///    e.IsHandled = true;
    /// }
    /// </code>
    /// </summary>
    event EventHandler<ShortcutPressedEventArgs> ShortcutPressed;

    /// <summary>
    /// Run a message loop that allows you to intercept keys in Console applications.
    /// <br/>WPF/WinForms applications have their own message loop, so there is no need to use this method in such applications.
    /// </summary>
    void RunMessageLoop();

    /// <summary>
    /// Registers a shortcut to intercept. If the shortcut already registered, the handler will be added to the existing one.
    /// <param name="shortcut">An intercepting shortcut</param>
    /// <param name="handler">A callback that will be invoked when the shortcut is pressed. Return true if you want to "eat" the pressed key.</param>"
    /// </summary>
    void RegisterShortcut(Shortcut shortcut, Func<bool> handler);

    /// <summary>
    /// Registers a shortcut to intercept. If the shortcut already registered, the handler will be added to the existing one.
    /// <param name="shortcut">An intercepting shortcut</param>
    /// <param name="handler">A callback that will be invoked when the shortcut is pressed.</param>
    /// <param name="handled">If true, the pressed shortcut will be "eaten" and not passed to the system.</param>
    /// </summary>
    void RegisterShortcut(Shortcut shortcut, Action handler, bool handled = false);

    /// <summary>
    /// Unregisters a specific shortcut handler.
    /// <param name="shortcut">The shortcut to unregister.</param>
    /// <param name="handler">The specific callback that was registered for the shortcut.</param>
    /// </summary>
    void UnregisterShortcut(Shortcut shortcut, Func<bool> handler);

    /// <summary>
    /// Unregister all handlers for the specified shortcut.
    /// <param name="shortcut">The shortcut to unregister with all registered handlers</param>
    /// </summary>
    void UnregisterShortcut(Shortcut shortcut);

    /// <summary>
    /// Unregisters all shortcuts and their handlers.
    /// </summary>
    void UnregisterAllShortcuts();
}

/// <summary>
/// A class that allows you to intercept the specified or every keystroke/shortcut in the system
/// </summary>
public class KeyInterceptor : IKeyInterceptor, IDisposable
{
    private readonly INativeKeyInterceptor _interceptor;
    private readonly IKeyUtilsService _keyUtilsService;
    private readonly Dictionary<Shortcut, HashSet<Func<bool>>> _shortcuts;
    private readonly bool _usedObsoleteConstructor; // TODO remove after 2.0 release

    private bool _disposed;

    /// <inheritdoc/>
    public event EventHandler<ShortcutPressedEventArgs> ShortcutPressed;

    internal KeyInterceptor(INativeKeyInterceptor nativeKeyInterceptor, IKeyUtilsService keyUtilsService)
    {
        _interceptor = nativeKeyInterceptor;
        _interceptor.KeyPressed += OnKeyPressed;
        _keyUtilsService = keyUtilsService;
        _shortcuts = [];
    }

    /// <summary>
    /// Creates an interceptor instance. To intercept specific key/shortcuts use <see cref="RegisterShortcut(Shortcut, Func{bool})"/>. 
    /// To receive all keys/shortcuts, use <see cref="ShortcutPressed"/> event.
    /// </summary>
    public KeyInterceptor() : this(new NativeKeyInterceptor(), new KeyUtilsService()) { }

    /// <summary>
    /// Creates an instance that intercepts specified keys/shortcuts. To receive intercepted keys/shortcuts, use <see cref="ShortcutPressed"/> event.
    /// </summary>
    /// <param name="interceptingShortcuts">A list of keys/shortcuts that will be intercepted.</param>
    [Obsolete("Use empty constructor with RegisterShortcut() method instead. Starting from version 2.0 this constructor will be removed.", false)]
    public KeyInterceptor(IEnumerable<Shortcut> interceptingShortcuts)
    {
        _usedObsoleteConstructor = true;
        _interceptor.KeyPressed += OnKeyPressed;
        _shortcuts = interceptingShortcuts.ToDictionary(
            s => s,
            s => new HashSet<Func<bool>>()
            {
                () =>
                {
                    var args = new ShortcutPressedEventArgs(s);
                    ShortcutPressed?.Invoke(this, args);
                    return args.IsHandled;
                }
            });
    }

    /// <inheritdoc/>
    public void RunMessageLoop()
    {
        while (true)
        {
            var result = NativeMethods.GetMessage(out var msg, IntPtr.Zero, 0, 0);

            if (result > 0)
            {
                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessage(ref msg);
            }
        }
    }

    /// <inheritdoc/>
    public void RegisterShortcut(Shortcut shortcut, Func<bool> handler)
    {
        if (_shortcuts.TryGetValue(shortcut, out var handlers))
        {
            handlers.Add(handler);
        }
        else
        {
            _shortcuts[shortcut] = [handler];
        }
    }

    // TODO write unit tests
    /// <inheritdoc/>
    public void RegisterShortcut(Shortcut shortcut, Action handler, bool handled = false)
    {
        RegisterShortcut(shortcut, () =>
        {
            handler();
            return handled;
        });
    }

    /// <inheritdoc/>
    public void UnregisterShortcut(Shortcut shortcut, Func<bool> handler)
    {
        if (_shortcuts.TryGetValue(shortcut, out var handlers))
        {
            handlers.Remove(handler);

            if (handlers.Count == 0)
                _shortcuts.Remove(shortcut);
        }
    }

    /// <inheritdoc/>
    public void UnregisterShortcut(Shortcut shortcut) => _shortcuts.Remove(shortcut);

    /// <inheritdoc/>
    public void UnregisterAllShortcuts() => _shortcuts.Clear();

    private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
    {
        var state = e.KeyState.ToKeyState();
        var baseKey = (Key)e.KeyData.VirtualCode;
        var pressedKey = GetPressedKey(baseKey, e.KeyData.Flags);
        var pressedModifier = GetPressedModifiers(pressedKey);

        Debug.WriteLine($"Key {pressedKey}. State: {state}");

        foreach (var scKeyValue in _shortcuts)
        {
            var sc = scKeyValue.Key;

            if ((sc.Key == Key.Ctrl && pressedKey.IsCtrl 
                || sc.Key == Key.Shift && pressedKey.IsShift
                || sc.Key == Key.Alt && pressedKey.IsAlt
                || sc.Key == pressedKey.BaseKey
                || sc.Key == pressedKey)
                && sc.Modifier == pressedModifier
                && sc.State == state)
            {
                foreach (var handler in scKeyValue.Value)
                    e.Handled |= handler();
            }
        }

        if (_usedObsoleteConstructor)
            return;

        var shortcut = new Shortcut(pressedKey, pressedModifier, state);
        var keyHookedEventArgs = new ShortcutPressedEventArgs(shortcut);

        ShortcutPressed?.Invoke(this, keyHookedEventArgs);
        e.Handled |= keyHookedEventArgs.IsHandled;
    }

    private Key GetPressedKey(Key key, int flags)
    {
        var vkCode = (int)key.BaseKey;
        var isExtended = (flags & 0x01) != 0;
        var offset = isExtended ? 0x200 : 0x100;
        var customKey = (Key)(vkCode | offset);

        return Enum.IsDefined(typeof(Key), customKey) ? customKey : key;
    }

    private KeyModifier GetPressedModifiers(Key pressedKey)
    {
        var pressedModifier = KeyModifier.None;

        // If a modifier specified as a key, then we ignore it as a modifier
        if (!pressedKey.IsCtrl && _keyUtilsService.IsCtrlPressed)
            pressedModifier |= KeyModifier.Ctrl;
        if (!pressedKey.IsShift && _keyUtilsService.IsShiftPressed)
            pressedModifier |= KeyModifier.Shift;
        if (!pressedKey.IsAlt && _keyUtilsService.IsAltPressed)
            pressedModifier |= KeyModifier.Alt;
        if (!pressedKey.IsWin && _keyUtilsService.IsWinPressed)
            pressedModifier |= KeyModifier.Win;

        return pressedModifier;
    }

    public void Dispose()
    {
        if (_interceptor != null && !_disposed)
        {
            _interceptor.Dispose();
            _disposed = true;

            ShortcutPressed = null;
            UnregisterAllShortcuts();
        }
    }

    ~KeyInterceptor() => Dispose();
}