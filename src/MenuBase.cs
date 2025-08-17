using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuBase(
    MenuValue? header = null,
    MenuValue? footer = null,
    MenuOptions? options = null
)
{
    private readonly long[] _lastInput = new long[Enum.GetValues(typeof(MenuButton)).Length];
    internal Action<MenuBase, MenuAction>? Callback { get; set; }

    public CCSPlayerController Player { get; set; } = null!;
    public List<MenuItem> Items { get; set; } = [];
    public (int Index, MenuItem Item)? SelectedItem { get; set; } = null;

    public MenuValue? Header { get; set; } = header;
    public MenuValue? Footer { get; set; } = footer;
    public MenuOptions Options { get; init; } = options ?? new MenuOptions();

    internal void Input(MenuButton button)
    {
        if (_lastInput[(int)button] + Options.ButtonsDelay > Environment.TickCount64)
        {
            if (!Options.Continuous[button])
            {
                _lastInput[(int)button] = Environment.TickCount64;
            }

            return;
        }

        Action? action = button switch
        {
            MenuButton.Up => HandleUp,
            MenuButton.Down => HandleDown,
            MenuButton.Left => HandleLeft,
            MenuButton.Right => HandleRight,
            MenuButton.Select => HandleSelect,
            MenuButton.Exit => HandleExit,
            MenuButton.Assist => HandleAssist,
            _ => null,
        };

        _lastInput[(int)button] = Environment.TickCount64;
        action?.Invoke();
    }

    private void HandleUp()
    {
        if (SelectedItem?.Index is not { } index)
        {
            return;
        }

        for (int newIndex = index - 1; newIndex >= 0; newIndex--)
        {
            if (IsSelected(newIndex))
            {
                Invoke(MenuAction.Update);
                return;
            }
        }
    }

    private void HandleDown()
    {
        if (SelectedItem?.Index is not { } index)
        {
            return;
        }

        for (int newIndex = index + 1; newIndex < Items.Count; newIndex++)
        {
            if (IsSelected(newIndex))
            {
                Invoke(MenuAction.Update);
                return;
            }
        }
    }

    private void HandleLeft()
    {
        if (SelectedItem?.Item is not { } menuItem)
        {
            return;
        }

        if (menuItem.Input(MenuButton.Left))
        {
            Invoke(MenuAction.Update);
        }
    }

    private void HandleRight()
    {
        if (SelectedItem?.Item is not { } menuItem)
        {
            return;
        }

        if (menuItem.Input(MenuButton.Right))
        {
            Invoke(MenuAction.Update);
        }
    }

    private void HandleSelect() => Invoke(MenuAction.Select);

    private void HandleExit()
    {
        if (!Options.Exitable)
        {
            return;
        }

        Invoke(MenuAction.Exit);
        Menu.Clear(Player);
    }

    private void HandleAssist() => Invoke(MenuAction.Assist);

    private void Invoke(MenuAction menuAction)
    {
        MenuItem? menuItem = SelectedItem?.Item;
        MenuValue? menuValue = menuItem?.SelectedValue?.Value;

        menuValue?.Callback?.Invoke(this, menuValue, menuAction);
        menuItem?.Callback?.Invoke(this, menuItem, menuAction);
        Callback?.Invoke(this, menuAction);
    }

    private bool IsSelected(int index) =>
        IsSelectable(Items[index]) && (SelectedItem = (index, Items[index])) != null;

    internal static bool IsSelectable(MenuItem menuItem) =>
        menuItem.Type is MenuItemType.Choice or MenuItemType.Button;
}
