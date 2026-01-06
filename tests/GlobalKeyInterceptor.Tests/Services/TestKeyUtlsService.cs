using GlobalKeyInterceptor.Utils;

namespace GlobalKeyInterceptor.Tests.Services;

internal class TestKeyUtilsService : IKeyUtilsService
{
    public bool IsCtrlPressed { get; set; }

    public bool IsShiftPressed { get; set; }

    public bool IsAltPressed { get; set; }

    public bool IsWinPressed { get; set; }
}
