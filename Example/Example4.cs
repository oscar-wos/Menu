using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private static readonly List<(string, string, int)> _example4Data =
    [
        ("Option", "1", 1),
        ("Option", "2", 2),
        ("Option", "3", 3),
    ];

    private void Example4Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuOptions options = new()
        {
            BlockMovement = true,
            Cursor =
            [
                new MenuObject("-", new MenuFormat(color: Color.Yellow)),
                new MenuObject("-", new MenuFormat(color: Color.Yellow)),
            ],
            Selector = [new MenuObject("( "), new MenuObject(" )")],
            Highlight = new MenuFormat(color: Color.Green, style: MenuStyle.Bold),
            HeaderFontSize = MenuFontSize.S,
            ItemFontSize = MenuFontSize.L,
            FooterFontSize = MenuFontSize.SM,
        };

        MenuBase menu = new(header: new("header"), footer: new("footer"), options: options);

        foreach ((string head, string tail, int data) in _example4Data)
        {
            menu.Items.Add(
                new MenuItem(
                    type: MenuItemType.Button,
                    head: new($"{head} "),
                    tail: new(tail, new MenuFormat(color: Color.Red, style: MenuStyle.Bold)),
                    data: data,
                    callback: Example4ItemCallback
                )
            );
        }

        menu.Items.Add(new(type: MenuItemType.Spacer));

        foreach ((string head, string tail, int data) in _example4Data)
        {
            menu.Items.Add(
                new MenuItem(
                    type: MenuItemType.Button,
                    head: new($"{head} "),
                    tail: new(tail, new MenuFormat(color: Color.Blue, style: MenuStyle.Bold)),
                    data: data,
                    callback: (menu, menuItem, menuAction) =>
                    {
                        if (menuAction == MenuAction.Select)
                        {
                            player.PrintToChat($"Select - Data: {menuItem.Data}");
                        }

                        if (menuAction == MenuAction.Update)
                        {
                            player.PrintToChat($"Update - Data: {menuItem.Data}");
                        }
                    }
                )
            );
        }

        Menu.Display(player, menu, callback: Example4MenuCallback);
    }

    private void Example4ItemCallback(MenuBase menu, MenuItem menuItem, MenuAction menuAction)
    {
        CCSPlayerController player = menu.Player;

        if (menuAction == MenuAction.Select)
        {
            player.PrintToChat($"Select - Data: {menuItem.Data}");
        }

        if (menuAction == MenuAction.Update)
        {
            player.PrintToChat($"Update - Data: {menuItem.Data}");
        }
    }

    private void Example4MenuCallback(MenuBase menu, MenuAction menuAction)
    {
        CCSPlayerController player = menu.Player;

        switch (menuAction)
        {
            case MenuAction.Start:
                player.PrintToChat("Menu Start");
                break;

            case MenuAction.Exit:
                player.PrintToChat("Menu Exit");
                break;

            case MenuAction.Select:
                Menu.Clear(player);
                break;
        }
    }
}
