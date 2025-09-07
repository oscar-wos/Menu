using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Models;

namespace RMenu;

public class MenuBase(
    MenuValue? header = null,
    MenuValue? footer = null,
    MenuOptions? options = null
)
{
    public CCSPlayerController Player { get; set; } = null!;
    public List<MenuItem> Items { get; set; } = [];
    public MenuSelectedItem? SelectedItem { get; set; } = null;
    public MenuValue? Header { get; set; } = header;
    public MenuValue? Footer { get; set; } = footer;
    public MenuOptions Options { get; set; } = options ?? new MenuOptions();

    internal Action<MenuBase, MenuAction>? Callback { get; set; }

    internal void Input(MenuButton button)
    {
        Action? action = button switch
        {
            MenuButton.Up => HandleUp,
            MenuButton.Down => HandleDown,
            MenuButton.Left => HandleLeft,
            MenuButton.Right => HandleRight,
            MenuButton.Select => HandleSelect,
            MenuButton.Back => HandleBack,
            MenuButton.Exit => HandleExit,
            MenuButton.Assist => HandleAssist,
            _ => null,
        };

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
                Invoke(MenuAction.Choose);
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
                Invoke(MenuAction.Choose);
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

    private void HandleSelect()
    {
        if (SelectedItem?.Item is null)
        {
            return;
        }

        Invoke(MenuAction.Select);
    }

    private void HandleBack()
    {
        if (Menu.Close(Player))
        {
            Invoke(MenuAction.Exit);
        }
    }

    private void HandleExit()
    {
        if (!Options.Exitable)
        {
            return;
        }

        Menu.Clear(Player);
        Invoke(MenuAction.Exit);
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
        Menu.IsSelectable(Items[index])
        && (SelectedItem = new MenuSelectedItem(index, Items[index])) != null;
}
