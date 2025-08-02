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
            ? "55 48 89 E5 41 57 49 89 FF 41 56 41 55 49 89 F5 41 54 53 48 83 EC ?? 48 8B 7F"
            : "48 89 5C 24 20 55 56 57 41 54 41 55 41 56 41 57 48 83 EC 20"
    );

    private static readonly MenuButton[] _menuButtons = Enum.GetValues<MenuButton>();

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

        for (int i = 0; i < _menuButtons.Length; i++)
        {
            MenuButton button = _menuButtons[i];
            ulong buttonMask = menu.Options.Buttons[button];

            if ((buttons & buttonMask) == buttonMask)
            {
                if (menu.InputDelay[i] + menu.Options.ButtonsDelay <= Environment.TickCount64)
                {
                    menu.InputDelay[i] = Environment.TickCount64;
                    menu.Input(player, button);
                }

                if (!menu.Options.Continuous[button])
                {
                    menu.InputDelay[i] = Environment.TickCount64;
                }
            }
        }

        return HookResult.Continue;
    }
}
