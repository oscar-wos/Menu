using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;
using Menu.Enums;

namespace Menu;

public class Menu
{
    public static Dictionary<CCSPlayerController, Stack<MenuBase>> Menus = [];
    private static readonly Timer Timer = new(0.1f, TimerRepeat, TimerFlags.REPEAT);

    public static void TimerRepeat()
    {
        foreach (var (controller, menus) in Menus)
        {
            if (!controller.IsValid || menus.Count == 0)
            {
                Menus.Remove(controller);
                continue;
            }

            var menu = menus.Peek();
            var buttons = (MenuButtons)controller.Buttons;
            var selectItems = menu.Items
                .Where(menuItem => menuItem.Type is not (MenuItemType.Spacer or MenuItemType.Text)).ToList();
            var selectedItem = selectItems[menu.Option];

            if (menu.AcceptButtons)
            {
                switch (buttons)
                {
                    case MenuButtons.Jump:
                        switch (selectedItem.Type)
                        {
                            case MenuItemType.Button:
                                //
                                break;

                            case MenuItemType.Bool:
                                selectedItem.Data[0] = selectedItem.Data[0] == 0 ? 1 : 0;
                                break;

                            case MenuItemType.ChoiceBool:
                                selectedItem.Data[selectedItem.Option] =
                                    selectedItem.Data[selectedItem.Option] == 0 ? 1 : 0;
                                break;

                            case MenuItemType.Input:
                                menu.AcceptInput = true;
                                break;
                        }
                        
                        break;

                    case MenuButtons.Forward when !menu.AcceptInput:
                        menu.Option = menu.Option == 0 ? 0 : menu.Option - 1;
                        break;

                    case MenuButtons.Back when !menu.AcceptInput:
                        menu.Option = menu.Option == selectItems.Count - 1 ? selectItems.Count - 1 : menu.Option + 1;
                        break;

                    case MenuButtons.Left when !menu.AcceptInput:
                        switch (selectedItem.Type)
                        {
                            case (MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button):
                                if (selectedItem.Pinwheel)
                                    selectedItem.Option = selectedItem.Option == 0 ? selectedItem.Values!.Count - 1 : selectedItem.Option - 1;
                                else
                                    selectedItem.Option = selectedItem.Option == 0 ? 0 : selectedItem.Option - 1;
                                break;

                            case (MenuItemType.Slider or MenuItemType.Percentage or MenuItemType.Value):
                                selectedItem.Data[0] = selectedItem.Data[0] == 0 ? 0 : selectedItem.Data[0] - 1;
                                break;
                        }

                        break;

                    case MenuButtons.Right when !menu.AcceptInput:
                        switch (selectedItem.Type)
                        {
                            case (MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button):
                                if (selectedItem.Pinwheel)
                                    selectedItem.Option = selectedItem.Option == selectedItem.Values!.Count - 1 ? 0 : selectedItem.Option + 1;
                                else
                                    selectedItem.Option = selectedItem.Option == selectedItem.Values!.Count - 1
                                        ? selectedItem.Values.Count - 1
                                        : selectedItem.Option + 1;
                                break;

                            case MenuItemType.Slider:
                                selectedItem.Data[0] = selectedItem.Data[0] == 10 ? 10 : selectedItem.Data[0] + 1;
                                break;

                            case MenuItemType.Percentage:
                                selectedItem.Data[0] = selectedItem.Data[0] == 100 ? 100 : selectedItem.Data[0] + 1;
                                break;

                            case MenuItemType.Value:
                                selectedItem.Data[0] += 1;
                                break;
                        }
                        
                        break;

                    case MenuButtons.Duck:
                        menus.Pop();
                        continue;

                    case MenuButtons.Scoreboard:
                        Menus.Remove(controller);
                        continue;
                }
            }

            menu.AcceptButtons = buttons == 0;
            DrawMenu(controller, menu, selectedItem);
        }
    }

    public static void DrawMenu(CCSPlayerController controller, MenuBase menu, MenuItem selectedItem)
    {
        var html = "";
        var menus = Menus[controller];

        if (menus.Count > 1)
        {
            for (var i = 0; i < menus.Count - 2; i++)
            {
                var stackMenu = menus.ElementAt(i);
                html += $"{stackMenu.Title} - ";
            }
        }

        html += menu.Title;

        foreach (var menuItem in menu.Items)
        {
            html += "<br>";

            if (menuItem == selectedItem)
                html += menu.Cursor[(int)MenuCursor.Left];

            if (menuItem.Head != null)
                html += menuItem.Head;

            switch (menuItem.Type)
            {
                case MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button:
                    html += FormatValues(menu, menuItem);
                    break;
            }

            if (menuItem.Tail != null)
                html += menuItem.Tail;

            if (menuItem == selectedItem)
                html += menu.Cursor[(int)MenuCursor.Right];
        }

        controller.PrintToCenterHtml(html);
    }

    private static string FormatValues(MenuBase menu, MenuItem menuItem)
    {
        var html = "";

        if (menuItem.Pinwheel)
        {
            var prev = menuItem.Option - 1;
            var next = menuItem.Option + 1;

            if (prev < 0)
                prev = menuItem.Values!.Count - 1;

            if (next > menuItem.Values!.Count - 1)
                next = 0;

            html += $"{FormatString(menu, menuItem, prev)} ";
            html += $"{menu.Selector[(int)MenuCursor.Left]}{FormatString(menu, menuItem, menuItem.Option)}{menu.Selector[(int)MenuCursor.Right]}";
            html += $" {FormatString(menu, menuItem, next)}";

            return html;
        }

        if (menuItem.Option == 0)
        {
            html += $"{menu.Selector[(int)MenuCursor.Left]}{FormatString(menu, menuItem, 0)}{menu.Selector[(int)MenuCursor.Right]}";

            for (var i = 0; i < 2 && i < menuItem.Values!.Count - 1; i++)
                html += $" {FormatString(menu, menuItem, i + 1)}";
        }
        else if (menuItem.Option == menuItem.Values!.Count - 1)
        {
            for (var i = 2; i > 0; i--)
            {
                if (menuItem.Option - i >= 0)
                    html += $"{FormatString(menu, menuItem, menuItem.Option - i)} ";
            }

            html += $"{menu.Selector[(int)MenuCursor.Left]}{FormatString(menu, menuItem, menuItem.Option)}{menu.Selector[(int)MenuCursor.Right]}";
        }
        else
            html += $"{FormatString(menu, menuItem, menuItem.Option - 1)} {menu.Selector[(int)MenuCursor.Left]}{FormatString(menu, menuItem, menuItem.Option)}{menu.Selector[(int)MenuCursor.Right]} {FormatString(menu, menuItem, menuItem.Option + 1)}";

        return html;
    }

    private static string FormatString(MenuBase menu, MenuItem menuItem, int index)
    {
        var menuValue = menuItem.Values![index];

        if (menuItem.Type != MenuItemType.ChoiceBool)
            return menuValue.ToString();

        menuValue.Prefix = menuItem.Data[index] == 0 ? menu.Bool[(int)MenuBool.False].Prefix : menu.Bool[(int)MenuBool.True].Prefix;
        menuValue.Suffix = menuItem.Data[index] == 0 ? menu.Bool[(int)MenuBool.False].Suffix : menu.Bool[(int)MenuBool.True].Suffix;

        return menuValue.ToString();
    }

    public void SetMenu(CCSPlayerController controller, MenuBase menu)
    {
        if (!Menus.ContainsKey(controller))
            Menus.Add(controller, new Stack<MenuBase>());

        Menus[controller].Clear();
        Menus[controller].Push(menu);
    }

    public void AddMenu(CCSPlayerController controller, MenuBase menu)
    {
        if (!Menus.ContainsKey(controller))
            Menus.Add(controller, new Stack<MenuBase>());

        Menus[controller].Push(menu);
    }
}