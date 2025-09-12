using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private void Example9Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuBase menu = new();

        menu.Items.Add(
            new MenuItem(
                type: MenuItemType.Input,
                head: new MenuValue("Enter value: "),
                callback: (menu, menuItem, menuAction) =>
                {
                    if (menuAction == MenuAction.Input && menuItem.Data is string input)
                    {
                        player.PrintToChat($"Input - Data: {menuItem.Data}");
                    }
                }
            )
        );

        Menu.Display(player, menu);
    }
}
