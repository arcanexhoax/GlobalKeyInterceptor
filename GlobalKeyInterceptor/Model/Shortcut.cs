using GlobalKeyInterceptor.Enum;
using System.Text;

namespace GlobalKeyInterceptor.Model
{
    public class Shortcut
    {
        /// <summary>
        /// Intercepted key.
        /// </summary>
        /// <remarks>
        /// To get VK code of the key use <see cref="int"/> casting.
        /// </remarks>
        public Key Key { get; }

        /// <summary>
        /// A modifier of the intercepted shortcut.
        /// </summary>
        /// <remarks>
        /// If modifier isn't present, the value <see cref="KeyModifier.None"/> is set.
        /// <br/>The value can contain multiple modifiers, to check if modifier is intercepted, use <see cref="System.Enum.HasFlag(System.Enum)"/>.
        /// <code>bool isCtrlIntercepted = e.Modifier.HasFlag(KeyModifier.Ctrl);</code>
        public KeyModifier Modifier { get; }

        /// <summary>
        /// A name of the shortcut.
        /// </summary>
        public string Name { get; }

        /// <param name="key">Intercepted key.</param>
        /// <param name="modifier">A modifier of the intercepted shortcut. Use "|" to set multiple modifiers.</param>
        /// <param name="name">A name of the shortcut.</param>
        public Shortcut(Key key, KeyModifier modifier = KeyModifier.None, string name = null)
        {
            Key = key;
            Modifier = modifier;
            Name = name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(string.IsNullOrEmpty(Name) ? "(" : Name + " (");

            if (Modifier.HasFlag(KeyModifier.Ctrl))
                sb.Append("Ctrl + ");
            if (Modifier.HasFlag(KeyModifier.Shift))
                sb.Append("Shift + ");
            if (Modifier.HasFlag(KeyModifier.Alt))
                sb.Append("Alt + ");

            sb.Append(Key.ToString() + ")");
            return sb.ToString();
        }
    }
}
