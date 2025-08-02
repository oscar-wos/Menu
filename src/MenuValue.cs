using System.Drawing;
using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Helpers;

namespace RMenu;

public class MenuValue(
    string value,
    Color? color = null,
    object? data = null,
    Action<CCSPlayerController, MenuBase, MenuValue, MenuAction>? callback = null
)
{
    internal string Display { get; set; } = value;
    public string Value { get; set; } = value;
    public Color Color { get; set; } = color ?? Color.White;
    public object? Data { get; set; } = data;
    public Action<CCSPlayerController, MenuBase, MenuValue, MenuAction>? Callback { get; } =
        callback;

    public static implicit operator MenuValue(string value) => new(value);

    public override string ToString() => FormatString();

    public string ToStringHighlighted(Color? highlight = null) => FormatString(highlight);

    private string FormatString(Color? highlight = null)
    {
        Color usedColor = highlight ?? Color;

        switch (usedColor.A)
        {
            case 0:
                usedColor = Rainbow.CurrentColor;
                break;

            case 1:
                return Rainbow.ApplyStrobeEffect(
                    Display,
                    usedColor.R,
                    usedColor.G,
                    usedColor.B,
                    false
                );

            case 2:
                return Rainbow.ApplyStrobeEffect(
                    Display,
                    usedColor.R,
                    usedColor.G,
                    usedColor.B,
                    true
                );

            default:
                break;
        }

        return $"<font color=\"#{usedColor.R:X2}{usedColor.G:X2}{usedColor.B:X2}\">{Display}</font>";
    }
}
