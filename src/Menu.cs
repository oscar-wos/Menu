using CounterStrikeSharp.API.Core;

namespace Menu;

public class Menu
{
    public static Dictionary<CCSPlayerController, Stack<MenuBase>> Menus = [];

    public void AddMenu(CCSPlayerController controller, MenuBase menu)
    {
        if (!Menus.ContainsKey(controller))
            Menus.Add(controller, new Stack<MenuBase>());

        Menus[controller].Push(menu);
    }

    public Stack<MenuBase>? GetMenu(CCSPlayerController controller)
    {
        return Menus.TryGetValue(controller, out var menu) ? menu : null;
    }
}