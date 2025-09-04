using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuData(CCSPlayerController player)
{
    public CCSPlayerController Player { get; } = player;

    public List<Stack<MenuBase>> Menus = [];
    public (MenuBase Menu, string Html)? Current = null;

    internal readonly long[] _lastInput = new long[Enum.GetValues(typeof(MenuButton)).Length];

    public void Update()
    {
        long currentTime = Environment.TickCount64;

        for (int i = 0; i < Enum.GetValues(typeof(MenuButton)).Length; i++)
        {
            _lastInput[i] = currentTime;
        }

        Current = null;
    }
}
