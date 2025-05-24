using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using RMenu.Enums;
using RMenu.Structs;
using System.Collections.Concurrent;

namespace RMenu;

public static partial class Menu
{
    private static readonly Dictionary<CCSPlayerController, List<Stack<MenuBase>>> _menus = [];
    private static readonly Dictionary<CCSPlayerController, (MenuBase, string)> _currentMenu = [];
    private static readonly MemoryFunctionVoid<CPlayer_MovementServices, IntPtr> _runCommand = new("40 53 56 57 48 81 EC 80 00 00 00 0F");
    private static readonly MemoryFunctionVoid<CPlayer_ObserverServices, int> _changeSpecMode = new("48 89 74 24 18 55 41 56 41 57 48 8D AC");
    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);
    public static event EventHandler<MenuEvent>? OnPrintMenuPre;

    static Menu()
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));
        _runCommand.Hook(RunCommand, HookMode.Pre);
        _changeSpecMode.Hook(ChangeSpecMode, HookMode.Pre);
    }

    private static void OnTick()
    {
        foreach (var (player, (menu, menuString)) in _currentMenu)
        {
            if (player.Connected != PlayerConnectedState.PlayerConnected)
            {
                _menus.Remove(player, out _);
                _currentMenu.Remove(player, out _);
                continue;
            }

            var menuEvent = new MenuEvent(player, menu, menuString);

            OnPrintMenuPre?.Invoke(null, menuEvent);
            player.PrintToCenterHtml(menuString);
        }
    }

    private unsafe static HookResult RunCommand(DynamicHook h)
    {
        var player = h.GetParam<CPlayer_MovementServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();

        if (player == null || !player.IsValid)
            return HookResult.Continue;

        var menu = Get(player);

        if (menu == null || !menu.Options.ProcessInput)
            return HookResult.Continue;

        var userCmd = (CUserCmd*)h.GetParam<IntPtr>(1);

        if (menu.Options.BlockMovement)
        {
            userCmd->BaseUserCmd->m_flForwardMove = 0;
            userCmd->BaseUserCmd->m_flSideMove = 0;
            userCmd->BaseUserCmd->m_flUpMove = 0;
        }

        var buttons = (ulong)userCmd->ButtonState.PressedButtons | (ulong)userCmd->ButtonState.ScrollButtons;

        foreach (MenuButton button in Enum.GetValues(typeof(MenuButton)))
        {
            if ((buttons & menu.Options.Buttons[button]) == menu.Options.Buttons[button])
            {
                if (menu.InputDelay[(int)button] + menu.Options.ButtonsDelay > Environment.TickCount64)
                    continue;

                Console.WriteLine(buttons);

                menu.InputDelay[(int)button] = Environment.TickCount64;
                menu.Input(player, button);
            }
        }

        return HookResult.Continue;
    }

    private static HookResult ChangeSpecMode(DynamicHook h)
    {
        var player = h.GetParam<CPlayer_ObserverServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();

        if (player == null || !player.IsValid)
            return HookResult.Continue;

        var menu = Get(player);

        if (menu == null || !menu.Options.ProcessInput)
            return HookResult.Continue;

        menu.Input(player, MenuButton.Select);
        return HookResult.Stop;
    }

    private static void ProcessMenu(object? state)
    {
        Parallel.ForEach(_menus, kvp =>
        {
            var player = kvp.Key;

            if (!player.IsValid)
                return;

            var menu = Get(player);

            if (menu == null)
                return;

            var html = $"\u00A0{menu.Title}";
            _currentMenu[player] = (menu, html);
        });
    }
}