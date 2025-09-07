using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu.Models;

public class MenuData(CCSPlayerController player)
{
    internal readonly long[] _lastInput = new long[Enum.GetValues(typeof(MenuButton)).Length];

    public CCSPlayerController Player { get; } = player;
    public List<List<MenuBase>> Menus { get; } = [];
    public (MenuBase Menu, string Html)? Current { get; set; } = null;

    public void Update()
    {
        long currentTime = Environment.TickCount64;

        for (int i = 0; i < _lastInput.Length; i++)
        {
            _lastInput[i] = currentTime;
        }

        Current = null;
    }
}
