using System.Drawing;
using RMenu.Enums;
using RMenu.Extensions;

namespace RMenu;

public class MenuOptions
{
    private readonly HashSet<string> _options = [];

    private MenuFontSize _headerFontSize = MenuFontSize.L;
    private MenuFontSize _itemFontSize = MenuFontSize.SM;
    private MenuFontSize _footerFontSize = MenuFontSize.S;

    private bool _blockMovement = false;
    private int _buttonsDelay = 150;
    private bool _displayItemsInHeader = true;
    private bool _exitable = true;
    private int _priority = 0;
    private bool _processInput = true;

    private MenuInput<MenuButton> _buttons = new();
    private MenuContinuous<MenuButton> _continuous = new();

    private MenuObject[] _cursor =
    [
        new("►", new MenuFormat(new Color().Rainbow())),
        new("◄", new MenuFormat(new Color().Rainbow())),
    ];

    private MenuObject[] _selector =
    [
        new("[ ", new MenuFormat(new Color().Rainbow())),
        new(" ]", new MenuFormat(new Color().Rainbow())),
    ];

    private MenuFormat? _highlight = null;

    public MenuOptions() => UpdateHtml();

    public MenuOptions(MenuOptions source)
    {
        _headerFontSize = source._headerFontSize;
        _itemFontSize = source._itemFontSize;
        _footerFontSize = source._footerFontSize;

        _blockMovement = source._blockMovement;
        _buttonsDelay = source._buttonsDelay;
        _displayItemsInHeader = source._displayItemsInHeader;
        _exitable = source._exitable;
        _priority = source._priority;
        _processInput = source._processInput;

        _buttons = new MenuInput<MenuButton>();
        _continuous = new MenuContinuous<MenuButton>();
        _cursor = new MenuObject[source._cursor.Length];
        _selector = new MenuObject[source._selector.Length];

        foreach (MenuButton button in Enum.GetValues<MenuButton>())
        {
            _buttons[button] = source._buttons[button];
        }

        foreach (MenuButton button in Enum.GetValues<MenuButton>())
        {
            _continuous[button] = source._continuous[button];
        }

        for (int i = 0; i < source._cursor.Length; i++)
        {
            MenuObject original = source._cursor[i];

            _cursor[i] = new MenuObject(
                original.Text,
                new MenuFormat(
                    original.Format.Color,
                    original.Format.Style,
                    original.Format.CanHighlight
                )
            );
        }

        for (int i = 0; i < source._selector.Length; i++)
        {
            MenuObject original = source._selector[i];

            _selector[i] = new MenuObject(
                original.Text,
                new MenuFormat(
                    original.Format.Color,
                    original.Format.Style,
                    original.Format.CanHighlight
                )
            );
        }

        if (source._highlight != null)
        {
            _highlight = new MenuFormat(
                source._highlight.Color,
                source._highlight.Style,
                source._highlight.CanHighlight
            );
        }

        _options = [.. source._options];
        UpdateHtml();
    }

    public MenuFontSize HeaderFontSize
    {
        get => _headerFontSize;
        set
        {
            _headerFontSize = value;
            _ = _options.Add(nameof(HeaderFontSize));
            UpdateHtml();
        }
    }
    public MenuFontSize ItemFontSize
    {
        get => _itemFontSize;
        set
        {
            _itemFontSize = value;
            _ = _options.Add(nameof(ItemFontSize));
            UpdateHtml();
        }
    }
    public MenuFontSize FooterFontSize
    {
        get => _footerFontSize;
        set
        {
            _footerFontSize = value;
            _ = _options.Add(nameof(FooterFontSize));
            UpdateHtml();
        }
    }

    public MenuInput<MenuButton> Buttons
    {
        get => _buttons;
        set
        {
            _buttons = value;
            _ = _options.Add(nameof(Buttons));
        }
    }
    public MenuContinuous<MenuButton> Continuous
    {
        get => _continuous;
        set
        {
            _continuous = value;
            _ = _options.Add(nameof(Continuous));
        }
    }
    public bool BlockMovement
    {
        get => _blockMovement;
        set
        {
            _blockMovement = value;
            _ = _options.Add(nameof(BlockMovement));
        }
    }
    public int ButtonsDelay
    {
        get => _buttonsDelay;
        set
        {
            _buttonsDelay = value;
            _ = _options.Add(nameof(ButtonsDelay));
        }
    }
    public bool DisplayItemsInHeader
    {
        get => _displayItemsInHeader;
        set
        {
            _displayItemsInHeader = value;
            _ = _options.Add(nameof(DisplayItemsInHeader));
        }
    }
    public bool Exitable
    {
        get => _exitable;
        set
        {
            _exitable = value;
            _ = _options.Add(nameof(Exitable));
        }
    }
    public int Priority
    {
        get => _priority;
        set
        {
            _priority = value;
            _ = _options.Add(nameof(Priority));
        }
    }
    public bool ProcessInput
    {
        get => _processInput;
        set
        {
            _processInput = value;
            _ = _options.Add(nameof(ProcessInput));
        }
    }
    public MenuObject[] Cursor
    {
        get => _cursor;
        set
        {
            _cursor = value;
            _ = _options.Add(nameof(Cursor));
        }
    }
    public MenuObject[] Selector
    {
        get => _selector;
        set
        {
            _selector = value;
            _ = _options.Add(nameof(Selector));
        }
    }
    public MenuFormat? Highlight
    {
        get => _highlight;
        set
        {
            _highlight = value;
            _ = _options.Add(nameof(Highlight));
        }
    }

    internal string HeaderSizeHtml { get; private set; } = string.Empty;
    internal string ItemSizeHtml { get; private set; } = string.Empty;
    internal string FooterSizeHtml { get; private set; } = string.Empty;
    internal int AvailableChars { get; private set; } = 1;
    internal int AvailableItems { get; private set; } = 1;

    internal void Merge(MenuOptions overrides)
    {
        if (overrides.IsSet(nameof(BlockMovement)))
        {
            BlockMovement = overrides.BlockMovement;
        }

        if (overrides.IsSet(nameof(ButtonsDelay)))
        {
            ButtonsDelay = overrides.ButtonsDelay;
        }

        if (overrides.IsSet(nameof(DisplayItemsInHeader)))
        {
            DisplayItemsInHeader = overrides.DisplayItemsInHeader;
        }

        if (overrides.IsSet(nameof(Exitable)))
        {
            Exitable = overrides.Exitable;
        }

        if (overrides.IsSet(nameof(Priority)))
        {
            Priority = overrides.Priority;
        }

        if (overrides.IsSet(nameof(ProcessInput)))
        {
            ProcessInput = overrides.ProcessInput;
        }

        if (overrides.IsSet(nameof(Highlight)))
        {
            Highlight = overrides.Highlight;
        }

        if (overrides.IsSet(nameof(Cursor)))
        {
            Cursor = overrides.Cursor;
        }

        if (overrides.IsSet(nameof(Selector)))
        {
            Selector = overrides.Selector;
        }

        if (overrides.IsSet(nameof(Buttons)))
        {
            Buttons = overrides.Buttons;
        }

        if (overrides.IsSet(nameof(Continuous)))
        {
            Continuous = overrides.Continuous;
        }

        if (overrides.IsSet(nameof(HeaderFontSize)))
        {
            HeaderFontSize = overrides.HeaderFontSize;
        }

        if (overrides.IsSet(nameof(ItemFontSize)))
        {
            ItemFontSize = overrides.ItemFontSize;
        }

        if (overrides.IsSet(nameof(FooterFontSize)))
        {
            FooterFontSize = overrides.FooterFontSize;
        }
    }

    private bool IsSet(string propertyName) => _options.Contains(propertyName);

    private void UpdateHtml()
    {
        HeaderSizeHtml = $"<font class=\"fontSize-{_headerFontSize.ToString().ToLower()}\">";
        ItemSizeHtml = $"<font class=\"fontSize-{_itemFontSize.ToString().ToLower()}\">";
        FooterSizeHtml = $"<font class=\"fontSize-{_footerFontSize.ToString().ToLower()}\">";

        int availableHeight = Menu.MENU_HEIGHT - ((int)HeaderFontSize + (int)FooterFontSize);

        AvailableChars = (int)(
            (Menu.MENU_LENGTH / ((int)ItemFontSize * 0.6))
            - (
                Cursor[0].Display.Length
                + Cursor[1].Display.Length
                + Selector[0].Display.Length
                + Selector[1].Display.Length
            )
        );

        AvailableItems = Math.Max(1, availableHeight / (int)ItemFontSize);
    }
}
