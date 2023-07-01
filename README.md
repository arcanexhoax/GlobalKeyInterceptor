## GlobalKeyInterceptor
It's a simple to use library that allows you to intercept keystrokes or shortcuts in the system

## Features 
- Intercept specified keys and shortcuts or intercept every keystroke
- Possibility to "eat" pressed key

## Example
```cs
using GlobalKeyInterceptor;
namespace InterceptorExample
{
    internal class Program
    {
        private static KeyInterceptor _interceptor;
        static void Main()
        {
            // Creating an array of shortcuts that you want to intercept
            Shortcut[] shortcuts = new Shortcut[]
            {
                // You can specify a key, up to 4 modifiers, and a name
                new Shortcut(Key.R, "R"),
                new Shortcut(Key.D, KeyModifier.Alt | KeyModifier.Shift, "Alt + Shift + D")
            };

            _interceptor = new KeyInterceptor(shortcuts);
            _interceptor.ShortcutPressed += OnShortcutPressed;
        }

        private static void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
        {
            // Specify a name of the shortcut to easy check if it pressed
            switch (e.Shortcut.Name)
            {
                case "R":
                    Console.WriteLine("The 'R' key is pressed");
                    // Set IsHandled to true if you want to "eat" pressed key
                    e.IsHandled = true;
                    break;
                case "Shift + Alt + D":
                    Console.WriteLine("The 'Shift + Alt + D' shortcut is pressed");
                    break;
            }
        }
    }
}
```
