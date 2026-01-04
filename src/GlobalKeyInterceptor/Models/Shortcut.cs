using GlobalKeyInterceptor.Utils;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GlobalKeyInterceptor;

/// <summary>
/// A class that represents an intercepted keystroke/shortcut in the system
/// </summary>
public class Shortcut
{
    /// <summary> Intercepted key. </summary>
    /// <remarks> To get VK code of the key use <see cref="int"/> casting. </remarks>
    public Key Key { get; }

    /// <summary> A modifier of the intercepted shortcut. </summary>
    public KeyModifier Modifier { get; }

    /// <summary> Intercepted state of the specified key. </summary>
    /// <remarks> It does not apply to modifiers. Default value is <see cref="KeyState.Up"/> </remarks>
    public KeyState State { get; }

    /// <summary> A name of the shortcut. </summary>
    public string Name { get; }

    /// <param name="key">Intercepted key. If the key specified as Ctrl/Shift/Alt/Win, then the corresponding modifier will be ignored.</param>
    /// <param name="modifier">A modifier of the intercepted shortcut. Use "|" to set multiple modifiers.</param>
    /// <param name="state">Intercepted state of the specified key.</param>
    /// <param name="name">A name of the shortcut.</param>
    public Shortcut(Key key, KeyModifier modifier = KeyModifier.None, KeyState state = KeyState.Up, string name = null)
    {
        if (key.IsCtrl && modifier.HasCtrl)
            modifier -= KeyModifier.Ctrl;
        else if (key.IsShift && modifier.HasShift)
            modifier -= KeyModifier.Shift;
        else if (key.IsAlt && modifier.HasAlt)
            modifier -= KeyModifier.Alt;
        else if (key.IsWin && modifier.HasWin)
            modifier -= KeyModifier.Win;

        Key = key;
        Modifier = modifier;
        State = state;
        Name = name;
    }

    /// <summary>
    /// Converts the string representation of a shortcut to the <see cref="Shortcut"/> value.
    /// </summary>
    /// <param name="shortcutStr">A string representation of a shortcut</param>
    /// <param name="state">A key state of the desired <see cref="Shortcut"/> value.</param>
    /// <returns>The result value of the conversion</returns>
    /// <exception cref="ArgumentException"/>
    public static Shortcut Parse(string shortcutStr, KeyState state)
    {
        if (!TryParse(shortcutStr, state, out var shortcut))
            throw new ArgumentException($"Failed to parse {shortcutStr}");

        return shortcut;
    }

    /// <summary>
    /// Converts the string representation of a shortcut to the <see cref="Shortcut"/> value.
    /// </summary>
    /// <param name="shortcutStr">A string representation of a shortcut</param>
    /// <param name="state">A key state of the desired <see cref="Shortcut"/> value.</param>
    /// <param name="shortcut">The result value of the conversion</param>
    /// <returns>true if <paramref name="shortcutStr"/> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string shortcutStr, KeyState state, out Shortcut shortcut)
    {
        shortcut = null;

        if (string.IsNullOrEmpty(shortcutStr))
            return false;

        var parts = Regex.Split(shortcutStr, @"[\s\+\-]+")
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

        if (parts.Length == 0)
            return false;

        var modifier = KeyModifier.None;

        for (int i = 0; i < parts.Length; i++)
        {
            if (i + 1 == parts.Length)
            {
                if (!Enum.TryParse<Key>(parts[i], true, out var parsedKey))
                    return false;

                shortcut = new Shortcut(parsedKey, modifier, state);
                return true;
            }

            modifier |= parts[i].ToLowerInvariant() switch
            {
                "ctrl" or "control" => KeyModifier.Ctrl,
                "shift" => KeyModifier.Shift,
                "alt" or "menu" => KeyModifier.Alt,
                "win" or "windows" => KeyModifier.Win,
                _ => KeyModifier.None
            };
        }

        return false;
    }

    /// <summary>
    /// Converts the <see cref="Shortcut"/> object to a string in format <b>Ctrl + Shift + E</b>.
    /// </summary>
    public override string ToString()
    {
        StringBuilder modifiersBuilder = new();

        if (Modifier.HasCtrl)
            modifiersBuilder.Append("Ctrl + ");
        if (Modifier.HasShift)
            modifiersBuilder.Append("Shift + ");
        if (Modifier.HasAlt)
            modifiersBuilder.Append("Alt + ");
        if (Modifier.HasWin)
            modifiersBuilder.Append("Win + ");

        modifiersBuilder.Append(Key.ToString());

        return string.IsNullOrEmpty(Name) ? modifiersBuilder.ToString() : $"{Name} ({modifiersBuilder})";
    }

    public override bool Equals(object obj)
    {
        if (obj is Shortcut shortcut)
        {
            return Key == shortcut.Key &&
                   Modifier == shortcut.Modifier &&
                   State == shortcut.State;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Key.GetHashCode();
            hash = hash * 23 + Modifier.GetHashCode();
            hash = hash * 23 + State.GetHashCode();
            return hash;
        }
    }

    public static bool operator ==(Shortcut left, Shortcut right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Shortcut left, Shortcut right)
    {
        return !(left == right);
    }
}
