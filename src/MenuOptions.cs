using RMenu.Enums;
using System.Drawing;

namespace RMenu;

public class MenuOptions
{
    private MenuFontSize _headerFontSize = MenuFontSize.L;
    private MenuFontSize _footerFontSize = MenuFontSize.S;
    private MenuFontSize _itemFontSize = MenuFontSize.SM;
    private string _headerString = string.Empty;
    private string _footerString = string.Empty;
    private string _itemString = string.Empty;

    public MenuOptions()
    {
        UpdateHtml();
    }

    public MenuFontSize TitleFontSize
    {
        get => _headerFontSize;
        set
        {
            _headerFontSize = value;
            UpdateHtml();
        }
    }

    public MenuFontSize FooterFontSize
    {
        get => _footerFontSize;
        set
        {
            _footerFontSize = value;
            UpdateHtml();
        }
    }

    public MenuFontSize ItemFontSize
    {
        get => _itemFontSize;
        set
        {
            _itemFontSize = value;
            UpdateHtml();
        }
    }

    public MenuInput<MenuButton> Buttons { get; set; } = new();
    public bool BlockMovement { get; set; } = false;
    public bool ProcessInput { get; set; } = true;
    public int ButtonsDelay { get; set; } = 150;
    public bool DisplayItemsInHeader { get; set; } = true;
    //TODO
    public bool Exitable { get; set; } = true;
    public bool BlockJump { get; set; } = false;
    public int Priority { get; set; } = 0;
    public int Timeout { get; set; } = 0;

    public string HeaderSizeHtml() => _headerString;
    public string FooterSizeHtml() => _footerString;
    public string ItemSizeHtml() => _itemString;

    private void UpdateHtml()
    {
        _headerString = $"<font class=\"fontSize-{(_headerFontSize).ToString().ToLower()}\">";
        _footerString = $"<font class=\"fontSize-{(_footerFontSize).ToString().ToLower()}\">";
        _itemString = $"<font class=\"fontSize-{(_itemFontSize).ToString().ToLower()}\">";
    } 

    public MenuValue[] Cursor =
    [
        new("►", Color.FromArgb(0, 0, 0, 0)),
        new("◄", Color.FromArgb(0, 0, 0, 0))
    ];

    public MenuValue[] Selector =
    [
        new("[ ", Color.FromArgb(0, 0, 0, 0)),
        new(" ]", Color.FromArgb(0, 0, 0, 0))
    ];

    public MenuValue[] Bool =
    [
        new("✘", Color.Red),
        new("✔", Color.Green)
    ];

    public MenuValue[] Slider =
    [
        new("("),
        new(")"),
        new("-"),
        new("|")
    ];

    public MenuValue Input = new("________", Color.White);
}