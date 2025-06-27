using System.Collections.Concurrent;
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

    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);
    internal static readonly ConcurrentDictionary<CCSPlayerController, List<MenuBase>> _menus = [];
    internal static readonly ConcurrentDictionary<CCSPlayerController, (MenuBase menu, string html)> _currentMenu = [];
    public static event EventHandler<MenuEvent>? OnPrintMenu;

    static Menu()
    {
        OnSayListener.Register();
        OnTickListener.Register();
        RunCommandHook.Register();
        SpecModeHook.Register();
    }

    internal static string RaiseOnPrintMenu(CCSPlayerController player, MenuBase menu, string html)
    {
        MenuEvent menuEvent = new(player, menu, html);
        OnPrintMenu?.Invoke(null, menuEvent);
        return menuEvent.String;
    }

    private static void ProcessMenu(object? state)
    {
        Rainbow.UpdateRainbowHue();

        _ = Parallel.ForEach(
            _menus,
            kvp =>
            {
                if (kvp.Key is not CCSPlayerController { IsValid: true } player)
                {
                    return;
                }

                if (Get(player) is not MenuBase menu)
                {
                    return;
                }

                StringBuilder html = new();
                _ = html.Append(' ');

                if (menu.Header is not null)
                {
                    _ = html.Append(menu.Options.HeaderSizeHtml());

                    foreach (MenuValue header in menu.Header)
                    {
                        _ = html.Append(header.ToString());
                    }

                    if (menu.Options.DisplayItemsInHeader && menu.SelectedItem is not null)
                    {
                        _ = html.Append("</font>")
                            .Append(menu.Options.FooterSizeHtml())
                            .Append(' ')
                            .Append(menu.SelectedItem.Value.Index + 1)
                            .Append('/')
                            .Append(menu.Items.Count);
                    }

                    _ = html.Append("<br>");
                }

                int start = 0;
                int end = menu.Items.Count;
                int selectedIdx = menu.SelectedItem?.Index ?? 0;

                if (menu.Items.Count > menu.Options._availableItems)
                {
                    int half = menu.Options._availableItems / 2;
                    start = Math.Max(0, selectedIdx - half);
                    end = Math.Min(menu.Items.Count, start + menu.Options._availableItems);

                    if (end - start < menu.Options._availableItems)
                    {
                        start = Math.Max(0, end - menu.Options._availableItems);
                    }
                }

                if (menu.Items.Count > 0)
                {
                    _ = html.Append("</font>").Append(menu.Options.ItemSizeHtml());
                }

                for (int i = start; i < end; i++)
                {
                    MenuItem item = menu.Items[i];

                    if (item.Type is MenuItemType.Spacer)
                    {
                        _ = html.Append(' ').Append("<br>");
                        continue;
                    }

                    if (item == menu.SelectedItem?.Item)
                    {
                        _ = html.Append(menu.Options.Cursor[0].ToString());
                    }

                    if (
                        item.Type is MenuItemType.Button
                        && (item.Values is not List<MenuValue> { Count: > 0 })
                    )
                    {
                        _ = html.Append(FormatSelector(menu, item, 0));
                    }

                    if (item.Head is not null)
                    {
                        _ = html.Append(item.Head.ToString());
                    }

                    switch (item.Type)
                    {
                        case MenuItemType.Button
                        or MenuItemType.Choice
                        or MenuItemType.ChoiceBool:
                            _ = html.Append(FormatValues(menu, item));
                            break;
                    }

                    if (item.Tail is not null)
                    {
                        _ = html.Append(item.Tail.ToString());
                    }

                    if (
                        item.Type == MenuItemType.Button
                        && (item.Values is not List<MenuValue> { Count: > 0 })
                    )
                    {
                        _ = html.Append(FormatSelector(menu, item, 1));
                    }

                    if (item == menu.SelectedItem?.Item)
                    {
                        _ = html.Append(menu.Options.Cursor[1].ToString());
                    }

                    if (i < end - 1 || menu.Footer is not null)
                    {
                        _ = html.Append("<br>");
                    }
                }

                if (menu.Footer is not null)
                {
                    _ = html.Append("</font>").Append(menu.Options.FooterSizeHtml());

                    foreach (MenuValue footer in menu.Footer)
                    {
                        _ = html.Append(footer.ToString());
                    }
                }

                _currentMenu[player] = (menu, html.ToString());
            }
        );
    }

    private static string FormatValues(MenuBase menu, MenuItem item)
    {
        if (item.Values is not List<MenuValue> { Count: > 0 })
        {
            return "";
        }

        int currentIdx = item.SelectedValue?.Index ?? 0;
        int selectedLength = item.Values[currentIdx].Value.Length;
        int remainingChars = menu.Options._availableChars - selectedLength;

        if (item.Head is not null)
        {
            remainingChars -= item.Head.Value.Length;
        }

        if (item.Tail is not null)
        {
            remainingChars -= item.Tail.Value.Length;
        }

        int splitChars = remainingChars / 2 < 1 ? 1 : remainingChars / 2;
        int prevIdx = (currentIdx == 0) ? item.Values.Count - 1 : currentIdx - 1;
        int nextIdx = (currentIdx == item.Values.Count - 1) ? 0 : currentIdx + 1;

        TrimValue(item.Values[prevIdx], splitChars);
        TrimValue(item.Values[nextIdx], splitChars);
        TrimValue(item.Values[currentIdx], remainingChars + selectedLength - (splitChars * 2) + 1);

        StringBuilder result = new();

        if (item.Options.Pinwheel)
        {
            _ = result
                .Append(item.Values[prevIdx].ToString())
                .Append(' ')
                .Append(FormatSelector(menu, item, 0))
                .Append(item.Values[currentIdx].ToString())
                .Append(FormatSelector(menu, item, 1))
                .Append(' ')
                .Append(item.Values[nextIdx].ToString());
        }
        else if (currentIdx == 0)
        {
            _ = result
                .Append(FormatSelector(menu, item, 0))
                .Append(item.Values[currentIdx].ToString())
                .Append(FormatSelector(menu, item, 1));

            for (int i = 0; i < 2 && i < item.Values.Count - 1; i++)
            {
                TrimValue(item.Values[i + 1], splitChars);
                _ = result.Append(' ').Append(item.Values[i + 1].ToString());
            }
        }
        else if (currentIdx == item.Values.Count - 1)
        {
            for (int i = 2; i > 0 && currentIdx - i >= 0; i--)
            {
                TrimValue(item.Values[currentIdx - i], splitChars);
                _ = result.Append(item.Values[currentIdx - i].ToString()).Append(' ');
            }

            _ = result
                .Append(FormatSelector(menu, item, 0))
                .Append(item.Values[currentIdx].ToString())
                .Append(FormatSelector(menu, item, 1));
        }
        else
        {
            _ = result
                .Append(item.Values[prevIdx].ToString())
                .Append(' ')
                .Append(FormatSelector(menu, item, 0))
                .Append(item.Values[currentIdx].ToString())
                .Append(FormatSelector(menu, item, 1))
                .Append(' ')
                .Append(item.Values[nextIdx].ToString());
        }

        return result.ToString();
    }

    private static string FormatSelector(MenuBase menu, MenuItem item, int index) =>
        item.Type is MenuItemType.Button or MenuItemType.ChoiceBool
        && item != menu.SelectedItem?.Item
            ? ""
            : menu.Options.Selector[index].ToString();

    private static void TrimValue(MenuValue value, int maxChars)
    {
        if (value.Value.Length == 1 || value.Value.Length < maxChars)
        {
            value.Display = value.Value;
        }
        else
        {
            value.Display = maxChars < 1 ? "." : value.Value[..(maxChars - 1)] + ".";
        }
    }
}
