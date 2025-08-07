using System.Text;
using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Helpers;
using RMenu.Hooks;
using RMenu.Listeners;

namespace RMenu;

public static partial class Menu
{
    internal const int MENU_HEIGHT = 140;
    internal const int MENU_LENGTH = 300;

    private static readonly StringBuilder _menuBuilder = new(8192);

    private static readonly Dictionary<int, CCSPlayerController> _players = [];
    private static readonly Dictionary<int, List<MenuBase>> _menus = [];
    private static readonly Dictionary<int, (MenuBase menu, string html)> _currentMenus = [];

    public static readonly IReadOnlyDictionary<int, CCSPlayerController> Players = _players;

    public static readonly IReadOnlyDictionary<int, (MenuBase menu, string html)> CurrentMenus =
        _currentMenus;

    public static event EventHandler<MenuEvent>? OnPrintMenu;

    static Menu()
    {
        OnTickListener.Register();
        RunCommandHook.Register();
        SpecModeHook.Register();

        Thread menuThread = new(MenuThread)
        {
            IsBackground = true,
            Priority = ThreadPriority.Lowest,
        };

        menuThread.Start();
    }

    internal static string RaiseOnPrintMenu(MenuBase menu, string html)
    {
        MenuEvent menuEvent = new(menu, html);
        OnPrintMenu?.Invoke(null, menuEvent);
        return menuEvent.String;
    }

    private static void MenuThread()
    {
        while (true)
        {
            ProcessMenus();
            Rainbow.UpdateHue();
            Thread.Sleep(100);
        }
    }

    internal static void ProcessMenus()
    {
        foreach ((int playerSlot, List<MenuBase> menus) in _menus)
        {
            if (menus.Count == 0)
            {
                continue;
            }

            MenuBase menu = menus[0];
            _currentMenus[playerSlot] = (menu, RenderMenu(menu));
        }
    }

    private static string RenderMenu(MenuBase menu)
    {
        _ = _menuBuilder.Clear();
        _ = _menuBuilder.Append(' ');

        if (menu.Header is not null)
        {
            RenderHeader(_menuBuilder, menu, menu.Header);
        }

        RenderBody(_menuBuilder, menu);

        if (menu.Footer is not null)
        {
            RenderFooter(_menuBuilder, menu, menu.Footer);
        }

        return _menuBuilder.ToString();
    }

    private static void RenderHeader(StringBuilder stringBuilder, MenuBase menu, MenuValue header)
    {
        _ = stringBuilder.Append(menu.Options.HeaderSizeHtml);
        header.Render(stringBuilder);

        if (menu.Options.DisplayItemsInHeader && menu.SelectedItem is not null)
        {
            _ = stringBuilder.Append(
                $"</font>{menu.Options.FooterSizeHtml} {menu.SelectedItem.Value.Index + 1}/{menu.Items.Count}"
            );
        }

        _ = stringBuilder.Append("<br>");
    }

    private static void RenderFooter(StringBuilder stringBuilder, MenuBase menu, MenuValue footer)
    {
        _ = stringBuilder.Append($"</font>{menu.Options.FooterSizeHtml}");
        footer.Render(stringBuilder);
    }

    private static void RenderBody(StringBuilder stringBuilder, MenuBase menu)
    {
        int start = 0;
        int end = menu.Items.Count;
        int selectedIndex = menu.SelectedItem?.Index ?? 0;

        if (menu.Items.Count > menu.Options.AvailableItems)
        {
            int half = menu.Options.AvailableItems / 2;
            start = Math.Max(0, selectedIndex - half);
            end = Math.Min(menu.Items.Count, start + menu.Options.AvailableItems);

            if (end - start < menu.Options.AvailableItems)
            {
                start = Math.Max(0, end - menu.Options.AvailableItems);
            }
        }

        if (menu.Items.Count != 0)
        {
            _ = stringBuilder.Append($"</font>{menu.Options.ItemSizeHtml}");
        }

        for (int i = start; i < end; i++)
        {
            RenderItem(stringBuilder, menu, menu.Items[i]);

            if (i < end - 1 || menu.Footer is not null)
            {
                _ = stringBuilder.Append("<br>");
            }
        }
    }

    private static void RenderItem(StringBuilder stringBuilder, MenuBase menu, MenuItem menuItem)
    {
        if (menuItem.Type is MenuItemType.Spacer)
        {
            _ = stringBuilder.Append(' ');
            return;
        }

        bool isSelected = menuItem == menu.SelectedItem?.Item;

        bool isSingleButton =
            menuItem.Type is MenuItemType.Button && (menuItem.Values is not { Count: > 0 });

        if (isSelected)
        {
            menu.Options.Cursor[0].Render(stringBuilder);
        }

        if (isSingleButton)
        {
            RenderSelector(stringBuilder, menu, menuItem, 0);
        }

        menuItem.Head?.Render(stringBuilder);
        FormatType(stringBuilder, menu, menuItem);
        menuItem.Tail?.Render(stringBuilder);

        if (isSingleButton)
        {
            RenderSelector(stringBuilder, menu, menuItem, 1);
        }

        if (isSelected)
        {
            menu.Options.Cursor[1].Render(stringBuilder);
        }
    }

    private static void RenderSelector(
        StringBuilder stringBuilder,
        MenuBase menu,
        MenuItem menuItem,
        int index
    )
    {
        if (menuItem.Type is MenuItemType.Button && menuItem != menu.SelectedItem?.Item)
        {
            return;
        }

        menu.Options.Selector[index].Render(stringBuilder);
    }

    private static void FormatType(StringBuilder stringBuilder, MenuBase menu, MenuItem menuItem)
    {
        switch (menuItem.Type)
        {
            case MenuItemType.Button or MenuItemType.Choice:
                FormatValues(stringBuilder, menu, menuItem);
                break;
        }
    }

    private static void FormatValues(StringBuilder stringBuilder, MenuBase menu, MenuItem menuItem)
    {
        if (menuItem.Values is not { Count: > 0 })
        {
            return;
        }

        int currentIndex = menuItem.SelectedValue?.Index ?? 0;
        int prevIndex = (currentIndex == 0) ? menuItem.Values.Count - 1 : currentIndex - 1;
        int nextIndex = (currentIndex == menuItem.Values.Count - 1) ? 0 : currentIndex + 1;

        int selectedLength = menuItem.Values[currentIndex].Length();
        int remainingChars = menu.Options.AvailableChars - selectedLength;

        if (menuItem.Head is not null)
        {
            remainingChars -= menuItem.Head.Length();
        }

        if (menuItem.Tail is not null)
        {
            remainingChars -= menuItem.Tail.Length();
        }

        int splitChars = remainingChars / 2 < 1 ? 1 : remainingChars / 2;

        TrimValue(menuItem.Values[prevIndex], splitChars);
        TrimValue(menuItem.Values[nextIndex], splitChars);

        TrimValue(
            menuItem.Values[currentIndex],
            remainingChars + selectedLength - (splitChars * 2)
        );

        if (
            menuItem.Options.Pinwheel
            || (currentIndex > 0 && currentIndex < menuItem.Values.Count - 1)
        )
        {
            menuItem.Values[prevIndex].Render(stringBuilder);
            _ = stringBuilder.Append(' ');
            FormatSelected(stringBuilder, menu, menuItem, currentIndex);
            _ = stringBuilder.Append(' ');
            menuItem.Values[nextIndex].Render(stringBuilder);
        }
        else if (currentIndex == 0)
        {
            FormatSelected(stringBuilder, menu, menuItem, currentIndex);

            for (int i = 0; i < 2 && i < menuItem.Values.Count - 1; i++)
            {
                TrimValue(menuItem.Values[i + 1], splitChars);

                _ = stringBuilder.Append(' ');
                menuItem.Values[i + 1].Render(stringBuilder);
            }
        }
        else
        {
            for (int i = 2; i > 0; i--)
            {
                if (currentIndex - i >= 0)
                {
                    TrimValue(menuItem.Values[currentIndex - i], splitChars);

                    menuItem.Values[currentIndex - i].Render(stringBuilder);
                    _ = stringBuilder.Append(' ');
                }

                FormatSelected(stringBuilder, menu, menuItem, currentIndex);
            }
        }
    }

    private static void FormatSelected(
        StringBuilder stringBuilder,
        MenuBase menu,
        MenuItem menuItem,
        int index
    )
    {
        RenderSelector(stringBuilder, menu, menuItem, 0);
        menuItem.Values![index].Render(stringBuilder, menu.Options.Highlight);
        RenderSelector(stringBuilder, menu, menuItem, 1);
    }

    private static void TrimValue(MenuValue menuValue, int remainingChars)
    {
        remainingChars = Math.Max(1, remainingChars);

        for (int i = 0; i < menuValue.Objects.Count; i++)
        {
            MenuObject menuObject = menuValue.Objects[i];
            TrimObject(menuObject, remainingChars);
            remainingChars -= menuObject.Display.Length;
        }
    }

    private static void TrimObject(MenuObject menuObject, int remainingChars)
    {
        if (remainingChars < 1)
        {
            menuObject.Display = string.Empty;
            return;
        }

        if (menuObject.Text.Length <= remainingChars)
        {
            menuObject.Display = menuObject.Text;
            return;
        }

        menuObject.Display = menuObject.Text[..(remainingChars - 1)] + '.';
    }
}
