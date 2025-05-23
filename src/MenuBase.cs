using RMenu.Enums;

namespace RMenu;

public class MenuBase(MenuOptions? options = null)
{
    public readonly MenuOptions Options = options ?? new MenuOptions();
    public Action<MenuBase, MenuAction, MenuItem?>? Callback;
    public List<MenuItem> Items { get; } = [];

    public readonly float[] InputDelay = new float[Enum.GetValues(typeof(MenuButton)).Length];

    public void Input(MenuButton button)
    {

    }
}