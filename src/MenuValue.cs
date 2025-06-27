using System.Drawing;
using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Helpers;

namespace RMenu;

public class MenuValue(
    string value,
    Color? color = null,
    object? data = null,
    Action<CCSPlayerController, MenuAction, MenuValue>? callback = null
)
{
    internal string Display { get; set; } = value;
    public string Value { get; set; } = value;
    public Color Color { get; set; } = color ?? Color.White;
    public object? Data { get; set; } = data;
    public Action<CCSPlayerController, MenuAction, MenuValue>? Callback { get; } = callback;

    public static implicit operator MenuValue(string value) => new(value);

    public override string ToString()
    {
        Color usedColor = Color;

        switch (usedColor.A)
        {
            case 0:
                usedColor = Rainbow.CurrentColor;
                break;

            case 1:
                return Rainbow.ApplyStrobeEffect(Display, usedColor.R, false);

            case 2:
                return Rainbow.ApplyStrobeEffect(Display, usedColor.R, true);

            default:
                break;
        }

        return $"<font color=\"#{usedColor.R:X2}{usedColor.G:X2}{usedColor.B:X2}\">{Display}</font>";
    }
}
