using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;
using Menu.Enums;

namespace Menu;

public class Menu
{
    private static readonly Dictionary<CCSPlayerController, Stack<MenuBase>> Menus = [];
    private static readonly Timer Timer = new(0.1f, TimerRepeat, TimerFlags.REPEAT);
    private static readonly SayEvent OnSay = new("say", OnSayEvent);
    private static readonly SayEvent OnSayTeam = new("say_team", OnSayEvent);
    public static event EventHandler<MenuEvent>? OnDrawMenu;

    private static void OnSayEvent(CCSPlayerController? controller, string message)
    {
        if (controller == null || !controller.IsValid || !Menus.TryGetValue(controller, out var value))
            return;

        var menu = value.Peek();

        if (!menu.AcceptInput)
            return;

        var selectedItem = menu.Items[menu.Option];
        selectedItem.DataString = message;
        menu.AcceptInput = false;

        menu.Callback?.Invoke(MenuButtons.Input, menu, selectedItem);
    }

    protected static void RaiseDrawMenu(CCSPlayerController controller, MenuBase menu, MenuItem? selectedItem)
    {
        OnDrawMenu?.Invoke(null, new MenuEvent(controller, menu, selectedItem));
    }

    private static void TimerRepeat()
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
            var selectItems = menu.Items.Where(menuItem => menuItem.Type is not (MenuItemType.Spacer or MenuItemType.Text)).ToList();
            var selectedItem = selectItems.Count > 0 ? selectItems[menu.Option] : null;

            if (menu.AcceptButtons)
            {
                switch (buttons)
                {
                    case MenuButtons.Select:
                        if (selectedItem == null)
                            break;

                        switch (selectedItem.Type)
                        {
                            case MenuItemType.Bool:
                                selectedItem.Data[0] = selectedItem.Data[0] == 0 ? 1 : 0;
                                break;

                            case MenuItemType.ChoiceBool:
                                selectedItem.Data[selectedItem.Option] = selectedItem.Data[selectedItem.Option] == 0 ? 1 : 0;
                                break;

                            case MenuItemType.Input:
                                menu.AcceptInput = true;
                                break;
                        }
                        
                        break;

                    case MenuButtons.Up when !menu.AcceptInput:
                        menu.Option = menu.Option == 0 ? 0 : menu.Option - 1;
                        break;

                    case MenuButtons.Down when !menu.AcceptInput:
                        menu.Option = menu.Option == selectItems.Count - 1 ? selectItems.Count - 1 : menu.Option + 1;
                        break;

                    case MenuButtons.Left when !menu.AcceptInput:
                        if (selectedItem == null)
                            break;

                        switch (selectedItem.Type)
                        {
                            case (MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button):
                                if (selectedItem.Pinwheel)
                                    selectedItem.Option = selectedItem.Option == 0 ? selectedItem.Values!.Count - 1 : selectedItem.Option - 1;
                                else
                                    selectedItem.Option = selectedItem.Option == 0 ? 0 : selectedItem.Option - 1;
                                break;

                            case MenuItemType.Slider:
                                selectedItem.Data[0] = selectedItem.Data[0] == 0 ? 0 : selectedItem.Data[0] - 1;
                                break;

                        }

                        break;

                    case MenuButtons.Right when !menu.AcceptInput:
                        if (selectedItem == null)
                            break;

                        switch (selectedItem.Type)
                        {
                            case (MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button):
                                if (selectedItem.Pinwheel)
                                    selectedItem.Option = selectedItem.Option == selectedItem.Values!.Count - 1 ? 0 : selectedItem.Option + 1;
                                else
                                    selectedItem.Option = selectedItem.Option == selectedItem.Values!.Count - 1 ? selectedItem.Values.Count - 1 : selectedItem.Option + 1;
                                break;

                            case MenuItemType.Slider:
                                selectedItem.Data[0] = selectedItem.Data[0] == 10 ? 10 : selectedItem.Data[0] + 1;
                                break;

                        }
                        
                        break;

                    case MenuButtons.Back:
                        if (menu.AcceptInput)
                            menu.AcceptInput = false;

                        if (menus.Count > 1)
                        {
                            menu.Callback?.Invoke(buttons, menu, null);
                            menus.Pop();
                        }
                            
                        continue;

                    case MenuButtons.Exit:
                        menu.Callback?.Invoke(buttons, menu, null);
                        Menus.Remove(controller);
                        continue;
                }

                if (buttons != 0)
                    menu.Callback?.Invoke(buttons, menu, selectedItem);
            }

            menu.AcceptButtons = buttons == 0;
            DrawMenu(controller, menu, selectedItem);
            RaiseDrawMenu(controller, menu, selectedItem);
        }
    }

    public static void DrawMenu(CCSPlayerController controller, MenuBase menu, MenuItem? selectedItem)
    {
        var html = "";
        var menus = Menus[controller];

        if (menus.Count > 1)
        {
            for (var i = menus.Count - 1; i >= 0; i--)
            {
                var stackMenu = menus.ElementAt(i);

                if (i == menus.Count - 1)
                    html += $"\u00A0{stackMenu.Title.Prefix}{stackMenu.Title.Value}{stackMenu.Separator}";
                else if (i == 0)
                    html += $"{stackMenu.Title.Value}{stackMenu.Title.Suffix}";
                else
                    html += $"{stackMenu.Title.Value}{stackMenu.Separator}";
            }
        }
        else
            html += $"\u00A0{menu.Title}";

        foreach (var menuItem in menu.Items)
        {
            html += $"<br>\u00A0{menu.Title.Suffix}";

            if (selectedItem != null && menuItem == selectedItem)
                html += menu.Cursor[(int)MenuCursor.Left];

            if (menuItem.Head != null)
                html += menuItem.Head;

            switch (menuItem.Type)
            {
                case MenuItemType.Choice or MenuItemType.ChoiceBool or MenuItemType.Button:
                    html += FormatValues(menu, menuItem, selectedItem!);
                    break;

                case MenuItemType.Slider:
                    html += FormatSlider(menu, menuItem);
                    break;

                case MenuItemType.Input:
                    html += FormatInput(menu, menuItem);
                    break;

                case MenuItemType.Bool:
                    html += FormatBool(menu, menuItem);
                    break;
            }

            if (menuItem.Tail != null)
                html += menuItem.Tail;

            if (selectedItem != null && menuItem == selectedItem)
                html += menu.Cursor[(int)MenuCursor.Right];
        }

        controller.PrintToCenterHtml(html);
    }

    private static string FormatValues(MenuBase menu, MenuItem menuItem, MenuItem selectedItem)
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
            html += $"{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Left)}{FormatString(menu, menuItem, menuItem.Option)}{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Right)}";
            html += $" {FormatString(menu, menuItem, next)}";

            return html;
        }

        if (menuItem.Option == 0)
        {
            html += $"{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Left)}{FormatString(menu, menuItem, 0)}{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Right)}";

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

            html += $"{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Left)}{FormatString(menu, menuItem, menuItem.Option)}{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Right)}";
        }
        else
            html += $"{FormatString(menu, menuItem, menuItem.Option - 1)} {FormatSelector(menu, menuItem, selectedItem, MenuCursor.Left)}{FormatString(menu, menuItem, menuItem.Option)}{FormatSelector(menu, menuItem, selectedItem, MenuCursor.Right)} {FormatString(menu, menuItem, menuItem.Option + 1)}";

        return html;
    }

    private static string FormatString(MenuBase menu, MenuItem menuItem, int index)
    {
        if (menuItem.Values == null)
            return "";

        var menuValue = menuItem.Values[index];

        if (menuItem.Type != MenuItemType.ChoiceBool)
            return menuValue.ToString();

        menuValue.Prefix = menuItem.Data[index] == 0 ? menu.Bool[(int)MenuBool.False].Prefix : menu.Bool[(int)MenuBool.True].Prefix;
        menuValue.Suffix = menuItem.Data[index] == 0 ? menu.Bool[(int)MenuBool.False].Suffix : menu.Bool[(int)MenuBool.True].Suffix;

        return menuValue.ToString();
    }

    private static string FormatSelector(MenuBase menu, MenuItem menuItem, MenuItem selectedItem, MenuCursor selector)
    {
        if (menuItem.Type is MenuItemType.Button or MenuItemType.ChoiceBool && menuItem != selectedItem)
            return "";

        return menu.Selector[(int)selector].ToString();
    }

    private static string FormatSlider(MenuBase menu, MenuItem menuItem)
    {
        var html = "";

        html += menu.Slider[(int)MenuSlider.Left].ToString();

        for (var i = 0; i < 11; i++)
            html += $"{(i == menuItem.Data[0] ? menu.Slider[(int)MenuSlider.Selected] : menu.Slider[(int)MenuSlider.Spacer])}{(i != 10 ? " " : "")}";

        html += menu.Slider[(int)MenuSlider.Right].ToString();

        return html;
    }

    private static string FormatInput(MenuBase menu, MenuItem menuItem)
    {
        var html = "";

        if (menu.AcceptInput)
            html += menu.Selector[(int)MenuCursor.Left].ToString();

        if (menuItem.DataString.Length == 0)
            html += menu.Input.ToString();
        else
            html += menuItem.DataString;

        if (menu.AcceptInput)
            html += menu.Selector[(int)MenuCursor.Right].ToString();

        return html;
    }

    private static string FormatBool(MenuBase menu, MenuItem menuItem)
    {
        return menuItem.Data[0] == 0 ? menu.Bool[(int)MenuBool.False].ToString() : menu.Bool[(int)MenuBool.True].ToString();
    }

    public void SetMenu(CCSPlayerController controller, MenuBase menu, Action<MenuButtons, MenuBase, MenuItem?> callback)
    {
        if (!Menus.ContainsKey(controller))
            Menus.Add(controller, new Stack<MenuBase>());

        menu.Callback = callback;
        Menus[controller].Clear();
        Menus[controller].Push(menu);
    }

    public void AddMenu(CCSPlayerController controller, MenuBase menu, Action<MenuButtons, MenuBase, MenuItem?> callback)
    {
        if (!Menus.ContainsKey(controller))
            Menus.Add(controller, new Stack<MenuBase>());

        menu.Callback = callback;
        Menus[controller].Push(menu);
    }

    public void ClearMenus(CCSPlayerController controller)
    {
        Menus.Remove(controller);
    }

    public void PopMenu(CCSPlayerController controller, MenuBase? menu = null)
    {
        if (!Menus.TryGetValue(controller, out var value))
            return;

        if (menu != null && value.Peek() != menu)
            return;

        value.Pop();
    }

    public bool IsCurrentMenu(CCSPlayerController controller, MenuBase menu)
    {
        if (!Menus.TryGetValue(controller, out var value))
            return false;

        return value.Peek() == menu;
    }
}