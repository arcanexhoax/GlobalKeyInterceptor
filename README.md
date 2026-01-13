## GlobalKeyInterceptor
![NET](https://img.shields.io/badge/.NET-Standard%202.0-%23512BD4)
[![Version](https://img.shields.io/nuget/vpre/GlobalKeyInterceptor.svg?label=NuGet)](https://www.nuget.org/packages/GlobalKeyInterceptor)
[![Downloads](https://img.shields.io/nuget/dt/GlobalKeyInterceptor.svg?label=Downloads)](https://www.nuget.org/packages/GlobalKeyInterceptor/)
[![License](https://img.shields.io/github/license/arcanexhoax/GlobalKeyInterceptor.svg?color=00b542&label=License)](https://raw.githubusercontent.com/arcanexhoax/GlobalKeyInterceptor/master/LICENSE)

It's a simple to use library that allows you to intercept keystrokes or shortcuts in the system.

## Features 
- Intercept specified keys and shortcuts or intercept every keystroke
- Possibility to "eat" pressed key
- Works on any Windows application platforms (WPF/WinForms/Console)

## Installation
Add the following string to the *.csproj* file:
```xml
<PackageReference Include="GlobalKeyInterceptor" Version="1.3.2" />
```

## Example
Add namespace
```cs
using GlobalKeyInterceptor;
```

### Intercept everything
```cs
var interceptor = new KeyInterceptor();

interceptor.ShortcutPressed += (_, e) =>
{
    Console.WriteLine($"{e.Shortcut} {e.Shortcut.State}");
};
```

### Intercept only specified keystrokes/shortcuts
```cs
interceptor.RegisterShortcut(new Shortcut(Key.R, state: KeyState.Down), () => Console.WriteLine("R is down"));
interceptor.RegisterShortcut(new Shortcut(Key.Alt, KeyModifier.Ctrl), () => Console.WriteLine("Modifier + Modifier as a simple key"));
interceptor.RegisterShortcut(new Shortcut(Key.D, KeyModifier.Ctrl | KeyModifier.Alt | KeyModifier.Shift | KeyModifier.Win), () =>
{
    Console.WriteLine("Every modifier + D");

    // Return true if you want to "eat" the pressed key
    return true;
});
```

### Console application
To allow the console application to intercept keystrokes, you need to add a message loop at the end of the `Main()` method:
```cs
interceptor.RunMessageLoop();
```

Full example you can find [here](https://github.com/arcanexhoax/GlobalKeyInterceptor/blob/main/samples/ConsoleApp/EntryPoint.cs) 
