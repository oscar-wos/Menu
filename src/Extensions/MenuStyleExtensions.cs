using RMenu.Enums;

namespace RMenu.Extensions;

public static class MenuStyleExtensions
{
    public static string Value(this MenuStyle style) =>
        style switch
        {
            MenuStyle.None => "stratum",
            MenuStyle.Bold => "stratum-bold",
            MenuStyle.Italic => "stratum-bold-italic",
            MenuStyle.Mono => "stratum-bold-mono",
            _ => string.Empty,
        };
}
