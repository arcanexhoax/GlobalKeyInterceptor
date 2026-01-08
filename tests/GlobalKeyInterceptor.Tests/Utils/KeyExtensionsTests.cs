using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests.Utils;

public class KeyExtensionsTests
{
    [Theory]
    [InlineData("IsCtrl", Key.Ctrl, true)]
    [InlineData("IsCtrl", Key.LeftCtrl, true)]
    [InlineData("IsCtrl", Key.RightCtrl, true)]
    [InlineData("IsCtrl", Key.A, false)]
    [InlineData("IsShift", Key.Shift, true)]
    [InlineData("IsShift", Key.LeftShift, true)]
    [InlineData("IsShift", Key.RightShift, true)]
    [InlineData("IsShift", Key.A, false)]
    [InlineData("IsAlt", Key.Alt, true)]
    [InlineData("IsAlt", Key.LeftAlt, true)]
    [InlineData("IsAlt", Key.RightAlt, true)]
    [InlineData("IsAlt", Key.A, false)]
    [InlineData("IsWin", Key.LeftWindows, true)]
    [InlineData("IsWin", Key.RightWindows, true)]
    [InlineData("IsWin", Key.A, false)]
    [InlineData("IsModifier", Key.Ctrl, true)]
    [InlineData("IsModifier", Key.LeftShift, true)]
    [InlineData("IsModifier", Key.RightAlt, true)]
    [InlineData("IsModifier", Key.LeftWindows, true)]
    [InlineData("IsModifier", Key.A, false)]
    [InlineData("IsDigit", Key.D0, true)]
    [InlineData("IsDigit", Key.D9, true)]
    [InlineData("IsDigit", Key.A, false)]
    [InlineData("IsDigit", Key.Help, false)]
    [InlineData("IsFunctionKey", Key.F1, true)]
    [InlineData("IsFunctionKey", Key.F24, true)]
    [InlineData("IsFunctionKey", Key.NumDivide, false)]
    [InlineData("IsFunctionKey", Key.NumLock, false)]
    [InlineData("IsNumpadDigit", Key.Num0, true)]
    [InlineData("IsNumpadDigit", Key.Num9, true)]
    [InlineData("IsNumpadDigit", Key.Sleep, false)]
    [InlineData("IsNumpadDigit", Key.NumMultiply, false)]
    [InlineData("IsNumpadKey", Key.Num0, true)]
    [InlineData("IsNumpadKey", Key.NumDecimal, true)]
    [InlineData("IsNumpadKey", Key.NumEnter, true)]
    [InlineData("IsNumpadKey", Key.A, false)]
    [InlineData("IsArrowKey", Key.LeftArrow, true)]
    [InlineData("IsArrowKey", Key.NumDownArrow, true)]
    [InlineData("IsArrowKey", Key.StandardRightArrow, true)]
    [InlineData("IsArrowKey", Key.A, false)]
    [InlineData("IsNavigationKey", Key.Home, true)]
    [InlineData("IsNavigationKey", Key.NumPageDown, true)]
    [InlineData("IsNavigationKey", Key.StandardEnd, true)]
    [InlineData("IsNavigationKey", Key.A, false)]
    [InlineData("IsLetter", Key.A, true)]
    [InlineData("IsLetter", Key.Z, true)]
    [InlineData("IsLetter", Key.D9, false)]
    [InlineData("IsLetter", Key.LeftWindows, false)]
    [InlineData("IsCharacterKey", Key.A, true)]
    [InlineData("IsCharacterKey", Key.D0, true)]
    [InlineData("IsCharacterKey", Key.Num0, true)]
    [InlineData("IsCharacterKey", Key.Space, true)]
    [InlineData("IsCharacterKey", Key.Ctrl, false)]
    public void AllKeyExtensionProperties_WorkCorrectly(string propertyName, Key key, bool expected)
    {
        var actual = propertyName switch
        {
            "IsCtrl" => key.IsCtrl,
            "IsShift" => key.IsShift,
            "IsAlt" => key.IsAlt,
            "IsWin" => key.IsWin,
            "IsModifier" => key.IsModifier,
            "IsDigit" => key.IsDigit,
            "IsFunctionKey" => key.IsFunctionKey,
            "IsNumpadDigit" => key.IsNumpadDigit,
            "IsNumpadKey" => key.IsNumpadKey,
            "IsArrowKey" => key.IsArrowKey,
            "IsNavigationKey" => key.IsNavigationKey,
            "IsLetter" => key.IsLetter,
            "IsCharacterKey" => key.IsCharacterKey,
            _ => throw new ArgumentException("Invalid property name", nameof(propertyName))
        };
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(Key.StandardEnter, Key.Enter)]
    [InlineData(Key.NumEnter, Key.Enter)]
    [InlineData(Key.Enter, Key.Enter)]
    [InlineData(Key.A, Key.A)]
    [InlineData(Key.Ctrl, Key.Ctrl)]
    public void BaseKey_ReturnsExpected(Key key, Key expectedBase)
    {
        Assert.Equal(expectedBase, key.BaseKey);
    }

    [Theory]
    [InlineData(Key.D0, "0")]
    [InlineData(Key.A, "A")]
    [InlineData(Key.Colon, ";")]
    [InlineData(Key.Plus, "=")]
    [InlineData(Key.Comma, ",")]
    [InlineData(Key.Minus, "-")]
    [InlineData(Key.Period, ".")]
    [InlineData(Key.Slash, "/")]
    [InlineData(Key.Tilde, "`")]
    [InlineData(Key.OpenBracket, "[")]
    [InlineData(Key.BackSlash, "\\")]
    [InlineData(Key.ClosingBracket, "]")]
    [InlineData(Key.Quote, "'")]
    [InlineData(Key.StandardEnter, "Enter")]
    public void ToFormattedString_ReturnsExpected(Key key, string expected)
    {
        Assert.Equal(expected, key.ToFormattedString());
    }

    [Theory]
    [InlineData("A", Key.A, true)]
    [InlineData("a", Key.A, true)]
    [InlineData("0", Key.D0, true)]
    [InlineData(";", Key.Colon, true)]
    [InlineData("=", Key.Plus, true)]
    [InlineData("+", Key.Plus, true)]
    [InlineData("Enter", Key.Enter, true)]
    [InlineData("numenter", Key.NumEnter, true)]
    [InlineData("Standardenter", Key.StandardEnter, true)]
    [InlineData("NonExistentKey", default(Key), false)]
    public void TryFormattedParse_Works(string input, Key expected, bool shouldParse)
    {
        var result = Key.TryFormattedParse(input, out var key);
        Assert.Equal(shouldParse, result);

        if (shouldParse)
            Assert.Equal(expected, key);
        else
            Assert.Equal(default, key);
    }

    [Fact]
    public void FormattedParse_ThrowsOnInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeyExtensions.FormattedParse("NonExistentKey"));
    }
}
