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
<PackageReference Include="GlobalKeyInterceptor" Version="1.2.1" />
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
    Console.WriteLine(e.Shortcut);
};
```

### Intercept only specified keystrokes/shortcuts
```cs
Shortcut[] shortcuts =
[
    // You can specify a key, up to 4 modifiers, key state and a name
    new Shortcut(Key.R, state: KeyState.Down, name: "R (key is down)"),
    new Shortcut(Key.Alt, KeyModifier.Ctrl, name: "Modifier + Modifier as a simple key"),
    new Shortcut(Key.D, KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win, 
        name: "Every modifier + D"),
];

var interceptor = new KeyInterceptor(shortcuts);

interceptor.ShortcutPressed += (_, e) =>
{
    // Specify a name of the shortcut to easy check if it pressed
    switch (e.Shortcut.Name)
    {
        case "R (key is down)":
            // Set e.IsHandled to true if you want to "eat" the pressed key
            e.IsHandled = true;

            // some logic
            break;
        case "Modifier + Modifier as a simple key":
            // some logic 2
            break;
        case "Every modifier + D":
            // some logic 3
            break;
    }
};
```

### Console application
To allow the console application to intercept keystrokes, you need to add a message loop at the end of the `Main()` method:
```cs
interceptor.RunMessageLoop();
```

Full example you can find [here](https://github.com/arcanexhoax/GlobalKeyInterceptor/blob/main/GlobalKeyInterceptor.Example.ConsoleApp/EntryPoint.cs) 
