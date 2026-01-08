## v1.3.1
**Fixed:** 
- Incorrect `IsHandled` logic when multiple callbacks are registered for a single shortcut.

**Added:** 
- New custom keys (`NumEnter`, `StandardEnter`, etc), the generic one (`Enter` etc) remains a wildcard key.
- `Parse()` and `TryParse()` methods for `Shortcut`.
- `FormattedParse()`, `TryFormattedParse()` extension methods for `Key` and `KeyModifier` enums.
- `ToFormattedString()` extension method for `Key` enum.
- `HasWin`, `HasCtrl`, `HasShift`, `HasAlt` extension properties for `KeyModifier`.

## v1.3.0
**Fixed:** 
- A bug where shortcuts couldn't be intercepted on some systems (only simple keystrokes worked)

**Added:** 
- ⚠️ `RegisterShortcut()` method instead of using the constructor `KeyInterceptor(shortcuts)`. This constructor is now deprecated and _will be removed in version 2.0.0_.
- `UnregisterShortcut()` to unregister a specific shortcut handler or all shortcut handlers.
- Extension methods for `Key`: `IsModifierKey()`, `IsFunctionKey()`, `IsNavigationKey()`, `IsDigit()`, `IsNumpadDigit()`, `IsNumpadKey()`, `IsArrowKey()`, `IsLetter()`, `IsCharacterKey()`

## v1.2.1
**Fixed:** 
- Passing key state parameter to the Shortcut constructor ([Issue #2](https://github.com/arcanexhoax/GlobalKeyInterceptor/issues/2))

## v1.2.0
**Fixed:** 
- Interception of shortcut with specified key state.

**Added:** 
- Console applications support