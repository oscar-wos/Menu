using Menus.Interfaces;

namespace Menus;

public class MenuValue(string value, string? prefix = "", string? suffix = "") : IMenuFormat
{
    public string Value { get; set; } = value;
    public string? Prefix { get; set; } = prefix;
    public string? Suffix { get; set; } = suffix;

    public override string ToString()
    {
        return $"{Prefix}{Value}{Suffix}";
    }
}