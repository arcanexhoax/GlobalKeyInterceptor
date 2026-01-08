using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests.Utils;

public class KeyModifierExtensionsTests
{
    [Theory]
    [InlineData("HasCtrl", KeyModifier.Ctrl, true)]
    [InlineData("HasCtrl", KeyModifier.Alt, false)]
    [InlineData("HasCtrl", KeyModifier.Ctrl | KeyModifier.Alt, true)]
    [InlineData("HasCtrl", KeyModifier.Shift | KeyModifier.Alt, false)]
    [InlineData("HasShift", KeyModifier.Shift, true)]
    [InlineData("HasShift", KeyModifier.Ctrl, false)]
    [InlineData("HasShift", KeyModifier.Ctrl | KeyModifier.Shift, true)]
    [InlineData("HasShift", KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Alt, false)]
    [InlineData("HasAlt", KeyModifier.Alt, true)]
    [InlineData("HasAlt", KeyModifier.Shift, false)]
    [InlineData("HasAlt", KeyModifier.Ctrl | KeyModifier.Alt | KeyModifier.Win, true)]
    [InlineData("HasAlt", KeyModifier.Ctrl | KeyModifier.Win, false)]
    [InlineData("HasWin", KeyModifier.Win, true)]
    [InlineData("HasWin", KeyModifier.Ctrl, false)]
    [InlineData("HasWin", KeyModifier.Ctrl | KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt, true)]
    [InlineData("HasWin", KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt, false)]
    public void AllKeyModifierExtensionProperties_WorkCorrectly(string propertyName, KeyModifier modifier, bool expected)
    {
        var actual = propertyName switch
        {
            "HasCtrl" => modifier.HasCtrl,
            "HasShift" => modifier.HasShift,
            "HasAlt" => modifier.HasAlt,
            "HasWin" => modifier.HasWin,
            _ => throw new ArgumentException("Invalid property name", nameof(propertyName))
        };

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Ctrl", KeyModifier.Ctrl, true)]
    [InlineData("shift", KeyModifier.Shift, true)]
    [InlineData("Alt", KeyModifier.Alt, true)]
    [InlineData("win", KeyModifier.Win, true)]
    [InlineData("control", KeyModifier.Ctrl, true)]
    [InlineData("menu", KeyModifier.Alt, true)]
    [InlineData("windows", KeyModifier.Win, true)]
    [InlineData("Ctrl + Shift", KeyModifier.None, false)]
    [InlineData("None", KeyModifier.None, false)]
    [InlineData("NonExistentModifier", KeyModifier.None, false)]
    public void TryFormattedParse_Works(string input, KeyModifier expected, bool shouldParse)
    {
        var result = KeyModifierExtensions.TryFormattedParse(input, out var modifier);
        Assert.Equal(shouldParse, result);

        if (shouldParse)
            Assert.Equal(expected, modifier);
        else
            Assert.Equal(KeyModifier.None, modifier);
    }

    [Fact]
    public void FormattedParse_ThrowsOnInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeyModifierExtensions.FormattedParse("NonExistentModifier"));
    }
}
