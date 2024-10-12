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
                new Shortcut(Key.R, state: KeyState.Down, name: "R (key is down)"),
                new Shortcut(Key.Alt, KeyModifier.Ctrl, name: "Modifier + Modifier as a simple key"),
                new Shortcut(Key.D, KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt | KeyModifier.Win,
                    name: "Every modifier + D"),
            ];

            s_interceptor = new KeyInterceptor(shortcuts);
            s_interceptor.ShortcutPressed += OnShortcutPressed;

            // Add message loop to a console app to intercept shortcuts
            s_interceptor.RunMessageLoop();
        }

        private static void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
        {
            Console.WriteLine(e.Shortcut);

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
        }
    }
}
