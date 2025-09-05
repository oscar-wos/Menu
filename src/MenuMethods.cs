using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using RMenu.Enums;

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

            if (menu.SelectedItem is null && MenuBase.IsSelectable(menuItem))
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

        if (subMenu && menuData.Menus.Count > 0)
        {
            menuData.Menus[0].Push(menu);
            menuData.Update();
            return;
        }

        Stack<MenuBase> menuStack = new([menu]);

        if (!subMenu && menuData.Menus.Count > 0 && menuData.Menus[0].Count > 0)
        {
            MenuBase menuParent = menuData.Menus[0].Last();

            if (
                menu.Options.Priority >= menuParent.Options.Priority
                || !menuParent.Options.Exitable
            )
            {
                menuData.Menus.Insert(0, menuStack);
                menuData.Update();
                return;
            }
        }

        menuData.Menus.Add(menuStack);
        menuData.Update();
    }

    public static MenuBase? Get(CCSPlayerController player, bool parent = false)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return null;
        }

        if (menuData.Menus.Count == 0 || menuData.Menus[0].Count == 0)
        {
            return null;
        }

        return parent ? menuData.Menus[0].Last() : menuData.Menus[0].Peek();
    }

    public static void Close(CCSPlayerController player, bool force = false)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return;
        }

        if (menuData.Menus.Count == 0 || menuData.Menus[0].Count < 2)
        {
            return;
        }

        _ = menuData.Menus[0].Pop();
        menuData.Update();
    }

    public static void Clear(CCSPlayerController player, bool force = false)
    {
        if (GetData(player.Slot) is not { } menuData)
        {
            return;
        }

        for (int i = menuData.Menus.Count; i > 0; i--)
        {
            Stack<MenuBase> menuStack = menuData.Menus[i - 1];

            if (menuStack.Count != 0 && !menuStack.Last().Options.Exitable && !force)
            {
                continue;
            }

            menuData.Menus.RemoveAt(i - 1);
        }

        menuData.Update();
    }

    internal static void Input(MenuBase menu, PlayerButtons buttons)
    {
        if (GetData(menu.Player.Slot) is not { } menuData)
        {
            return;
        }

        MenuBase menuParent = menuData.Menus[0].Last();

        for (int i = 0; i < _menuButtons.Length; i++)
        {
            MenuButton button = _menuButtons[i];
            PlayerButtons buttonMask = menu.Options.Buttons[button];

            if (buttonMask != 0 && (buttons & buttonMask) != 0)
            {
                if (
                    menuData._lastInput[i] + menuParent.Options.ButtonsDelay
                    > Environment.TickCount64
                )
                {
                    if (!menuParent.Options.Continuous[button])
                    {
                        menuData._lastInput[i] = Environment.TickCount64;
                    }

                    continue;
                }

                menuData._lastInput[i] = Environment.TickCount64;
                menu.Input(button);
            }
        }
    }

    internal static void Remove(int playerSlot) => _menuData[playerSlot] = null;

    internal static MenuData? GetData(int playerSlot) => _menuData[playerSlot];
}
