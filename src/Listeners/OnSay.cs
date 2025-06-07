using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;

namespace RMenu.Listeners;

internal static class OnSayListener
{
    public static void Register()
    {
        NativeAPI.HookUsermessage(118, (InputArgument)FunctionReference.Create(OnSay), HookMode.Pre);
    }

    private static HookResult OnSay(UserMessage um)
    {
        var index = um.ReadInt("entityindex");
        var message = um.ReadString("param2");
        var player = Utilities.GetPlayerFromIndex(index);

        if (player is null || !player.IsValid)
            return HookResult.Continue;

        var menu = Menu.Get(player);

        if (menu is null || !menu.Text)
            return HookResult.Continue;

        var item = menu.SelectedItem?.Item;
        var value = item?.SelectedValue?.Value;

        if (value is null)
            return HookResult.Continue;

        value.Data = message;
        menu.Text = false;
        return HookResult.Continue;
    }
}