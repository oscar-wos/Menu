using System.Text;
using RMenu.Enums;
using RMenu.Helpers;
using RMenu.Hooks;
using RMenu.Listeners;
using RMenu.Models;

namespace RMenu;

public static partial class Menu
{
    public const int MAX_PLAYERS = 64;
    internal const int MENU_HEIGHT = 140;
    internal const int MENU_LENGTH = 290;

    private static readonly StringBuilder _menuBuilder = new(8192);
    private static readonly MenuData?[] _menuData = new MenuData[MAX_PLAYERS];

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
        return menuEvent.Html;
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

    private static void ProcessMenus()
    {
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            if (_menuData[i] is not { } menuData)
            {
                continue;
            }

            if (menuData.Menus.Count == 0 || menuData.Menus[0].Count == 0)
            {
                _menuData[i] = null;
                continue;
            }

            MenuBase menu = menuData.Menus[0][^1];
            menuData.Current = (menu, RenderMenu(menu));
        }
    }

    private static string RenderMenu(MenuBase menu)
    {
        _ = _menuBuilder.Clear();
        _ = _menuBuilder.Append(' ');

        if (menu.Header is { } header)
        {
            RenderHeader(_menuBuilder, menu, header);
        }

        RenderBody(_menuBuilder, menu);

        if (menu.Footer is { } footer)
        {
            RenderFooter(_menuBuilder, menu, footer);
        }

        return _menuBuilder.ToString();
    }

    private static void RenderHeader(StringBuilder stringBuilder, MenuBase menu, MenuValue header)
    {
        _ = stringBuilder.Append(menu.Options.HeaderSizeHtml);
        header.Render(stringBuilder);

        if (menu.Options.DisplayItemsInHeader)
        {
            bool isSubMenu = _menuData[menu.Player.Slot]?.Menus[0].Count > 1;

            if (isSubMenu || menu.SelectedItem is not null)
            {
                _ = stringBuilder.Append($"</font>{menu.Options.FooterSizeHtml}");
            }

            if (isSubMenu)
            {
                _ = stringBuilder.Append(" ⇦");
            }

            if (menu.SelectedItem is not null)
            {
                _ = stringBuilder.Append($" {menu.SelectedItem.Index + 1}/{menu.Items.Count}");
            }
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

        int headLength = menuItem.Head?.Length(isSelected ? menu.Options.Highlight : null) ?? 0;
        int tailLength = menuItem.Tail?.Length(isSelected ? menu.Options.Highlight : null) ?? 0;

        if (menuItem.Options.Trim == MenuTrim.Head && menuItem.Head is not null)
        {
            TrimValue(menuItem.Head, menu.Options.AvailableChars - tailLength);
        }
        else if (menuItem.Options.Trim == MenuTrim.Tail && menuItem.Tail is not null)
        {
            TrimValue(menuItem.Tail, menu.Options.AvailableChars - headLength);
        }

        if (isSelected)
        {
            menu.Options.Cursor[0].Render(stringBuilder);
        }

        if (isSingleButton)
        {
            RenderSelector(stringBuilder, menu, menuItem, 0);
        }

        menuItem.Head?.Render(stringBuilder, isSelected ? menu.Options.Highlight : null);
        FormatType(stringBuilder, menu, menuItem);
        menuItem.Tail?.Render(stringBuilder, isSelected ? menu.Options.Highlight : null);

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

        int selectedLength = menuItem.Values[currentIndex].Length(menu.Options.Highlight);
        int remainingChars = menu.Options.AvailableChars - selectedLength;

        if (menuItem.Head is { } head)
        {
            remainingChars -= head.Length();
        }

        if (menuItem.Tail is { } tail)
        {
            remainingChars -= tail.Length();
        }

        int renderItems = 1;

        if (menuItem.Options.Pinwheel || menuItem.Values.Count > 2)
        {
            renderItems = 2;
        }

        int splitChars = remainingChars / renderItems < 1 ? 1 : remainingChars / renderItems;

        TrimValue(menuItem.Values[prevIndex], splitChars);
        TrimValue(menuItem.Values[nextIndex], splitChars);

        TrimValue(
            menuItem.Values[currentIndex],
            remainingChars + selectedLength - (splitChars * renderItems)
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
            }

            FormatSelected(stringBuilder, menu, menuItem, currentIndex);
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
