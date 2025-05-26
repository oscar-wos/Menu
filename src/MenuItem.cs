using RMenu.Enums;

namespace RMenu;

public class MenuItem(MenuItemType type, MenuValue? head = null, List<MenuValue>? values = null, MenuValue? tail = null, MenuItemOptions? options = null)
{
    public MenuItemOptions Options { get; set; } = options ?? new MenuItemOptions();
    public MenuItemType Type { get; set; } = type;
    public MenuValue? Head { get; set; } = head;
    public MenuValue? Tail { get; set; } = tail;
    public object? Data { get; set; } = null;
    public List<MenuValue>? Values { get; set; } = values;
    public (int Index, MenuValue MenuValue)? SelectedValue { get; set; } = null;
}