using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Add(CCSPlayerController player, MenuBase menu, Action<CCSPlayerController, MenuBase, MenuAction, MenuItem?> callback)
    {
        if (!_menus.TryGetValue(player, out _))
            _menus[player] = [];

        if (_menus[player].Count == 0)
            _menus[player].Add(new Stack<MenuBase>());

        menu.Callback = callback;
        var menuStack = _menus[player].ElementAt(0);
        menuStack.Push(menu);
    }

    public static void Clear(CCSPlayerController player)
    {
        Console.WriteLine("Cleared");

        _menus.Remove(player, out _);
        _currentMenu.Remove(player, out _);
    }

    public static MenuBase? Get(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player, out var menus))
            return null;

        if (menus.Count == 0)
            return null;

        var currentMenuStack = menus.ElementAt(0);

        if (currentMenuStack.Count == 0)
            return null;

        return currentMenuStack.Peek();
    }
}