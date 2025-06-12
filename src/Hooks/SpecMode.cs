﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using RMenu.Enums;
using System.Runtime.InteropServices;

namespace RMenu.Hooks;

internal static class SpecModeHook
{
    private static readonly MemoryFunctionVoid<CPlayer_ObserverServices, int> _specMode = new(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "55 48 89 E5 41 57 41 56 41 55 49 89 FD 41 54 53 89 F3 40" : "48 89 74 24 18 55 41 56 41 57 48 8D AC");

    public static void Register()
    {
        _specMode.Hook(SpecModePre, HookMode.Pre);
    }

    private static HookResult SpecModePre(DynamicHook h)
    {
        var player = h.GetParam<CPlayer_ObserverServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();

        if (player is null || !player.IsValid)
            return HookResult.Continue;

        var menu = Menu.Get(player);

        if (menu is null || !menu.Options.ProcessInput)
            return HookResult.Continue;

        menu.Input(player, MenuButton.Select);
        return HookResult.Stop;
    }
}