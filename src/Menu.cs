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
    }

    public static void Set(CCSPlayerController controller, MenuBase menu, Action<MenuButtons, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static void Add(CCSPlayerController controller, MenuBase menu, Action<MenuButtons, MenuBase?, MenuItem?> callback)
    {
        menu.Callback = callback;
    }

    public static bool Clear(CCSPlayerController controller, bool invoke = false)
    {
        if (!Menus.TryGetValue(controller, out var value))
            return false;

        if (invoke && value.Count > 0)
            value.Peek().Callback?.Invoke(MenuButtons.Exit, null, null);

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
            peek.Callback?.Invoke(value.Count == 1 ? MenuButtons.Exit : MenuButtons.Back, null, null);

        value.Pop();
        return true;
    }
}