using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Add(CCSPlayerController player, MenuBase menu, Action<CCSPlayerController, MenuBase, MenuAction>? callback = null)
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
            var currentMenu = Get(player);

            if (currentMenu is not null && (menu.Options.Priority >= currentMenu.Options.Priority || currentMenu.Options.Exitable == false))
                _menus[player].Insert(0, new([menu]));
            else
                _menus[player].Add(new([menu]));
        }
    }

    public static void Clear(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player, out var menus) || menus.Count == 0)
            return;

        for (var i = menus.Count; i > 0; i--)
        {
            var menuStack = menus[i - 1];

            if (menuStack.Count == 0)
                continue;

            var currentMenu = menuStack.Peek();

            if (currentMenu.Options.Exitable)
            {
                menuStack.Clear();
                _menus[player].RemoveAt(_menus[player].IndexOf(menuStack));
            }
        }

        _currentMenu.Remove(player, out _);
    }

    internal static void Remove(CCSPlayerController player)
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