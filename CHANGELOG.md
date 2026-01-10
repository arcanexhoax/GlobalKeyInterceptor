## v2.0.0
- ⚠️ Removed `KeyInterceptor(shortcuts)` constructor of `KeyInterceptor` class

## v1.3.1
- Fixed incorrect `IsHandled` logic when multiple callbacks are registered for a single shortcut.
- Added new custom keys (`NumEnter`, `StandardEnter`, etc), the generic one (`Enter` etc) remains a wildcard key.
- Added `Parse()` and `TryParse()` methods for `Shortcut`.
- Added `FormattedParse()`, `TryFormattedParse()` extension methods for `Key` and `KeyModifier` enums.
- Added `ToFormattedString()` extension method for `Key` enum.
- Added `HasWin`, `HasCtrl`, `HasShift`, `HasAlt` extension properties for `KeyModifier`.

## v1.3.0
- Fixed a bug where shortcuts couldn't be intercepted on some systems (only simple keystrokes worked)
- ⚠️ Added `RegisterShortcut()` method instead of using the constructor `KeyInterceptor(shortcuts)`. This constructor is now deprecated and _will be removed in version 2.0.0_.
- Added `UnregisterShortcut()` to unregister a specific shortcut handler or all shortcut handlers.
- Added extension methods for `Key`: `IsModifierKey()`, `IsFunctionKey()`, `IsNavigationKey()`, `IsDigit()`, `IsNumpadDigit()`, `IsNumpadKey()`, `IsArrowKey()`, `IsLetter()`, `IsCharacterKey()`

## v1.2.1
- Fixed passing key state parameter to the Shortcut constructor ([Issue #2](https://github.com/arcanexhoax/GlobalKeyInterceptor/issues/2))

## v1.2.0
- Fixed interception of shortcut with specified key state.
- Added console applications support