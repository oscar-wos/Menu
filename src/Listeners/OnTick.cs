using CounterStrikeSharp.API.Core;

namespace RMenu.Listeners;

internal static class OnTickListener
{
    public static void Register()
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));
    }

    private static void OnTick()
    {
        foreach (var (player, (menu, menuString)) in Menu._currentMenu)
        {
            if (player.Connected != PlayerConnectedState.PlayerConnected)
            {
                Menu._menus.Remove(player, out _);
                Menu._currentMenu.Remove(player, out _);
                continue;
            }

            Menu.RaiseOnPrintMenuPre(player, menu, menuString);
            player.PrintToCenterHtml(menuString);
        }
    }
}