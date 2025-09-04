using RMenu.Enums;

namespace RMenu;

public class MenuItem
{
    private List<MenuValue>? _values;
    public MenuSelectedValue? SelectedValue { get; set; } = null;

    public List<MenuValue>? Values
    {
        get => _values;
        set
        {
            _values = value;

            if (_values is { Count: > 0 })
            {
                SelectedValue = new(0, _values[0]);
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

        SelectedValue ??= new(0, _values[0]);

        int newIndex = button switch
        {
            MenuButton.Left => Options.Pinwheel
                ? (SelectedValue.Index - 1 + _values.Count) % _values.Count
                : Math.Max(0, SelectedValue.Index - 1),
            MenuButton.Right => Options.Pinwheel
                ? (SelectedValue.Index + 1) % _values.Count
                : Math.Min(_values.Count - 1, SelectedValue.Index + 1),
            _ => SelectedValue.Index,
        };

        if (newIndex == SelectedValue.Index)
        {
            return false;
        }

        SelectedValue = new(newIndex, _values[newIndex]);
        return true;
    }
}
