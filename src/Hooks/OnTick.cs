using CounterStrikeSharp.API.Core;

namespace Menus.Hooks;

public class OnTick
{
    public OnTick(Action callback)
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(callback));
    }
}