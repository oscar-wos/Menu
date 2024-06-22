using Menu.Enums;

namespace Menu;

public class MenuItem
{
    public MenuItemType Type { get; init; }
    public bool Pinwheel { get; init; }
    public MenuValue? Head { get; set; }
    public MenuValue? Tail { get; set; }
    public List<MenuValue>? Values { get; set; }
    public int[] Data { get; set; }
    public string DataString { get; set; } = "";
    public int Option { get; set; } = 0;

    public MenuItem(MenuItemType type, MenuValue head, List<MenuValue> values, MenuValue tail, bool pinwheel = false)
    {
        Type = type;
        Pinwheel = pinwheel;
        Head = head;
        Tail = tail;
        Values = values;
        Data = new int[values.ToList().Count];
    }

    public MenuItem(MenuItemType type, MenuValue head, List<MenuValue> values, bool pinwheel = false)
    {
        Type = type;
        Pinwheel = pinwheel;
        Head = head;
        Values = values;
        Data = new int[values.ToList().Count];
    }

    public MenuItem(MenuItemType type, List<MenuValue> values, MenuValue tail, bool pinwheel = false)
    {
        Type = type;
        Pinwheel = pinwheel;
        Tail = tail;
        Values = values;
        Data = new int[values.ToList().Count];
    }

    public MenuItem(MenuItemType type, List<MenuValue> values, bool pinwheel = false)
    {
        Type = type;
        Pinwheel = pinwheel;
        Values = values;
        Data = new int[values.ToList().Count];
    }

    public MenuItem(MenuItemType type, MenuValue head, MenuValue tail)
    {
        Type = type;
        Head = head;
        Tail = tail;
        Data = new int[1];
    }

    public MenuItem(MenuItemType type, MenuValue head)
    {
        Type = type;
        Head = head;
        Data = new int[1];
    }

    public MenuItem(MenuItemType type)
    {
        Type = type;
        Data = new int[1];
    }
}