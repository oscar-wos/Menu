using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;

namespace Example;

public partial class Example
{
    private void Example2Menu(CCSPlayerController? player, CommandInfo info)
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

        MenuValue footer = new(
            "footer",
            new MenuFormat(color: new Color().Strobe(Color.Red, Color.Orange))
        );

        MenuBase menu = new(header: header, footer: footer);

        Menu.Display(
            player,
            menu,
            (menu, menuAction) =>
            {
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
        );
    }
}
