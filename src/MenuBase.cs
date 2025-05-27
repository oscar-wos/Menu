using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuBase
{
    public long[] InputDelay { get; } = new long[Enum.GetValues(typeof(MenuButton)).Length];
    public MenuOptions Options { get; init; }
    public List<MenuValue>? Header { get; set; }
    public List<MenuValue>? Footer { get; set; }
    public Action<CCSPlayerController, MenuBase, MenuAction, MenuItem?>? Callback { get; set; }
    public List<MenuItem> Items { get; set; } = [];
    public (int Index, MenuItem Item)? SelectedItem { get; set; } = null;
    public bool Text { get; set; } = false;

    public MenuBase(MenuValue? header = null, MenuValue? footer = null, MenuOptions? options = null)
    {
        Header = header is null ? null : [header];
        Footer = footer is null ? null : [footer];
        Options = options ?? new MenuOptions();
    }

    public MenuBase(IEnumerable<MenuValue>? header = null, IEnumerable<MenuValue>? footer = null, MenuOptions? options = null)
    {
        Header = header?.ToList();
        Footer = footer?.ToList();
        Options = options ?? new MenuOptions();
    }

    public MenuBase()
    {
        Options = new MenuOptions();
    }

    public static bool IsSelectable(MenuItem item)
    {
        return item.Type == MenuItemType.Choice ||
               item.Type == MenuItemType.Button ||
               item.Type == MenuItemType.Bool ||
               item.Type == MenuItemType.ChoiceBool ||
               item.Type == MenuItemType.Slider ||
               item.Type == MenuItemType.Input;
    }

    private bool HasSelectedItem() => SelectedItem.HasValue && SelectedItem.Value.Index >= 0 && SelectedItem.Value.Index < Items.Count;
    private bool SelectItem(int index) => IsSelectable(Items[index]) && (SelectedItem = (index, Items[index])) != null;

    public void Input(CCSPlayerController player, MenuButton button)
    {
        switch (button)
        {
            case MenuButton.Up when !Text:
                if (!HasSelectedItem())
                    return;

                for (int newIndex = SelectedItem!.Value.Index - 1; newIndex >= 0; newIndex--)
                {
                    if (SelectItem(newIndex))
                    {
                        Callback?.Invoke(player, this, MenuAction.Update, SelectedItem.Value.Item);
                        break;
                    }
                }

                break;

            case MenuButton.Down when !Text:
                if (!HasSelectedItem())
                    return;

                for (int newIndex = SelectedItem!.Value.Index + 1; newIndex < Items.Count; newIndex++)
                {
                    if (SelectItem(newIndex))
                    {
                        Callback?.Invoke(player, this, MenuAction.Update, SelectedItem.Value.Item);
                        break;
                    }
                }

                break;

            case MenuButton.Left or MenuButton.Right when !Text:
                if (!HasSelectedItem())
                    return;

                if (SelectedItem!.Value.Item.Input(button))
                    Callback?.Invoke(player, this, MenuAction.Update, SelectedItem.Value.Item);

                break;

            case MenuButton.Select when !Text:
                if (!HasSelectedItem())
                    return;

                Callback?.Invoke(player, this, MenuAction.Select, SelectedItem!.Value.Item);
                break;

            case MenuButton.Exit:
                Callback?.Invoke(player, this, MenuAction.Cancel, null);
                Menu.Clear(player);
                break;
        }



        /*
         * Up,
    Down,
    Left,
    Right,
    Select,
    Back,
    Exit,
    Assist
        switch (button)
        {
            case MenuButton.Exit:
                Callback?.Invoke(player, this, MenuAction.Cancel, null);
                Menu.Clear(player);
                break;
        }
        */
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
}