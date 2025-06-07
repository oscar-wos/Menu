using CounterStrikeSharp.API.Core;
using RMenu.Enums;
using RMenu.Helpers;
using RMenu.Hooks;
using RMenu.Listeners;
using System.Collections.Concurrent;

namespace RMenu;

public static partial class Menu
{
    internal const int MENU_HEIGHT = 140;
    internal const int MENU_LENGTH = 300;
    internal static readonly ConcurrentDictionary<CCSPlayerController, List<Stack<MenuBase>>> _menus = [];
    internal static readonly Dictionary<CCSPlayerController, (MenuBase, string)> _currentMenu = [];
    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);

    public static event EventHandler<MenuEvent>? OnPrintMenuPre;

    static Menu()
    {
        OnSayListener.Register();
        OnTickListener.Register();
        RunCommandHook.Register();
        SpecModeHook.Register();
    }

    internal static void RaiseOnPrintMenuPre(CCSPlayerController player, MenuBase menu, string menuString)
    {
        OnPrintMenuPre?.Invoke(null, new MenuEvent(player, menu, menuString));
    }

    private static void ProcessMenu(object? state)
    {
        Rainbow.UpdateRainbowHue();

        Parallel.ForEach(_menus, kvp =>
        {
            var player = kvp.Key;

            if (!player.IsValid)
                return;

            var menu = Get(player);

            if (menu is null)
                return;

            var html = "\u200A";

            if (menu.Header is not null)
            {
                html += menu.Options.HeaderSizeHtml();

                foreach (var header in menu.Header)
                    html += header;

                if (menu.Options.DisplayItemsInHeader && menu.SelectedItem is not null)
                    html += $"</font>{menu.Options.FooterSizeHtml()} {menu.SelectedItem.Value.Index + 1}/{menu.Items.Count}";

                html += "<br>";
            }

            var start = 0;
            var end = menu.Items.Count;
            var selectedIndex = menu.SelectedItem?.Index ?? 0;

            if (menu.Items.Count > menu.Options._availableItems)
            {
                var half = menu.Options._availableItems / 2;
                start = Math.Max(0, selectedIndex - half);
                end = Math.Min(menu.Items.Count, start + menu.Options._availableItems);

                if (end - start < menu.Options._availableItems)
                    start = Math.Max(0, end - menu.Options._availableItems);
            }

            if (menu.Items.Count > 0)
                html += $"</font>{menu.Options.ItemSizeHtml()}";

            for (var i = start; i < end; i++)
            {
                var item = menu.Items[i];

                if (item.Type == MenuItemType.Spacer)
                {
                    html += "\u00A0<br>";
                    continue;
                }

                if (item == menu.SelectedItem?.Item)
                    html += menu.Options.Cursor[0];

                if (item.Type == MenuItemType.Button && (item.Values is null || item.Values.Count == 0))
                    html += FormatSelector(menu, item, 0);

                if (item.Head is not null)
                    html += item.Head;

                switch (item.Type)
                {
                    case MenuItemType.Button or MenuItemType.Choice or MenuItemType.ChoiceBool:
                        html += FormatValues(menu, item);
                        break;
                }

                if (item.Tail is not null)
                    html += item.Tail;

                if (item.Type == MenuItemType.Button && (item.Values is null || item.Values.Count == 0))
                    html += FormatSelector(menu, item, 1);

                if (item == menu.SelectedItem?.Item)
                    html += menu.Options.Cursor[1];

                if (i < end - 1 || menu.Footer is not null)
                    html += "<br>";
            }

            if (menu.Footer is not null)
            {
                html += $"</font>{menu.Options.FooterSizeHtml()}";

                foreach (var footer in menu.Footer)
                    html += footer;
            }

            _currentMenu[player] = (menu, html);
        });
    }

    private static string FormatValues(MenuBase menu, MenuItem item)
    {
        if (item.Values is null || item.Values.Count == 0)
            return "";

        var currentIndex = item.SelectedValue?.Index ?? 0;
        var selectedLength = item.Values[currentIndex].Value.Length;
        var remainingChars = menu.Options._availableChars - selectedLength;

        if (item.Head is not null)
            remainingChars -= item.Head.Value.Length;

        if (item.Tail is not null)
            remainingChars -= item.Tail.Value.Length;

        var splitChars = remainingChars / 2 < 1 ? 1 : remainingChars / 2;
        var prevIndex = (currentIndex == 0) ? item.Values.Count - 1 : currentIndex - 1;
        var nextIndex = (currentIndex == item.Values.Count - 1) ? 0 : currentIndex + 1;

        TrimValue(item.Values[prevIndex], splitChars);
        TrimValue(item.Values[nextIndex], splitChars);
        TrimValue(item.Values[currentIndex], remainingChars + selectedLength - (splitChars * 2) + 1);

        if (item.Options.Pinwheel)
            return $"{item.Values[prevIndex]} {FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)} {item.Values[nextIndex]}";

        var html = "";

        if (currentIndex == 0)
        {
            html += $"{FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)}";

            for (var i = 0; i < 2 && i < item.Values.Count - 1; i++)
            {
                TrimValue(item.Values[i + 1], splitChars);
                html += $" {item.Values[i + 1]}";
            }
        }
        else if (currentIndex == item.Values.Count - 1)
        {
            for (var i = 2; i > 0 && currentIndex - i >= 0; i--)
            {
                TrimValue(item.Values[currentIndex - i], splitChars);
                html += $"{item.Values[currentIndex - i]} ";
            }

            html += $"{FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)}";
        }
        else
            html += $"{item.Values[prevIndex]} {FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)} {item.Values[nextIndex]}";

        return html;
    }

    private static string FormatSelector(MenuBase menu, MenuItem item, int index)
    {
        if (item.Type is (MenuItemType.Button or MenuItemType.ChoiceBool) && item != menu.SelectedItem?.Item)
            return "";

        return menu.Options.Selector[index].ToString();
    }

    private static void TrimValue(MenuValue value, int maxChars)
    {
        if (value.Value.Length == 1 || value.Value.Length < maxChars)
            value.Display = value.Value;
        else if (maxChars < 1)
            value.Display = ".";
        else
            value.Display = value.Value[..(maxChars - 1)] + ".";
    }
}