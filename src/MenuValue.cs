using Menus.Enums;

namespace Menus;

public class MenuValue(string value, MenuFontSize size = MenuFontSize.M, string? prefix = "", string? suffix = "")
{
    public string Value { get; set; } = value;
    public MenuFontSize Size { get; set; } = size;
    public string? Prefix { get; set; } = prefix;
    public string? Suffix { get; set; } = suffix;

    public override string ToString()
    {
        return $"{Prefix}{Value}{Suffix}";
    }
}