using Menu.Enums;

namespace Menu;

public class MenuValue(string value) : IMenuFormat
{
    public string Value { get; set; } = value;
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";

    public override string ToString()
    {
        return $"{Prefix}{Value}{Suffix}";
    }
}