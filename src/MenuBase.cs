using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuBase(MenuOptions? options = null)
{
    public readonly MenuOptions Options = options ?? new MenuOptions();
    public List<MenuItem> Items { get; } = [];
    public readonly float[] InputDelay = new float[Enum.GetValues(typeof(MenuButton)).Length];
    public Action<CCSPlayerController, MenuBase, MenuAction, MenuItem?>? Callback;

    public void Input(MenuButton button)
    {

    }
}