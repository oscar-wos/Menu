using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;

namespace Example;

public partial class Example
{
    private void Example1Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuValue header = new("new menu");
        MenuValue footer = new("footer", new MenuFormat(color: Color.Green, style: MenuStyle.Bold));
        MenuBase menu = new(header: header, footer: footer);

        Menu.Display(player, menu, Example1MenuCallback);
    }

    private void Example1MenuCallback(MenuBase menu, MenuAction menuAction)
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
