using CounterStrikeSharp.API.Core;

namespace Menu;

public class OnTick
{
    public OnTick(Action callback)
    {
        var functionReference = FunctionReference.Create(callback);
        NativeAPI.AddListener("OnTick", functionReference);
    }
}