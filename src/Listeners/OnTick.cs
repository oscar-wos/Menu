using CounterStrikeSharp.API.Core;

namespace RMenu.Listeners;

internal static class OnTickListener
{
    public static void Register() =>
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));

    private static void OnTick()
    {
        foreach ((CCSPlayerController player, (MenuBase menu, string html)) in Menu._currentMenu)
        {
            if (!player.IsValid || player.Connected != PlayerConnectedState.PlayerConnected)
            {
                Menu.Remove(player);
                continue;
            }

            string result = Menu.RaiseOnPrintMenu(player, menu, html);
            player.PrintToCenterHtml(result);
        }
    }
}
