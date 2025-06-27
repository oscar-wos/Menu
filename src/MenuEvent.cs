using CounterStrikeSharp.API.Core;

namespace RMenu;

public class MenuEvent(CCSPlayerController player, MenuBase menu, string html)
{
    public CCSPlayerController Player { get; } = player;
    public MenuBase Menu { get; } = menu;
    public string String { get; set; } = html;

    public override string ToString() => String;
}
