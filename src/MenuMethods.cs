using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Add(
        CCSPlayerController player,
        MenuBase menu,
        Action<CCSPlayerController, MenuBase, MenuAction>? callback = null
    )
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

        if (!_menus.TryAdd(player, [menu]))
        {
            if (
                Get(player) is MenuBase { Options: { } options }
                && (menu.Options.Priority >= options.Priority || options.Exitable == false)
            )
            {
                _menus[player].Insert(0, menu);
            }
            else
            {
                _menus[player].Add(menu);
            }
        }
    }

    public static void Clear(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player, out List<MenuBase>? menus) || menus.Count == 0)
        {
            return;
        }

        for (int i = menus.Count; i > 0; i--)
        {
            MenuBase menu = menus[i - 1];

            if (menu.Options.Exitable)
            {
                _menus[player].RemoveAt(i - 1);
            }
        }

        _ = _currentMenu.Remove(player, out _);
    }

    public static void Remove(CCSPlayerController player)
    {
        _ = _menus.Remove(player, out _);
        _ = _currentMenu.Remove(player, out _);
    }

    public static MenuBase? Get(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player, out List<MenuBase>? menus))
        {
            return null;
        }

        if (menus.Count == 0)
        {
            return null;
        }

        return menus[0];
    }
}
