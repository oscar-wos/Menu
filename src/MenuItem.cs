using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuItem(
    MenuItemType type,
    MenuValue? head = null,
    List<MenuValue>? values = null,
    MenuValue? tail = null,
    MenuItemOptions? options = null,
    object? data = null,
    Action<CCSPlayerController, MenuBase, MenuItem, MenuAction>? callback = null
)
{
    public MenuItemType Type { get; set; } = type;
    public MenuValue? Head { get; set; } = head;
    public MenuValue? Tail { get; set; } = tail;
    public MenuItemOptions Options { get; init; } = options ?? new MenuItemOptions();
    public object? Data { get; set; } = data;
    public Action<CCSPlayerController, MenuBase, MenuItem, MenuAction>? Callback { get; } =
        callback;
    public List<MenuValue>? Values { get; set; } = values;
    public (int Index, MenuValue Value)? SelectedValue { get; set; } = null;

    public bool Input(MenuButton button)
    {
        if (Values is null || Values.Count == 0)
        {
            return false;
        }

        SelectedValue ??= (0, Values[0]);
        int newIdx = SelectedValue.Value.Index;

        if (Type is MenuItemType.Button or MenuItemType.Choice or MenuItemType.ChoiceBool)
        {
            switch (button)
            {
                case MenuButton.Left:
                    if (Options.Pinwheel)
                    {
                        newIdx = (newIdx - 1 + Values.Count) % Values.Count;
                    }
                    else if (newIdx > 0)
                    {
                        newIdx--;
                    }

                    break;

                case MenuButton.Right:
                    if (Options.Pinwheel)
                    {
                        newIdx = (newIdx + 1) % Values.Count;
                    }
                    else if (newIdx < Values.Count - 1)
                    {
                        newIdx++;
                    }

                    break;
            }
        }

        if (newIdx == SelectedValue.Value.Index)
        {
            return false;
        }

        SelectedValue = (newIdx, Values[newIdx]);
        return true;
    }
}
