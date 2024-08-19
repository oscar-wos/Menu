using CounterStrikeSharp.API.Core;

namespace Menus.Hooks;

public class OnSay
{
    public OnSay(string command, Func<CCSPlayerController, string, HookResult> callback)
    {
        var wrappedHandler = new Func<int, IntPtr, HookResult>((i, ptr) =>
        {
            var caller = (i != -1) ? new CCSPlayerController(NativeAPI.GetEntityFromIndex(i + 1)) : null;
            return callback.Invoke(caller!, NativeAPI.CommandGetArgString(ptr)[1..^1]);
        });

        NativeAPI.AddCommandListener(command, FunctionReference.Create(wrappedHandler), false);
    }
}