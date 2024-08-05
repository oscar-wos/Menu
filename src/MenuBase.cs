using Menus.Enums;

namespace Menus;

public class MenuBase(MenuOptions? options = null)
{
    private readonly MenuOptions _options = options ?? new MenuOptions();
    public Action<MenuAction, MenuBase?, MenuItem?>? Callback;
}