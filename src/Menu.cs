using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Collections.Concurrent;

namespace RMenu;

public static partial class Menu
{
    private static readonly ConcurrentDictionary<CCSPlayerController, List<Stack<MenuBase>>> _menus = [];
    private static readonly ConcurrentDictionary<CCSPlayerController, string> _currentMenu = [];
    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);
    private static readonly MemoryFunctionVoid<IntPtr, IntPtr> _runCommand = new("40 53 56 57 48 81 EC 80 00 00 00 0F");
    private static readonly MemoryFunctionVoid<IntPtr, int> _changeSpecMode = new("48 89 74 24 18 55 41 56 41 57 48 8D AC");


    static Menu()
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));
        _runCommand.Hook(RunCommand, HookMode.Pre);
        _changeSpecMode.Hook(ChangeSpecMode, HookMode.Pre);
    }

    private static HookResult RunCommand(DynamicHook h)
    {
        return HookResult.Continue;
    }

    private static HookResult ChangeSpecMode(DynamicHook h)
    {
        return HookResult.Continue;
    }

    private static void ProcessMenu(object? state)
    {
        Parallel.ForEach(_menus, kvp =>
        {
            var player = kvp.Key;

            if (!player.IsValid)
                return;

            _currentMenu[player] = string.Empty;
        });
    }

    private static void OnTick()
    {
        Parallel.ForEach(_currentMenu, kvp =>
        {
            var player = kvp.Key;

            if (!player.IsValid)
            {
                _menus.TryRemove(player, out _);
                _currentMenu.TryRemove(player, out _);
                return;
            }

            var menuString = kvp.Value;

            if (menuString != string.Empty)
                player.PrintToCenterHtml(menuString);
        });
    }
}