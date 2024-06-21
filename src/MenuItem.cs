using Menu.Enums;

namespace Menu;

public class MenuItem
{
    public MenuItemType Type { get; init; }
    public MenuValue? Head { get; set; }
    public MenuValue? Tail { get; set; }
    public List<MenuValue>? Values { get; set; }
    public int[]? Data { get; set; }
    public string DataString { get; set; } = "";
    public bool Pinwheel { get; init; }
    public int Option { get; set; } = 0;

    public MenuItem(MenuItemType type, MenuValue head, MenuValue tail, List<MenuValue> values, bool pinwheel = false)
    {
        Type = type;
        Head = head;
        Tail = tail;
        Values = values;
        Pinwheel = pinwheel;
        Data = new int[values.ToList().Count - 1];
    }

    public MenuItem(MenuItemType type, MenuValue head, bool pinwheel = false)
    {
        Type = type;
        Head = head;
        Pinwheel = pinwheel;
    }

    public MenuItem(MenuItemType type, List<MenuValue> values, bool pinwheel = false)
    {
        Type = type;
        Values = values;
        Pinwheel = pinwheel;
        Data = new int[values.ToList().Count - 1];
    }

    public MenuItem(MenuItemType type, MenuValue head, List<MenuValue> values, bool pinwheel = false)
    {
        Type = type;
        Head = head;
        Values = values;
        Pinwheel = pinwheel;
        Data = new int[values.ToList().Count - 1];
    }
}