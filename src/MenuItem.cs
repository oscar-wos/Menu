using RMenu.Enums;

namespace RMenu;

public class MenuItem
{
    private List<MenuValue>? _values;
    public (int Index, MenuValue Value)? SelectedValue { get; set; } = null;

    public List<MenuValue>? Values
    {
        get => _values;
        set
        {
            _values = value;

            if (_values is { Count: > 0 })
            {
                SelectedValue = (0, _values[0]);
            }
        }
    }

    public MenuItemType Type { get; set; }
    public MenuValue? Head { get; set; }
    public MenuValue? Tail { get; set; }
    public MenuItemOptions Options { get; init; }
    public object? Data { get; set; }
    public Action<MenuBase, MenuItem, MenuAction>? Callback { get; }

    public MenuItem(
        MenuItemType type,
        MenuValue? head = null,
        List<MenuValue>? values = null,
        MenuValue? tail = null,
        MenuItemOptions? options = null,
        object? data = null,
        Action<MenuBase, MenuItem, MenuAction>? callback = null
    )
    {
        Type = type;
        Head = head;
        Values = values;
        Tail = tail;
        Options = options ?? new MenuItemOptions();
        Data = data;
        Callback = callback;
    }

    public bool Input(MenuButton button)
    {
        if (_values is null || _values.Count == 0)
        {
            return false;
        }

        SelectedValue ??= (0, _values[0]);

        int newIndex = button switch
        {
            MenuButton.Left => Options.Pinwheel
                ? (SelectedValue.Value.Index - 1 + _values.Count) % _values.Count
                : Math.Max(0, SelectedValue.Value.Index - 1),
            MenuButton.Right => Options.Pinwheel
                ? (SelectedValue.Value.Index + 1) % _values.Count
                : Math.Min(_values.Count - 1, SelectedValue.Value.Index + 1),
            _ => SelectedValue.Value.Index,
        };

        if (newIndex == SelectedValue.Value.Index)
        {
            return false;
        }

        SelectedValue = (newIndex, _values[newIndex]);
        return true;
    }
}
