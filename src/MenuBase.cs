using Menus.Enums;

namespace Menus;

public class MenuBase
{
    private readonly MenuOptions _options;
    public Action<MenuButtons, MenuBase?, MenuItem?>? Callback;

    public MenuBase(MenuValue title, MenuOptions? options = null)
    {
        _options = options ?? new MenuOptions();
    }
}