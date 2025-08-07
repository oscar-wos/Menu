using RMenu.Enums;

namespace RMenu;

public class MenuItem(
    MenuItemType type,
    MenuValue? head = null,
    List<MenuValue>? values = null,
    MenuValue? tail = null,
    MenuItemOptions? options = null,
    object? data = null,
    Action<MenuBase, MenuItem, MenuAction>? callback = null
)
{
    public (int Index, MenuValue Value)? SelectedValue { get; set; } = null;

    public MenuItemType Type { get; set; } = type;
    public MenuValue? Head { get; set; } = head;
    public List<MenuValue>? Values { get; set; } = values;
    public MenuValue? Tail { get; set; } = tail;
    public MenuItemOptions Options { get; init; } = options ?? new MenuItemOptions();
    public object? Data { get; set; } = data;
    public Action<MenuBase, MenuItem, MenuAction>? Callback { get; } = callback;

    public bool Input(MenuButton button)
    {
        if (Values is null || Values.Count == 0)
        {
            return false;
        }

        SelectedValue ??= (0, Values[0]);

        int newIndex = button switch
        {
            MenuButton.Left => Options.Pinwheel
                ? (SelectedValue.Value.Index - 1 + Values.Count) % Values.Count
                : Math.Max(0, SelectedValue.Value.Index - 1),
            MenuButton.Right => Options.Pinwheel
                ? (SelectedValue.Value.Index + 1) % Values.Count
                : Math.Min(Values.Count - 1, SelectedValue.Value.Index + 1),
            _ => SelectedValue.Value.Index,
        };

        if (newIndex == SelectedValue.Value.Index)
        {
            return false;
        }

        SelectedValue = (newIndex, Values[newIndex]);
        return true;
    }
}
