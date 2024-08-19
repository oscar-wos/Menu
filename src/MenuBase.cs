using Menus.Enums;

namespace Menus;

public class MenuBase(MenuOptions? options = null)
{
    public readonly MenuOptions Options = options ?? new MenuOptions();
    public readonly Dictionary<MenuButton, float[]> InputDelay = [];
    public Action<MenuAction, MenuBase?, MenuItem?>? Callback;
    
    public List<MenuItem> Items { get; set; } = [];
    public int Option { get; set; } = 0;
    private MenuButton LastAction { get; set; } = MenuButton.Up;

    public bool AcceptInput { get; set; } = false;

    public void Input(MenuButton button)
    {
        if (AcceptInput && button != MenuButton.Back)
            return;

        switch (button)
        {

        }
    }
}