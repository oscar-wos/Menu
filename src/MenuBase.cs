using Menu.Enums;

namespace Menu;

public class MenuBase(MenuValue title)
{
    public Action<MenuButtons, MenuBase, MenuItem>? Callback;
    public MenuValue Title { get; set; } = title;
    public List<MenuItem> Items { get; set; } = [];
    public int Option { get; set; } = 0;

    public bool AcceptButtons { get; set; } = false;
    public bool AcceptInput { get; set; } = false;

    public MenuValue[] Cursor =
    [
        new MenuValue("►") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue("◄") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }
    ];

    public MenuValue[] Selector =
    [
        new MenuValue("[ ") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }
    ];

    public MenuValue[] Bool =
    [
        new MenuValue("✘") { Prefix = "<font color=\"#FF0000\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue("✔") { Prefix = "<font color=\"#008000\">", Suffix = "<font color=\"#FFFFFF\">" }
    ];

    public MenuValue[] Slider =
    [
        new MenuValue("(") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(")") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue("-") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue("|") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }
    ];

    public MenuValue[] Input =
    [
        new MenuValue("________") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }
    ];

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
}