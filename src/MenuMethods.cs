using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Add(CCSPlayerController player, MenuBase menu, Action<MenuBase, MenuAction, MenuItem> callback)
    {

    }

    public static void Clear(CCSPlayerController player)
    {

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