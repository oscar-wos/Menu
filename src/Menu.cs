using CounterStrikeSharp.API.Core;
using System.Collections.Concurrent;

namespace RMenu;

public static class Menu
{
    private static readonly ConcurrentDictionary<CCSPlayerController, List<Stack<MenuBase>>> _menus = [];
    private static readonly ConcurrentDictionary<CCSPlayerController, string> _currentMenu = [];
    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);

    static Menu()
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));
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

    public static void Add(CCSPlayerController player, MenuBase menu)
    {

    }

    public static void Remove(CCSPlayerController player, MenuBase menu)
    {
        
    }

    public static void Clear(CCSPlayerController player)
    {

    }
}