namespace GlobalKeyInterceptor.Example.ConsoleApp
{
    internal class EntryPoint
    {
        private static KeyInterceptor? s_interceptor;

        static void Main()
        {
            s_interceptor = new KeyInterceptor();

            // Intercepts specific shortcuts
            s_interceptor.RegisterShortcut(new Shortcut(Key.R, state: KeyState.Down), () => Console.WriteLine("R is down"));
            s_interceptor.RegisterShortcut(new Shortcut(Key.Alt, KeyModifier.Ctrl), () => Console.WriteLine("Modifier + Modifier as a simple key"));
            s_interceptor.RegisterShortcut(new Shortcut(Key.D, KeyModifier.Ctrl | KeyModifier.Alt | KeyModifier.Shift | KeyModifier.Win), () =>
            {
                Console.WriteLine("Every modifier + D");

                // Return true if you want to "eat" the pressed key
                return true;
            });

            // Intercepts all pressed keys and shortcuts
            s_interceptor.ShortcutPressed += OnShortcutPressed;

            // Add message loop to a console app to intercept shortcuts
            s_interceptor.RunMessageLoop();
        }

        private static void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
        {
            Console.WriteLine(e.Shortcut);

            // You can also "eat" the pressed key by setting IsHandled to true
            e.IsHandled = true;
        }
    }
}
