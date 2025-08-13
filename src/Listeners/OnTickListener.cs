using CounterStrikeSharp.API.Core;

namespace RMenu.Listeners;

internal static class OnTickListener
{
    public static void Register() =>
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));

    private static void OnTick()
    {
        foreach ((int playerSlot, (MenuBase menu, string html)) in Menu.CurrentMenus)
        {
            if (
                !Menu.Players.TryGetValue(playerSlot, out CCSPlayerController? player)
                || !player.IsValid
                || player.Connected != PlayerConnectedState.PlayerConnected
            )
            {
                Menu.Remove(playerSlot);
                continue;
            }

            string result = Menu.RaiseOnPrintMenu(menu, html);
            player.PrintToCenterHtml(result, 1);
        }
    }
}
