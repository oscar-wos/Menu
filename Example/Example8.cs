using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private void Example8Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuBase menu = new();

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                values: [.. Enumerable.Range(1, 100).Select(i => new MenuValue(i.ToString()))],
                options: new MenuItemOptions()
                {
                    Pinwheel = false,
                    Continuous = new() { [MenuButton.Left] = 50, [MenuButton.Right] = 50 },
                }
            )
        );

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Button,
                values: [.. Enumerable.Range(1, 100).Select(i => new MenuValue(i.ToString()))],
                options: new MenuItemOptions()
                {
                    Pinwheel = false,
                    Continuous = new() { [MenuButton.Left] = 500, [MenuButton.Right] = 500 },
                }
            )
        );

        Menu.Display(player, menu);
    }
}
