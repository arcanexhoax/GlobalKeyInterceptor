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
    [InlineData("Home", Key.Home, KeyModifier.None, KeyState.Up)]
    [InlineData("NumLeftArrow", Key.NumLeftArrow, KeyModifier.None, KeyState.Down)]
    [InlineData("StandardPageUp", Key.StandardPageUp, KeyModifier.None, KeyState.Up)]
    public void TryParse_ParsesShortcutStrings(string input, Key expectedKey, KeyModifier expectedModifier, KeyState state)
    {
        var result = Shortcut.TryParse(input, state, out var shortcut);
        Assert.True(result);
        Assert.NotNull(shortcut);
        Assert.Equal(expectedKey, shortcut.Key);
        Assert.Equal(expectedModifier, shortcut.Modifier);
        Assert.Equal(state, shortcut.State);
    }

    [Fact]
    public void TryParse_InvalidShortcutString_ReturnsFalse()
    {
        var result = Shortcut.TryParse("e ctrl shift", KeyState.Up, out var shortcut);
        Assert.False(result);
        Assert.Null(shortcut);
    }

    [Fact]
    public void Parse_InvalidShortcutString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Shortcut.Parse("e ctrl shift", KeyState.Up));
    }

    [Theory]
    [InlineData(Key.A, KeyModifier.None, "A")]
    [InlineData(Key.D3, KeyModifier.None, "D3")]
    [InlineData(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, "Ctrl + Shift + E")]
    [InlineData(Key.StandardEnter, KeyModifier.Alt | KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift, "Win + Ctrl + Shift + Alt + StandardEnter")]
    [InlineData(Key.NumEnd, KeyModifier.None, "NumEnd")]
    [InlineData(Key.Delete, KeyModifier.None, "Delete")]
    public void ToString_ProducesExpectedFormat(Key key, KeyModifier modifier, string expected)
    {
        var shortcut = new Shortcut(key, modifier);
        Assert.Equal(expected, shortcut.ToString());
    }

    [Theory]
    [InlineData(Key.A, KeyModifier.None, "A")]
    [InlineData(Key.D3, KeyModifier.None, "3")]
    [InlineData(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, "Ctrl + Shift + E")]
    [InlineData(Key.StandardEnter, KeyModifier.Alt | KeyModifier.Win | KeyModifier.Ctrl | KeyModifier.Shift, "Win + Ctrl + Shift + Alt + Enter")]
    [InlineData(Key.NumEnd, KeyModifier.None, "NumEnd")]
    [InlineData(Key.Delete, KeyModifier.None, "Delete")]
    public void ToFormattedString_ProducesExpectedFormat(Key key, KeyModifier modifier, string expected)
    {
        var shortcut = new Shortcut(key, modifier);
        Assert.Equal(expected, shortcut.ToFormattedString());
    }

    [Fact]
    public void Equals_And_OperatorEquals_WorkCorrectly()
    {
        var s1 = new Shortcut(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, KeyState.Up);
        var s2 = new Shortcut(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, KeyState.Up);
        var s3 = new Shortcut(Key.E, KeyModifier.Ctrl, KeyState.Up);
        var s4 = new Shortcut(Key.A, KeyModifier.Ctrl | KeyModifier.Shift, KeyState.Up);
        var s5 = new Shortcut(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, KeyState.Down);

        Assert.True(s1.Equals(s2));
        Assert.True(s1 == s2);

        Assert.False(s1.Equals(s3));
        Assert.False(s1 == s3);

        Assert.False(s1.Equals(s4));
        Assert.False(s1 == s4);

        Assert.False(s1.Equals(s5));
        Assert.False(s1 == s5);

        Assert.True(s1 != s3);
        Assert.True(s1 != s4);
        Assert.True(s1 != s5);
    }
}
