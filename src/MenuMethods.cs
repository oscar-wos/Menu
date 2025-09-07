using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Models;

namespace RMenu;

public static partial class Menu
{
    private static readonly MenuButton[] _menuButtons = Enum.GetValues<MenuButton>();

    public static void Display(
        CCSPlayerController player,
        MenuBase menu,
        bool subMenu = false,
        Action<MenuBase, MenuAction>? callback = null
    )
    {
        menu.Player = player;
        menu.Callback = callback;

        for (int i = 0; i < menu.Items.Count; i++)
        {
            MenuItem menuItem = menu.Items[i];

            if (menu.SelectedItem is null && IsSelectable(menuItem))
            {
                menu.SelectedItem = new MenuSelectedItem(i, menuItem);
            }

            menuItem.Callback?.Invoke(menu, menuItem, MenuAction.Start);
        }

        menu.Callback?.Invoke(menu, MenuAction.Start);

        if (GetData(player.Slot) is not { } menuData)
        {
            menuData = new(player);
            _menuData[player.Slot] = menuData;
        }

        if (subMenu && menuData.Menus.Count > 0 && menuData.Menus[0].Count > 0)
        {
            MenuOptions options = new(menuData.Menus[0][^1].Options);
            options.Merge(menu.Options);

            menu.Options = options;
            menuData.Menus[0].Add(menu);
        }
        else
        {
            List<MenuBase> menuStack = [menu];

            bool isInsert =
                !subMenu
                && Get(player) is { } parent
                && (menu.Options.Priority >= parent.Options.Priority || !parent.Options.Exitable);

            if (isInsert)
            {
                menuData.Menus.Insert(0, menuStack);
            }
            else
            {
                menuData.Menus.Add(menuStack);
            }
        }

        menuData.Update();
    }

    public static MenuBase? Get(CCSPlayerController player)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return null;
        }

        if (menuData.Menus.Count == 0 || menuData.Menus[0].Count == 0)
        {
            return null;
        }

        return menuData.Menus[0][^1];
    }

    public static bool Close(CCSPlayerController player)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return false;
        }

        if (menuData.Menus.Count == 0 || menuData.Menus[0].Count < 2)
        {
            return false;
        }

        menuData.Menus[0].RemoveAt(menuData.Menus[0].Count - 1);
        menuData.Update();
        return true;
    }

    public static void Clear(CCSPlayerController player, bool force = false)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return;
        }

        for (int i = menuData.Menus.Count; i > 0; i--)
        {
            List<MenuBase> menuStack = menuData.Menus[i - 1];

            if (menuStack.Count != 0 && !menuStack[^1].Options.Exitable && !force)
            {
                continue;
            }

            menuData.Menus.RemoveAt(i - 1);
        }

        menuData.Update();
    }

    internal static void Input(CCSPlayerController player, MenuBase menu, PlayerButtons buttons)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return;
        }

        if (menuData.Menus.Count == 0 || menuData.Menus[0].Count == 0)
        {
            return;
        }

        for (int i = 0; i < _menuButtons.Length; i++)
        {
            MenuButton button = _menuButtons[i];
            PlayerButtons buttonMask = menu.Options.Buttons[button];

            if (buttonMask == 0)
            {
                continue;
            }

            bool isPressed = (buttons & buttonMask) != 0;

            int continuousDelay =
                menu.SelectedItem?.Item.Options.Continuous?[button]
                ?? menu.Options.Continuous[button];

            if (!isPressed)
            {
                menuData._lastInput[i] = 0;
                continue;
            }

            if (
                continuousDelay == 0
                    ? menuData._lastInput[i] != 0
                    : menuData._lastInput[i] + continuousDelay > Environment.TickCount64
            )
            {
                continue;
            }

            menuData._lastInput[i] = Environment.TickCount64;
            menu.Input(button);
        }
    }

    internal static void Remove(int playerSlot) => _menuData[playerSlot] = null;

    internal static MenuData? GetData(int playerSlot) => _menuData[playerSlot];

    internal static bool IsSelectable(MenuItem menuItem) =>
        menuItem.Type is MenuItemType.Choice or MenuItemType.Button;
}
