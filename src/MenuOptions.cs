using RMenu.Enums;
using RMenu.Extensions;
using System.Drawing;

namespace RMenu;

public class MenuOptions
{
    internal int _availableItems = 1;
    internal int _availableChars = 1;
    private MenuFontSize _headerFontSize = MenuFontSize.L;
    private MenuFontSize _itemFontSize = MenuFontSize.SM;
    private MenuFontSize _footerFontSize = MenuFontSize.S;
    private string _headerString = string.Empty;
    private string _itemString = string.Empty;
    private string _footerString = string.Empty;

    public MenuOptions()
    {
        UpdateHtml();
    }

    public MenuFontSize HeaderFontSize
    {
        get => _headerFontSize;
        set
        {
            _headerFontSize = value;
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

    public MenuFontSize FooterFontSize
    {
        get => _footerFontSize;
        set
        {
            _footerFontSize = value;
            UpdateHtml();
        }
    }

    public MenuInput<MenuButton> Buttons { get; set; } = new();
    public bool BlockMovement { get; set; } = false;
    public bool ProcessInput { get; set; } = true;
    public int ButtonsDelay { get; set; } = 150;
    public bool DisplayItemsInHeader { get; set; } = true;
    public bool Exitable { get; set; } = true;
    public int Priority { get; set; } = 0;
    //TODO
    public bool BlockJump { get; set; } = false;
    public int Timeout { get; set; } = 0;

    public string HeaderSizeHtml() => _headerString;
    public string ItemSizeHtml() => _itemString;
    public string FooterSizeHtml() => _footerString;

    private void UpdateHtml()
    {
        _headerString = $"<font class=\"fontSize-{(_headerFontSize).ToString().ToLower()}\">";
        _itemString = $"<font class=\"fontSize-{(_itemFontSize).ToString().ToLower()}\">";
        _footerString = $"<font class=\"fontSize-{(_footerFontSize).ToString().ToLower()}\">";

        var availableHeight = Menu.MENU_HEIGHT - ((int)HeaderFontSize + (int)FooterFontSize);
        _availableItems = Math.Max(1, availableHeight / (int)ItemFontSize);
        _availableChars = (int)(Menu.MENU_LENGTH / ((int)ItemFontSize * 0.6)) - Selector[0].Value.Length - Selector[1].Value.Length;
    }

    public MenuValue[] Cursor =
    [
        new("►", new Color().Rainbow()),
        new("◄", new Color().Rainbow())
    ];

    public MenuValue[] Selector =
    [
        new("[ ", new Color().Rainbow()),
        new(" ]", new Color().Rainbow())
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