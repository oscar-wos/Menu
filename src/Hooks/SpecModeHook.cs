using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using RMenu.Enums;

namespace RMenu.Hooks;

internal static class SpecModeHook
{
    private static readonly MemoryFunctionVoid<CPlayer_ObserverServices, int> _specMode = new(
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? "55 48 89 E5 41 57 45 31 FF 41 56 41 55 49 89 FD 41 54 53 89 F3 40 0F B6 F6 48 83 EC ?? 48 8B 07 FF 90 ?? ?? ?? ?? 85 C0 78 19 48 8D 15 ?? ?? ?? ?? 48 8B 12 44 8B 7A ?? 41 83 EF ?? 41 39 C7 44 0F 4F F8 80 FB ?? 45"
            : "48 89 5C 24 08 48 89 74 24 18 57 41"
    );

    public static void Register() => _specMode.Hook(SpecMode, HookMode.Pre);

    private static HookResult SpecMode(DynamicHook hook)
    {
        if (
            hook.GetParam<CPlayer_ObserverServices>(0)
                .Pawn.Value.Controller.Value?.As<CCSPlayerController>()
            is not { IsValid: true } player
        )
        {
            return HookResult.Continue;
        }

        if (Menu.Get(player) is not { } menu)
        {
            return HookResult.Continue;
        }

        if ((menu.Options.Buttons[MenuButton.Select] & PlayerButtons.Jump) == 0)
        {
            return HookResult.Continue;
        }

        Menu.Input(player, menu, PlayerButtons.Jump);
        return HookResult.Continue;
    }
}
