using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using RMenu.Enums;
using RMenu.Structs;

namespace RMenu.Hooks;

internal static class RunCommandHook
{
    private static readonly MemoryFunctionVoid<CPlayer_MovementServices, IntPtr> _runCommand = new(
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? "55 48 89 E5 41 57 49 89 FF 41 56 48 8D 55"
            : "40 53 56 57 48 81 EC 80 00 00 00 0F"
    );

    public static void Register() => _runCommand.Hook(RunCommandPre, HookMode.Pre);

    private static unsafe HookResult RunCommandPre(DynamicHook h)
    {
        if (
            h.GetParam<CPlayer_MovementServices>(0)
                .Pawn.Value.Controller.Value?.As<CCSPlayerController>()
            is not CCSPlayerController { IsValid: true } player
        )
        {
            return HookResult.Continue;
        }

        if (Menu.Get(player) is not MenuBase { Options.ProcessInput: true } menu)
        {
            return HookResult.Continue;
        }

        CUserCmd* userCmd = (CUserCmd*)h.GetParam<IntPtr>(1);

        if (menu.Options.BlockMovement)
        {
            userCmd->BaseUserCmd->m_flForwardMove = 0;
            userCmd->BaseUserCmd->m_flSideMove = 0;
            userCmd->BaseUserCmd->m_flUpMove = 0;
        }

        ulong buttons = userCmd->ButtonState.PressedButtons | userCmd->ButtonState.ScrollButtons;

        foreach (MenuButton button in Enum.GetValues(typeof(MenuButton)))
        {
            if ((buttons & menu.Options.Buttons[button]) == menu.Options.Buttons[button])
            {
                if (
                    menu.InputDelay[(int)button] + menu.Options.ButtonsDelay
                    > Environment.TickCount64
                )
                {
                    continue;
                }

                menu.InputDelay[(int)button] = Environment.TickCount64;
                menu.Input(player, button);
            }
        }

        return HookResult.Continue;
    }
}
