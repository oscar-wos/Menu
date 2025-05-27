using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        Menu.OnPrintMenuPre += OnPrintMenuPre;
        AddCommand("css_test", "", CommandTest);
    }

    public override void Unload(bool hotReload)
    {
        Menu.OnPrintMenuPre -= OnPrintMenuPre;
    }

    private void OnPrintMenuPre(object? sender, MenuEvent e)
    {
        //Console.WriteLine(e);
    }

    private void CommandTest(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid)
            return;

        var options = new MenuOptions()
        {
            BlockMovement = true,
            DisplayItemsInHeader = true
        };

        var customHeader = new List<MenuValue>
        {
            new("wow", new Color().RainbowStrobe()),
            new("header", Color.Green)
        };

        //var newMenu = new MenuBase(header: ["test"], options: options);

        var itemOptions = new MenuItemOptions()
        {
            Pinwheel = false
        };

        var newMenu = new MenuBase(header: customHeader, footer: ["footer: ", new("rasco", new Color().RainbowStrobeReversed(20))], options: options);
        newMenu.AddItem(new(MenuItemType.Button, head: "button: ", values: ["1", "2", "3", "4", "5"]));
        newMenu.AddItem(new(MenuItemType.Button, head: "button2: ", values: [new("ak-47", Color.Red, data: true), new("m4a1", new Color().RainbowStrobe(), data: 2), "3", "4", new("5", data: player)], options: itemOptions));
        newMenu.AddItem(new(MenuItemType.Spacer));
        newMenu.AddItem(new(MenuItemType.Button, new("button", new Color().Rainbow(), data: "object?", (player, menuAction, menuValue) =>
        {
            if (menuAction == MenuAction.Select)
                Console.WriteLine($"Selected, data: {menuValue.Data}");
        })));
        newMenu.AddItem(new(MenuItemType.Button, new("button3 strobe fast", new Color().RainbowStrobe(254))));
        //newMenu.AddItem(new(MenuItemType.Button, new("button4 strobe fast reversed", new Color().RainbowStrobeReversed(254))));
        //newMenu.AddItem(new(MenuItemType.Button, new("button6 strobe slow reversed", new Color().RainbowStrobe(60))));

        /*
        //newMenu.AddItem(new(MenuItemType.Spacer, "lol"));
        newMenu.AddItem(new(MenuItemType.Button, new("green", Color.Green)));
        newMenu.AddItem(new(MenuItemType.Button, new("red", Color.Red)));
        newMenu.AddItem(new(MenuItemType.Button, "blank"));
        newMenu.AddItem(new(MenuItemType.Button, new("blue", Color.Blue)));

        newMenu.AddItem(new(MenuItemType.Button, new("full text", new Color().Rainbow())));
        newMenu.AddItem(new(MenuItemType.Button, new("strobe fast", new Color().RainbowStrobe(254))));
        newMenu.AddItem(new(MenuItemType.Button, new("strobe medium", new Color().RainbowStrobe(128))));
        newMenu.AddItem(new(MenuItemType.Button, new("strobe slow", new Color().RainbowStrobe(30))));
        //newMenu.AddItem(new(MenuItemType.Spacer));

        for (var i = 0; i < 25; i++)
            newMenu.AddItem(new(MenuItemType.Button, $"button {i}"));
        */

        //newMenu.AddItem(new(MenuItemType.Spacer));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));

        Menu.Add(player, newMenu, OnMenuAction);

        void OnMenuAction(CCSPlayerController player, MenuBase menu, MenuAction action, MenuItem? item)
        {
            var itemRowIndex = menu.SelectedItem?.Index;
            Console.WriteLine($"{itemRowIndex}{action} - {item?.SelectedValue?.MenuValue.Data}");

            if (itemRowIndex == 1)
            {
                if (item?.SelectedValue?.MenuValue.Data is CCSPlayerController dataPlayer)
                {
                    menu.Items[2].Type = MenuItemType.Button;
                    menu.Items[2].Values = [new("case", Color.Blue, data: "", callback: OnCaseHarden), "test", "stock"];
                }
                else
                {
                    menu.Items[2].Type = MenuItemType.Spacer;
                    menu.Items[2].Values = null;
                }
            }
        }

        void OnCaseHarden(CCSPlayerController player, MenuAction action, MenuValue? value)
        {
            Console.WriteLine($"Case hardened selected by {player.PlayerName} with value: {value?.Data}");
        }
    }
}