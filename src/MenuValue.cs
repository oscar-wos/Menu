using Menu.Enums;

namespace Menu;

public class MenuValue(string value) : IMenuFormat
{
    public string Value { get; set; } = "";
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";
}