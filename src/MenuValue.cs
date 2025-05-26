using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Models;
using System.Drawing;

namespace RMenu;

public class MenuValue(string value, Color? color = null, object? data = null, Action<CCSPlayerController, MenuAction, MenuValue>? callback = null)
{
    public string Value { get; set; } = value;
    public Color Color { get; set; } = color ?? Color.White;
    public object? Data { get; set; } = data;
    public Action<CCSPlayerController, MenuAction, MenuValue>? Callback { get; } = callback;

    public static implicit operator MenuValue(string value) => new(value);

    public override string ToString()
    {
        var usedColor = Color;

        if (usedColor.R == 0 && usedColor.G == 0 && usedColor.B == 0)
        {
            if (usedColor.A != 0)
                return Rainbow.Strobe(Value, usedColor.A);

            usedColor = Rainbow.Color;
        }

        return $"<font color=\"#{usedColor.R:X2}{usedColor.G:X2}{usedColor.B:X2}\">{Value}";
    }
}