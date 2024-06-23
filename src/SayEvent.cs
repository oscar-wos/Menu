using CounterStrikeSharp.API.Core;

namespace Menu;

public class SayEvent
{
    public SayEvent(string sayCommand, Action<CCSPlayerController, string> callback)
    {
        var wrappedHandler = new Action<int, IntPtr>((i, ptr) =>
        {
            var caller = (i != -1) ? new CCSPlayerController(NativeAPI.GetEntityFromIndex(i + 1)) : null;
            callback.Invoke(caller!, NativeAPI.CommandGetArgString(ptr).Trim('"'));
        });

        var functionReference = FunctionReference.Create(wrappedHandler);
        NativeAPI.AddCommandListener(sayCommand, functionReference, false);
    }
}