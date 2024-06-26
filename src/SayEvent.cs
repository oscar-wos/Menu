using CounterStrikeSharp.API.Core;

namespace Menu;

public class SayEvent
{
    public SayEvent(string sayCommand, Func<CCSPlayerController, string, HookResult> callback)
    {
        var wrappedHandler = new Func<int, IntPtr, HookResult>((i, ptr) =>
        {
            var caller = (i != -1) ? new CCSPlayerController(NativeAPI.GetEntityFromIndex(i + 1)) : null;
            return callback.Invoke(caller!, NativeAPI.CommandGetArgString(ptr).Trim('"'));
        });

        var functionReference = FunctionReference.Create(wrappedHandler);
        NativeAPI.AddCommandListener(sayCommand, functionReference, false);
    }
}