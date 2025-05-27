using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.UserMessages;
using RMenu.Enums;
using RMenu.Models;
using RMenu.Structs;
using System.Collections.Concurrent;

namespace RMenu;

public static partial class Menu
{
    private const int MENU_HEIGHT = 168;
    private static readonly ConcurrentDictionary<CCSPlayerController, List<Stack<MenuBase>>> _menus = [];
    private static readonly Dictionary<CCSPlayerController, (MenuBase, string)> _currentMenu = [];
    private static readonly MemoryFunctionVoid<CPlayer_MovementServices, IntPtr> _runCommand = new("40 53 56 57 48 81 EC 80 00 00 00 0F");
    private static readonly MemoryFunctionVoid<CPlayer_ObserverServices, int> _changeSpecMode = new("48 89 74 24 18 55 41 56 41 57 48 8D AC");
    private static readonly Timer _menuTimer = new(ProcessMenu, null, 0, 100);
    public static event EventHandler<MenuEvent>? OnPrintMenuPre;

    static Menu()
    {
        NativeAPI.AddListener("OnTick", FunctionReference.Create(OnTick));
        NativeAPI.HookUsermessage(118, (InputArgument)FunctionReference.Create(OnSay), HookMode.Pre);
        _runCommand.Hook(RunCommand, HookMode.Pre);
        _changeSpecMode.Hook(ChangeSpecMode, HookMode.Pre);
    }

    private static HookResult OnSay(UserMessage um)
    {
        var index = um.ReadInt("entityindex");
        var message = um.ReadString("param2");
        var player = Utilities.GetPlayerFromIndex(index);

        if (player is null || !player.IsValid)
            return HookResult.Continue;

        var menu = Get(player);

        if (menu is null || !menu.Text)
            return HookResult.Continue;

        var item = menu.SelectedItem?.Item;
        var value = item?.SelectedValue?.Value;

        if (value is null)
            return HookResult.Continue;

        value.Data = message;
        menu.Text = false;
        return HookResult.Continue;
    }

    private static void OnTick()
    {
        foreach (var (player, (menu, menuString)) in _currentMenu)
        {
            if (player.Connected != PlayerConnectedState.PlayerConnected)
            {
                _menus.Remove(player, out _);
                _currentMenu.Remove(player, out _);
                continue;
            }

            OnPrintMenuPre?.Invoke(null, new MenuEvent(player, menu, menuString));
            player.PrintToCenterHtml(menuString);
        }
    }

    private unsafe static HookResult RunCommand(DynamicHook h)
    {
        var player = h.GetParam<CPlayer_MovementServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();

        if (player is null || !player.IsValid)
            return HookResult.Continue;

        var menu = Get(player);

        if (menu is null || !menu.Options.ProcessInput)
            return HookResult.Continue;

        var userCmd = (CUserCmd*)h.GetParam<IntPtr>(1);

        if (menu.Options.BlockMovement)
        {
            userCmd->BaseUserCmd->m_flForwardMove = 0;
            userCmd->BaseUserCmd->m_flSideMove = 0;
            userCmd->BaseUserCmd->m_flUpMove = 0;
        }

        var buttons = userCmd->ButtonState.PressedButtons | userCmd->ButtonState.ScrollButtons;

        foreach (MenuButton button in Enum.GetValues(typeof(MenuButton)))
        {
            if ((buttons & menu.Options.Buttons[button]) == menu.Options.Buttons[button])
            {
                if (menu.InputDelay[(int)button] + menu.Options.ButtonsDelay > Environment.TickCount64)
                    continue;

                menu.InputDelay[(int)button] = Environment.TickCount64;
                menu.Input(player, button);
            }
        }

        return HookResult.Continue;
    }

    private static HookResult ChangeSpecMode(DynamicHook h)
    {
        var player = h.GetParam<CPlayer_ObserverServices>(0).Pawn.Value.Controller.Value?.As<CCSPlayerController>();

        if (player is null || !player.IsValid)
            return HookResult.Continue;

        var menu = Get(player);

        if (menu is null || !menu.Options.ProcessInput)
            return HookResult.Continue;

        menu.Input(player, MenuButton.Select);
        return HookResult.Stop;
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

                html += menu.Items.Count == 0 && menu.Footer is null ? "" : $"<br>";
            }

            if (menu.Items.Count > 0)
                html += $"</font>{menu.Options.ItemSizeHtml()}";

            for (var i = 0; i < menu.Items.Count; i++)
            {
                var item = menu.Items[i];

                if (item.Type == MenuItemType.Spacer)
                {
                    html += "<br>";
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

                if (i < menu.Items.Count - 1 || menu.Footer is not null)
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
        var html = "";

        if (item.Options.Pinwheel)
        {
            int prevIndex = (currentIndex == 0) ? item.Values.Count - 1 : currentIndex - 1;
            int nextIndex = (currentIndex == item.Values.Count - 1) ? 0 : currentIndex + 1;

            html += $"{item.Values[prevIndex]} ";
            html += $"{FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)}";
            html += $" {item.Values[nextIndex]}";
            return html;
        }
        
        if (currentIndex == 0)
        {
            html += $"{FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)}";

            for (var i = 0; i < 2 && i < item.Values.Count - 1; i++)
                html += $" {item.Values[i + 1]}";
        }
        else if (currentIndex == item.Values.Count - 1)
        {
            for (int i = 2; i > 0; i--)
            {
                if (currentIndex - i >= 0)
                    html += $"{item.Values[currentIndex - i]} ";
            }

            html += $"{FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)}";
        }
        else
            html += $"{item.Values[currentIndex - 1]} {FormatSelector(menu, item, 0)}{item.Values[currentIndex]}{FormatSelector(menu, item, 1)} {item.Values[currentIndex + 1]}";

        return html;
    }

    private static string FormatSelector(MenuBase menu, MenuItem item, int index)
    {
        if (item.Type is (MenuItemType.Button or MenuItemType.ChoiceBool) && item != menu.SelectedItem?.Item)
            return "";

        return menu.Options.Selector[index].ToString();
    }
}