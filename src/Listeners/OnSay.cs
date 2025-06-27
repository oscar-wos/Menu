using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;

namespace RMenu.Listeners;

internal static class OnSayListener
{
    public static void Register() =>
        NativeAPI.HookUsermessage(
            118,
            (InputArgument)FunctionReference.Create(OnSay),
            HookMode.Pre
        );

    private static HookResult OnSay(UserMessage um)
    {
        int index = um.ReadInt("entityindex");
        string message = um.ReadString("param2");

        if (Utilities.GetPlayerFromIndex(index) is not CCSPlayerController { IsValid: true } player)
        {
            return HookResult.Continue;
        }

        if (Menu.Get(player) is not MenuBase { Text: true } menu)
        {
            return HookResult.Continue;
        }

        if (menu.SelectedItem?.Item.SelectedValue?.Value is not MenuValue value)
        {
            return HookResult.Continue;
        }

        value.Data = message;
        menu.Text = false;
        return HookResult.Continue;
    }
}
