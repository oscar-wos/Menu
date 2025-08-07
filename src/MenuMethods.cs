using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public static partial class Menu
{
    public static void Display(
        CCSPlayerController player,
        MenuBase menu,
        Action<MenuBase, MenuAction>? callback = null
    )
    {
        _players[player.Slot] = player;

        menu.Player = player;
        menu.Callback = callback;

        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].SelectedValue is null && menu.Items[i]?.Values?[0] is { } value)
            {
                menu.Items[i].SelectedValue = (0, value);
            }

            if (menu.SelectedItem is null && MenuBase.IsSelectable(menu.Items[i]))
            {
                menu.SelectedItem = (i, menu.Items[i]);
            }
        }

        if (!_menus.TryAdd(player.Slot, [menu]))
        {
            if (
                Get(player) is { Options: { } options }
                && (menu.Options.Priority >= options.Priority || options.Exitable == false)
            )
            {
                _menus[player.Slot].Insert(0, menu);
            }
            else
            {
                _menus[player.Slot].Add(menu);
            }
        }

        menu.Callback?.Invoke(menu, MenuAction.Start);
    }

    public static MenuBase? Get(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player.Slot, out List<MenuBase>? menus))
        {
            return null;
        }

        if (menus.Count == 0)
        {
            return null;
        }

        return menus[0];
    }

    public static void Clear(CCSPlayerController player)
    {
        if (!_menus.TryGetValue(player.Slot, out List<MenuBase>? menus) || menus.Count == 0)
        {
            return;
        }

        for (int i = menus.Count; i > 0; i--)
        {
            MenuBase menu = menus[i - 1];

            if (menu.Options.Exitable)
            {
                _menus[player.Slot].RemoveAt(i - 1);
            }
        }

        _ = _currentMenus.Remove(player.Slot, out _);
    }

    public static void Remove(CCSPlayerController player)
    {
        _ = _menus.Remove(player.Slot, out _);
        _ = _currentMenus.Remove(player.Slot, out _);
    }

    internal static void Remove(int playerSlot)
    {
        _ = _menus.Remove(playerSlot, out _);
        _ = _currentMenus.Remove(playerSlot, out _);
    }
}
