using RMenu.Enums;

namespace RMenu;

public class MenuItem
{
    public MenuItemType Type { get; init; }
    public MenuValue? Head { get; set; }
    public MenuValue? Tail { get; set; }
    public List<MenuItem>? Values { get; set; }
    public MenuItemOptions Options { get; set; }

    public MenuItem(MenuItemType type, MenuValue head, List<MenuItem> values, MenuValue tail, MenuItemOptions? options = null)
    {
        Type = type;
        Head = head;
        Values = values;
        Tail = tail;
        Options = options ?? new MenuItemOptions();
    }

    public MenuItem(MenuItemType type, MenuValue head, List<MenuItem> values, MenuItemOptions? options = null)
    {
        Type = type;
        Head = head;
        Values = values;
        Options = options ?? new MenuItemOptions();
    }

    public MenuItem(MenuItemType type, List<MenuItem> values, MenuValue tail, MenuItemOptions? options = null)
    {
        Type = type;
        Values = values;
        Tail = tail;
        Options = options ?? new MenuItemOptions();
    }

    public MenuItem(MenuItemType type, List<MenuItem> values, MenuItemOptions? options = null)
    {
        Type = type;
        Values = values;
        Options = options ?? new MenuItemOptions();
    }

    public MenuItem(MenuItemType type, MenuValue head, MenuItemOptions? options = null)
    {
        Type = type;
        Head = head;
        Options = options ?? new MenuItemOptions();
    }

    public MenuItem(MenuItemType type, MenuItemOptions? options = null)
    {
        Type = type;
        Options = options ?? new MenuItemOptions();
    }
}