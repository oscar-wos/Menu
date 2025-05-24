using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using System.Drawing;

namespace RMenu;

public class MenuValue
{
    public string Value { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.White;
    public object? Data { get; set; } = null;
    public Action<CCSPlayerController, MenuAction>? Callback { get; } = null;

    public MenuValue(string value, Color color, object data, Action<CCSPlayerController, MenuAction>? callback = null)
    {
        Value = value;
        Color = color;
        Data = data;
        Callback = callback;
    }

    public MenuValue(string value, Color color, Action<CCSPlayerController, MenuAction>? callback = null)
    {
        Value = value;
        Color = color;
        Callback = callback;
    }

    public MenuValue(string value, object data, Action<CCSPlayerController, MenuAction>? callback = null)
    {
        Value = value;
        Data = data;
        Callback = callback;
    }

    public MenuValue(string value, Action<CCSPlayerController, MenuAction>? callback = null)
    {
        Value = value;
        Callback = callback;
    }

    public override string ToString()
    {
        return $"<font color=\"#{Color.R:X2}{Color.G:X2}{Color.B:X2}\">{Value}<font color=\"#FFFFFF\">";
    }
}