using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private static readonly List<(string, string, int)> _example5Data =
    [
        ("Option ", "1", 1),
        ("Option ", "2", 2),
        ("Option trailing ", "3", 3),
    ];

    private void Example5Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuOptions options = new() { BlockMovement = true, DisplayItemsInHeader = false };
        MenuBase menu = new(header: new("header"), footer: new("footer"), options: options);

        List<MenuValue> values = [];

        foreach ((string head, string tail, int data) in _example5Data)
        {
            MenuValue value = FormatValue(head, tail);
            value.Data = data;

            values.Add(value);
        }

        menu.Items.Add(
            new MenuItem(type: MenuItemType.Button, head: new("Select: "), values: values)
        );

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                head: new("verylonghead verylonghead", new MenuFormat(color: Color.Blue)),
                tail: new(" short tail", new MenuFormat(color: Color.Red)),
                options: new MenuItemOptions() { Trim = MenuTrim.Head }
            )
        );

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                head: new("short head", new MenuFormat(color: Color.Blue)),
                tail: new(" verylongtail verylongtail", new MenuFormat(color: Color.Red)),
                options: new MenuItemOptions() { Trim = MenuTrim.Tail }
            )
        );

        Menu.Display(player, menu, Example5MenuCallback);
    }

    private static MenuValue FormatValue(string head, string tail)
    {
        List<MenuObject> value =
        [
            new($"{head} ", new MenuFormat(color: Color.Blue, style: MenuStyle.Bold)),
            new(tail, new MenuFormat(color: Color.Red, style: MenuStyle.Bold)),
        ];

        return value;
    }

    private void Example5MenuCallback(MenuBase menu, MenuAction menuAction)
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
