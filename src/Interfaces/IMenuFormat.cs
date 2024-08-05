namespace Menus.Interfaces;

public interface IMenuFormat
{
    string Value { get; set; }
    string? Prefix { get; set; }
    string? Suffix { get; set; }
}