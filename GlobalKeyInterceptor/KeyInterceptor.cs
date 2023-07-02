﻿using GlobalKeyInterceptor.Enum;
using GlobalKeyInterceptor.Model;
using GlobalKeyInterceptor.Native;
using GlobalKeyInterceptor.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalKeyInterceptor
{
    public class KeyInterceptor : IDisposable
    {
        private readonly NativeKeyInterceptor _interceptor;
        private readonly IEnumerable<Shortcut> _interceptingShortcuts;

        /// <summary>
        /// An event that invokes when any of the specified keys was pressed.
        /// </summary>
        public event EventHandler<ShortcutPressedEventArgs> ShortcutPressed;

        /// <summary>
        /// A class that intercept specified keys. To receive intercepted keys, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        public KeyInterceptor() : this(Enumerable.Empty<Shortcut>()) { }

        /// <summary>
        /// A class that intercept specified keys. To receive intercepted keys, use <see cref="ShortcutPressed"/> event.
        /// </summary>
        /// <param name="interceptingShortcuts">A list of keys that will be intercepted. If parameter is empty, every key will be intercepted.</param>
        public KeyInterceptor(IEnumerable<Shortcut> interceptingShortcuts)
        {
            _interceptor = new NativeKeyInterceptor();
            _interceptor.KeyPressed += OnKeyPressed;
            _interceptingShortcuts = interceptingShortcuts ?? Enumerable.Empty<Shortcut>();
        }

        private void OnKeyPressed(object sender, NativeKeyHookedEventArgs e)
        {
            if (e.KeyState != KeyState.KeyDown && e.KeyState != KeyState.SysKeyDown)
                return;

            Key pressedKey = (Key)e.KeyData.VirtualCode;
            Shortcut shortcut = null;

            // If a modifier specified as a key, then we ignore it as a modifier
            bool ctrlModifierPressed = !KeyUtil.IsKeyCtrl(pressedKey) && KeyUtil.IsCtrlPressed();
            bool shiftModifierPressed = !KeyUtil.IsKeyShift(pressedKey) && KeyUtil.IsShiftPressed();
            bool altModifierPressed = !KeyUtil.IsKeyAlt(pressedKey) && KeyUtil.IsAltPressed();
            bool winModifierPressed = !KeyUtil.IsKeyWin(pressedKey) && KeyUtil.IsWinPressed();

            if (!_interceptingShortcuts.Any())
            {
                KeyModifier ctrlModifier = ctrlModifierPressed ? KeyModifier.Ctrl : KeyModifier.None;
                KeyModifier shiftModifier = shiftModifierPressed ? KeyModifier.Shift : KeyModifier.None;
                KeyModifier altModifier = altModifierPressed ? KeyModifier.Alt : KeyModifier.None;
                KeyModifier winModifier = winModifierPressed ? KeyModifier.Win : KeyModifier.None;

                shortcut = new Shortcut(pressedKey, ctrlModifier | shiftModifier | altModifier | winModifier);
            }
            else
            {
                foreach (var sc in _interceptingShortcuts)
                {
                    if ((sc.Key == Key.Ctrl && (pressedKey == Key.LeftCtrl || pressedKey == Key.RightCtrl)) ||
                        (sc.Key == Key.Shift && (pressedKey == Key.LeftShift || pressedKey == Key.RightShift)) ||
                        (sc.Key == Key.Alt && (pressedKey == Key.LeftAlt || pressedKey == Key.RightAlt)) ||
                        sc.Key == pressedKey)
                    {
                        bool isCtrlHooking = sc.Modifier.HasFlag(KeyModifier.Ctrl);
                        bool isShiftHooking = sc.Modifier.HasFlag(KeyModifier.Shift);
                        bool isAltHooking = sc.Modifier.HasFlag(KeyModifier.Alt);
                        bool isWinHooking = sc.Modifier.HasFlag(KeyModifier.Win);

                        if (isCtrlHooking == ctrlModifierPressed && isShiftHooking == shiftModifierPressed && isAltHooking == altModifierPressed &&
                            isWinHooking == winModifierPressed)
                        {
                            shortcut = sc;
                            break;
                        }
                    }
                }
            }

            if (shortcut != null)
            {
                var keyHookedEventArgs = new ShortcutPressedEventArgs(shortcut);
                ShortcutPressed?.Invoke(this, keyHookedEventArgs);
                e.Handled = keyHookedEventArgs.IsHandled;
            }
        }

        public void Dispose()
        {
            if (_interceptor != null)
            {
                _interceptor.KeyPressed -= OnKeyPressed;
                _interceptor.Dispose();
            }
        }

        ~KeyInterceptor() => Dispose();
    }
}