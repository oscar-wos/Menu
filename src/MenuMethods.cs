using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Add(CCSPlayerController player, MenuBase menu, Action<CCSPlayerController, MenuBase, MenuAction, MenuItem?> callback)
    {
        menu.Callback = callback;

        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (MenuBase.IsSelectable(menu.Items[i]))
            {
                menu.SelectedItem = (i, menu.Items[i]);
                break;
            }
        }

        if (!_menus.TryAdd(player, [new([menu])]))
        {
            // Priorities
        }
    }

    public static void Clear(CCSPlayerController player)
    {
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