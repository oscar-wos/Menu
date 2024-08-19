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

    public static bool Pop(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        var stack = menus.Peek();

        if (stack.Count == 0)
            return false;

        if (invoke)
            stack.Peek().Callback?.Invoke(stack.Count == 1 ? MenuAction.Exit : MenuAction.Back, null, null);

        stack.Pop();
        return true;
    }

    public static bool Close(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        var stack = menus.Peek();

        if (stack.Count == 0)
            return false;

        if (invoke)
            stack.Peek().Callback?.Invoke(MenuAction.Exit, null, null);

        stack.Clear();
        return true;
    }

    public static bool Clear(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return false;

        if (invoke)
            foreach (var menu in menus.Where(s => s.Count > 0))
                menu.Peek().Callback?.Invoke(MenuAction.Exit, null, null);

        menus.Clear();
        return true;
    }

    public static MenuBase? Get(CCSPlayerController controller)
    {
        if (!Menus.TryGetValue(controller, out var menus) || menus.Count == 0)
            return null;

        var stack = menus.Peek();
        return stack.Count == 0 ? null : stack.Peek();
    }
}