using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests;

public class ShortcutTests
{
    [Fact]
    public void ModifierSetAsSimpleKey_IsIgnoredAsModifier()
    {
        var shortcut = new Shortcut(Key.Ctrl, KeyModifier.Ctrl | KeyModifier.Alt | KeyModifier.Shift | KeyModifier.Win);
        Assert.Equal(Key.Ctrl, shortcut.Key);
        Assert.False(shortcut.Modifier.HasCtrl);
        Assert.True(shortcut.Modifier.HasAlt);
        Assert.True(shortcut.Modifier.HasShift);
        Assert.True(shortcut.Modifier.HasWin);

        shortcut = new Shortcut(Key.LeftShift, KeyModifier.Shift | KeyModifier.Ctrl);
        Assert.Equal(Key.LeftShift, shortcut.Key);
        Assert.False(shortcut.Modifier.HasShift);
        Assert.True(shortcut.Modifier.HasCtrl);

        shortcut = new Shortcut(Key.Alt, KeyModifier.Shift | KeyModifier.Ctrl);
        Assert.Equal(Key.Alt, shortcut.Key);
        Assert.True(shortcut.Modifier.HasShift);
        Assert.True(shortcut.Modifier.HasCtrl);
    }

    [Theory]
    [InlineData("Ctrl + Shift + E", Key.E, KeyModifier.Ctrl | KeyModifier.Shift, KeyState.Up)]
    [InlineData("alt shift win ctrl e", Key.E, KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt, KeyState.Down)]
    [InlineData("e", Key.E, KeyModifier.None, KeyState.Down)]
    public void TryParse_ParsesShortcutStrings(string input, Key expectedKey, KeyModifier expectedModifier, KeyState state)
    {
        var result = Shortcut.TryParse(input, state, out var shortcut);
        Assert.True(result);
        Assert.Equal(expectedKey, shortcut.Key);
        Assert.Equal(expectedModifier, shortcut.Modifier);
        Assert.Equal(state, shortcut.State);
    }

    [Fact]
    public void TryParse_InvalidShortcutString_ReturnsFalse()
    {
        var result = Shortcut.TryParse("e ctrl shift", KeyState.Up, out var shortcut);
        Assert.False(result);
    }

    [Fact]
    public void Parse_InvalidShortcutString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Shortcut.Parse("e ctrl shift", KeyState.Up));
    }

    [Theory]
    [InlineData(Key.A, KeyModifier.None, "A")]
    [InlineData(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, "Ctrl + Shift + E")]
    [InlineData(Key.Z, KeyModifier.Alt | KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift, "Win + Ctrl + Shift + Alt + Z")]
    public void ToString_ProducesExpectedFormat(Key key, KeyModifier modifier, string expected)
    {
        var shortcut = new Shortcut(key, modifier);
        Assert.Equal(expected, shortcut.ToString());
    }
}
