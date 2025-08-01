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
        _players[player.Slot] = player;
        menu.Callback = callback;

        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i]?.Values?[0] is MenuValue value && menu.Items[i].SelectedValue is null)
            {
                menu.Items[i].SelectedValue = (0, value);
            }

            if (MenuBase.IsSelectable(menu.Items[i]) && menu.SelectedItem is null)
            {
                menu.SelectedItem = (i, menu.Items[i]);
            }
        }

        if (!_menus.TryAdd(player.Slot, [menu]))
        {
            if (
                Get(player) is MenuBase { Options: { } options }
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

        menu.Callback?.Invoke(player, menu, MenuAction.Start);
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

        _ = _currentMenu.Remove(player.Slot, out _);
    }

    public static void Remove(CCSPlayerController player)
    {
        _ = _menus.Remove(player.Slot, out _);
        _ = _currentMenu.Remove(player.Slot, out _);
    }

    internal static void Remove(int playerSlot)
    {
        _ = _menus.Remove(playerSlot, out _);
        _ = _currentMenu.Remove(playerSlot, out _);
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
}
