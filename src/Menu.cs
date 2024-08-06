using CounterStrikeSharp.API.Core;
using Menus.Enums;
using Menus.Hooks;

namespace Menus;

public static class Menu
{
    private static readonly Dictionary<CCSPlayerController, Stack<MenuBase>> Menus = [];
    private static readonly OnSay OnSay = new("say", OnSayListener);
    private static readonly OnSay OnSayTeam = new("say_team", OnSayListener);
    private static readonly OnTick OnTick = new(OnTickListener);

    private static HookResult OnSayListener(CCSPlayerController? controller, string message)
    {
        return HookResult.Handled;
    }

    private static void OnTickListener()
    {
        foreach (var (controller, value) in Menus)
        {
            if (!controller.IsValid() || value.Count == 0)
            {
                Menus.Remove(controller);
                continue;
            }

            //test

        }
    }

    public static void Set(CCSPlayerController controller, MenuBase menu, Action<MenuAction, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static void Add(CCSPlayerController controller, MenuBase menu, Action<MenuAction, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static bool Close(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var value))
            return false;

        if (invoke)
            value.Peek().Callback?.Invoke(MenuAction.Exit, null, null);

        value.Clear();
        return true;
    }

    public static bool Pop(CCSPlayerController controller, MenuBase? menu = null, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var value) || value.Count == 0)
            return false;

        var peek = value.Peek();

        if (menu != null && peek != menu)
            return false;

        if (invoke)
            peek.Callback?.Invoke(value.Count == 1 ? MenuAction.Exit : MenuAction.Back, null, null);

        value.Pop();
        return true;
    }
}