using System.Drawing;
using RMenu.Enums;
using RMenu.Extensions;

namespace RMenu;

public class MenuOptions
{
    private MenuFontSize _headerFontSize = MenuFontSize.L;
    private MenuFontSize _itemFontSize = MenuFontSize.SM;
    private MenuFontSize _footerFontSize = MenuFontSize.S;

    public string HeaderSizeHtml { get; private set; } = string.Empty;
    public string ItemSizeHtml { get; private set; } = string.Empty;
    public string FooterSizeHtml { get; private set; } = string.Empty;

    public int AvailableChars { get; private set; } = 1;
    public int AvailableItems { get; private set; } = 1;

    public MenuInput<MenuButton> Buttons { get; set; } = new();
    public MenuContinuous<MenuButton> Continuous { get; set; } = new();
    public bool BlockMovement { get; set; } = false;
    public int ButtonsDelay { get; set; } = 150;
    public bool DisplayItemsInHeader { get; set; } = true;
    public bool Exitable { get; set; } = true;
    public int Priority { get; set; } = 0;
    public bool ProcessInput { get; set; } = true;

    //TODO
    public bool BlockJump { get; set; } = false;
    public int Timeout { get; set; } = 0;

    public MenuObject[] Cursor =
    [
        new("►", new MenuFormat(new Color().Rainbow())),
        new("◄", new MenuFormat(new Color().Rainbow())),
    ];

    public MenuObject[] Selector =
    [
        new("[ ", new MenuFormat(new Color().Rainbow())),
        new(" ]", new MenuFormat(new Color().Rainbow())),
    ];

    public MenuFormat? Highlight { get; set; } = null;

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

    public MenuOptions() => UpdateHtml();

    private void UpdateHtml()
    {
        HeaderSizeHtml = $"<font class=\"fontSize-{_headerFontSize.ToString().ToLower()}\">";
        ItemSizeHtml = $"<font class=\"fontSize-{_itemFontSize.ToString().ToLower()}\">";
        FooterSizeHtml = $"<font class=\"fontSize-{_footerFontSize.ToString().ToLower()}\">";

        int availableHeight = Menu.MENU_HEIGHT - ((int)HeaderFontSize + (int)FooterFontSize);
        AvailableChars = (int)(Menu.MENU_LENGTH / ((int)ItemFontSize * 0.6));
        AvailableItems = Math.Max(1, availableHeight / (int)ItemFontSize);
    }
}
