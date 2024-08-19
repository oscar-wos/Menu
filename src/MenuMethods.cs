using CounterStrikeSharp.API.Core;
using Menus.Enums;

namespace Menus;

public static partial class Menu
{
    public static void Set(CCSPlayerController controller, MenuBase menu, bool save, Action<MenuAction, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static void Add(CCSPlayerController controller, MenuBase menu, Action<MenuAction, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static bool Pop(CCSPlayerController controller, bool invoke = false, MenuBase? menu = null)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        var currentStack = menus.Peek();

        if (currentStack.Count == 0)
            return false;

        var currentMenu = currentStack.Peek();

        if (menu != null && currentMenu != menu)
            return false;

        if (invoke)
            currentMenu.Callback?.Invoke(currentStack.Count == 1 ? MenuAction.Exit : MenuAction.Back, null, null);

        currentStack.Pop();
        return true;
    }

    public static bool Close(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        var currentStack = menus.Peek();

        if (currentStack.Count == 0)
            return false;

        var currentMenu = currentStack.Peek();

        if (invoke)
            currentMenu.Callback?.Invoke(MenuAction.Exit, null, null);

        currentStack.Clear();
        return true;
    }

    public static bool Clear(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        if (invoke)
            foreach (var menu in menus.Peek())
                menu.Callback?.Invoke(MenuAction.Exit, null, null);

        menus.Clear();
        return true;
    }
}