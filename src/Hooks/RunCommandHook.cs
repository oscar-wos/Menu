using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using RMenu.Structs;

namespace RMenu.Hooks;

internal static class RunCommandHook
{
    private static readonly MemoryFunctionVoid<CPlayer_MovementServices, IntPtr> _runCommand = new(
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? "55 48 89 E5 41 57 49 89 FF 41 56 41 55 49 89 F5 41 54 53 48 83 EC ?? 48 8B 7F"
            : "48 89 5C 24 20 55 56 57 41 54 41 55 41 56 41 57 48 83 EC 20"
    );

    public static void Register() => _runCommand.Hook(RunCommand, HookMode.Pre);

    private static unsafe HookResult RunCommand(DynamicHook hook)
    {
        if (
            hook.GetParam<CPlayer_MovementServices>(0)
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

        CUserCmd* userCmd = (CUserCmd*)hook.GetParam<IntPtr>(1);

        if (menu.Options.BlockMovement)
        {
            userCmd->m_pBaseUserCmd->m_flForwardMove = 0;
            userCmd->m_pBaseUserCmd->m_flSideMove = 0;
            userCmd->m_pBaseUserCmd->m_flUpMove = 0;
        }

        PlayerButtons buttons = (PlayerButtons)(
            userCmd->m_InButtonState.m_nPressedButtons | userCmd->m_InButtonState.m_nScrollButtons
        );

        Menu.Input(player, menu, buttons);
        return HookResult.Continue;
    }
}
