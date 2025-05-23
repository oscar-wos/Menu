using CounterStrikeSharp.API.Core;

namespace RMenu;

public class MenuEvent(CCSPlayerController player, MenuBase? menu)
{
    public CCSPlayerController Player { get; } = player;
    public MenuBase? Menu { get; } = menu;
}