using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private static readonly List<MenuObject> _header =
    [
        "new",
        new MenuObject("header", new MenuFormat(color: Color.Green)),
    ];

    private static readonly MenuOptions _options = new()
    {
        BlockMovement = true,
        DisplayItemsInHeader = true,
        Highlight = new MenuFormat { Color = Color.Green, Style = MenuStyle.Bold },
    };

    private void Example7Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuBase menu = new(header: _header, options: _options);

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                values:
                [
                    new("sub menu", callback: SubMenuCallback),
                    new("sub menu2", callback: SubMenu2Callback),
                ],
                options: new MenuItemOptions() { Pinwheel = false }
            )
        );

        Menu.Display(player, menu);
    }

    private void SubMenuCallback(MenuBase menu, MenuValue menuValue, MenuAction menuAction)
    {
        if (menuAction != MenuAction.Select)
        {
            return;
        }

        MenuBase subMenu = new(header: _header, options: _options);

        subMenu.Items.Add(
            new MenuItem(
                type: MenuItemType.Text,
                head: new MenuValue("sub menu text, use shift (walk)")
            )
        );

        Menu.Display(menu.Player, subMenu, true);
    }

    private void SubMenu2Callback(MenuBase menu, MenuValue menuValue, MenuAction menuAction)
    {
        if (menuAction != MenuAction.Select)
        {
            return;
        }

        MenuBase subMenu = new(header: _header, options: _options);

        subMenu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                head: new MenuValue("back button"),
                callback: (menu, menuItem, menuAction) =>
                {
                    if (menuAction == MenuAction.Select)
                    {
                        Menu.Close(menu.Player);
                    }
                }
            )
        );

        Menu.Display(menu.Player, subMenu, true);
    }
}
