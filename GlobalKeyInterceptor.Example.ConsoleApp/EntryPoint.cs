namespace GlobalKeyInterceptor.Example.ConsoleApp
{
    internal class EntryPoint
    {
        private static KeyInterceptor? s_interceptor;

        static void Main()
        {
            // Creating an array of shortcuts that you want to intercept
            Shortcut[] shortcuts =
            [
                // You can specify a key, up to 4 modifiers, key state and a name
                new Shortcut(Key.R, name: "R"),
                new Shortcut(Key.Alt, KeyModifier.Ctrl, name: "Ctrl + Alt"),
                new Shortcut(Key.D, KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win, KeyState.Down, "Ctrl + Shift + Alt + Win + D"),
            ];

            s_interceptor = new KeyInterceptor(shortcuts);
            s_interceptor.ShortcutPressed += OnShortcutPressed;

            // Add message loop to a console app to intercept shortcuts
            s_interceptor.RunMessageLoop();
        }

        private static void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
        {
            Console.WriteLine(e.Shortcut.Name);

            // Specify a name of the shortcut to easy check if it pressed
            switch (e.Shortcut.Name)
            {
                case "R":
                    // some logic
                    // Set IsHandled to true if you want to "eat" the pressed key
                    e.IsHandled = true;
                    break;
                case "Ctrl + Alt":
                    // some logic 2
                    break;
                case "Ctrl + Shift + Alt + Win + D":
                    // some logic 3
                    break;
            }
        }
    }
}
