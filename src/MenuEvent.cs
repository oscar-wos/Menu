using CounterStrikeSharp.API.Core;

namespace RMenu;

public class MenuEvent(CCSPlayerController player, MenuBase menu, string menuString)
{
    public CCSPlayerController Player { get; } = player;
    public MenuBase Menu { get; } = menu;
    public string String { get; set; } = menuString;

    public override string ToString()
    {
        return String;
    }
}