using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;

namespace Example;

public partial class Example
{
    private void Example3Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        List<MenuObject> header =
        [
            new("new ", new MenuFormat(color: Color.Green)),
            new("menu", new MenuFormat(color: Color.Blue, style: MenuStyle.Bold)),
        ];

        List<MenuObject> footer =
        [
            new(
                "footer",
                new MenuFormat(color: new Color().StrobeReversed(Color.Red, Color.Orange))
            ),
            new(" extension", new MenuFormat(style: MenuStyle.Italic)),
        ];

        MenuOptions options = new() { BlockMovement = true, DisplayItemsInHeader = true };
        options.Buttons[MenuButton.Select] = PlayerButtons.Use;

        MenuBase menu = new(header: header, footer: footer, options: options);

        Menu.Display(player, menu, Example3MenuCallback);
    }

    private void Example3MenuCallback(MenuBase menu, MenuAction menuAction)
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
