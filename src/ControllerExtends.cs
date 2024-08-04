using CounterStrikeSharp.API.Core;

namespace Menus;

public static class ControllerExtends
{
    public static bool IsValid(this CCSPlayerController? controller)
    {
        return controller != null && controller.IsValid;
    }
}