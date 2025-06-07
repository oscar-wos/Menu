using CounterStrikeSharp.API.Core;
using RMenu.Enums;

namespace RMenu;

public class MenuItem(MenuItemType type, MenuValue? head = null, List<MenuValue>? values = null, MenuValue? tail = null, MenuItemOptions? options = null, Action<CCSPlayerController, MenuAction, MenuItem>? callback = null)
{
    public MenuItemOptions Options { get; init; } = options ?? new MenuItemOptions();
    public MenuItemType Type { get; set; } = type;
    public MenuValue? Head { get; set; } = head;
    public MenuValue? Tail { get; set; } = tail;
    public object? Data { get; set; } = null;
    public List<MenuValue>? Values { get; set; } = values;
    public Action<CCSPlayerController, MenuAction, MenuItem>? Callback { get; } = callback;
    public (int Index, MenuValue Value)? SelectedValue { get; set; } = null;

    public bool Input(MenuButton button)
    {
        if (Values is null || Values.Count == 0)
            return false;

        SelectedValue ??= (0, Values[0]);
        int newIndex = SelectedValue.Value.Index;

        if (Type is (MenuItemType.Button or MenuItemType.Choice or MenuItemType.ChoiceBool))
        {
            switch (button)
            {
                case MenuButton.Left:
                    if (Options.Pinwheel)
                        newIndex = (newIndex - 1 + Values.Count) % Values.Count;
                    else if (newIndex > 0)
                        newIndex--;

                    break;

                case MenuButton.Right:
                    if (Options.Pinwheel)
                        newIndex = (newIndex + 1) % Values.Count;
                    else if (newIndex < Values.Count - 1)
                        newIndex++;

                    break;
            }
        }

        if (newIndex == SelectedValue.Value.Index)
            return false;

        SelectedValue = (newIndex, Values[newIndex]);
        return true;
    }
}