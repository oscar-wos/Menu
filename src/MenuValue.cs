using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Helpers;
using System.Drawing;

namespace RMenu;

public class MenuValue(string value, Color? color = null, object? data = null, Action<CCSPlayerController, MenuAction, MenuValue>? callback = null)
{
    internal string Display { get; set; } = value;
    public string Value { get; set; } = value;
    public Color Color { get; set; } = color ?? Color.White;
    public object? Data { get; set; } = data;
    public Action<CCSPlayerController, MenuAction, MenuValue>? Callback { get; } = callback;

    public static implicit operator MenuValue(string value) => new(value);

    public override string ToString()
    {
        var usedColor = Color;
        int rgb = (usedColor.R << 16) | (usedColor.G << 8) | usedColor.B;

        if (rgb == 0x010000)
            return "";

        if (rgb == 0x000100)
            return "";

        if (rgb == 0x000001)
            return Rainbow.ApplyStrobeEffect(Display, usedColor.A, true);

        if (rgb == 0x000000)
        {
            if (usedColor.A != 0)
                return Rainbow.ApplyStrobeEffect(Display, usedColor.A, false);

            usedColor = Rainbow.CurrentColor;
        }

        return $"<font color=\"#{usedColor.R:X2}{usedColor.G:X2}{usedColor.B:X2}\">{Display}</font>";
    }
}