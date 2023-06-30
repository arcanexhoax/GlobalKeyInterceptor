using GlobalKeyInterceptor.Enum;

namespace GlobalKeyInterceptor.Util
{
    internal class KeyUtil
    {
        public static bool IsCtrl(Key key) => key == Key.Ctrl || key == Key.LeftCtrl || key == Key.RightCtrl;

        public static bool IsShift(Key key) => key == Key.Shift || key == Key.LeftShift || key == Key.RightShift;

        public static bool IsAlt(Key key) => key == Key.Alt || key == Key.LeftAlt || key == Key.RightAlt;
    }
}
