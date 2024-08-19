using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Menus.Enums;
using Menus.Hooks;

namespace Menus;

public static partial class Menu
{
    private static readonly Dictionary<CCSPlayerController, Stack<Stack<MenuBase>>> Menus = [];
    private static readonly OnSay OnSay = new("say", OnSayListener);
    private static readonly OnSay OnSayTeam = new("say_team", OnSayListener);
    private static readonly OnTick OnTick = new(OnTickListener);

    private static HookResult OnSayListener(CCSPlayerController? controller, string message)
    {
        if (controller == null || !controller.IsValid())
            return HookResult.Continue;

        var menu = Get(controller);

        if (menu is not { AcceptInput: true })
            return HookResult.Continue;

        menu.Callback?.Invoke(MenuAction.Input, null, null);
        return HookResult.Handled;
    }

    private static void OnTickListener()
    {
        foreach (var (controller, menus) in Menus)
        {
            if (!controller.IsValid() || menus.Count == 0)
            {
                Menus.Remove(controller);
                continue;
            }

            var stack = menus.Peek();

            if (stack.Count == 0)
            {
                menus.Pop();
                continue;
            }

            var menu = stack.Peek();

            InputButtons(controller, menu);
            DrawMenu(controller, menu);
        }
    }

    private static void InputButtons(CCSPlayerController controller, MenuBase menu)
    {
        foreach (MenuButton button in Enum.GetValues(typeof(MenuButton)))
        {
            if (((ulong)controller.Buttons & menu.Options.Buttons[button]) == menu.Options.Buttons[button])
            {
                if (!menu.InputDelay[button].Contains(0))
                    menu.InputDelay[button][0] = Server.CurrentTime;
                else
                {
                    if (Server.CurrentTime - menu.InputDelay[button][0] < menu.Options.ButtonsFirstDelay)
                        continue;

                    if (!menu.InputDelay[button].Contains(1) || Server.CurrentTime - menu.InputDelay[button][1] >= menu.Options.ButtonsContinuousDelay)
                        menu.InputDelay[button][1] = Server.CurrentTime;
                    else
                        continue;
                }

                menu.Input(button);
            }
            else
                menu.InputDelay[button] = [];
        }
    }

    private static void DrawMenu(CCSPlayerController controller, MenuBase menu)
    {

    }
}