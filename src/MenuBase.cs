using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuBase
{
    public MenuValue? Title { get; set; }
    public MenuOptions Options { get; init; }
    public List<MenuItem> Items { get; } = [];
    public long[] InputDelay { get; } = new long[Enum.GetValues(typeof(MenuButton)).Length];
    public Action<CCSPlayerController, MenuBase, MenuAction, MenuItem?>? Callback { get; set; }

    public MenuBase(MenuValue title, MenuOptions? options = null)
    {
        Title = title;
        Options = options ?? new MenuOptions();
    }

    public MenuBase(string title, MenuOptions? options = null)
    {
        Title = new MenuValue(title);
        Options = options ?? new MenuOptions();
    }

    public MenuBase(MenuOptions? options = null)
    {
        Options = options ?? new MenuOptions();
    }

    public void Input(CCSPlayerController player, MenuButton button)
    {
        switch (button)
        {
            case MenuButton.Exit:
                Callback?.Invoke(player, this, MenuAction.Cancel, null);
                Menu.Clear(player);
                break;
        }
    }
}