using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;
using System.Drawing;

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

        var menuFontSizes = Enum.GetValues(typeof(MenuFontSize)).Cast<MenuFontSize>().Select(fontSize => new MenuValue(fontSize.ToString(), data: fontSize)).ToList();

        var newMenu = new MenuBase(header: customHeader, footer: ["footer: ", new("rasco", new Color().RainbowStrobeReversed(20))], options: options);
        newMenu.AddItem(new(MenuItemType.Choice, "h: ", values: menuFontSizes));
        newMenu.Items[0].SelectedValue = (5, menuFontSizes[5]);

        newMenu.AddItem(new(MenuItemType.Choice, "i: ", values: menuFontSizes));
        newMenu.Items[1].SelectedValue = (2, menuFontSizes[2]);

        newMenu.AddItem(new(MenuItemType.Choice, "f: ", values: menuFontSizes));
        newMenu.Items[2].SelectedValue = (1, menuFontSizes[1]);
        
        newMenu.AddItem(new(MenuItemType.Choice, values: ["Dragonfire", "Acid Fade", "Turbo Peak", "Big Iron", new("Blood in the Water", new Color().RainbowStrobe())], options: itemOptions));
        newMenu.AddItem(new(MenuItemType.Spacer));
        newMenu.AddItem(new(MenuItemType.Choice, head: "choice: ", values: [new("ak-47", Color.Red, data: true), new("m4a1", new Color().RainbowStrobe(), data: 2), "3", "4", new("5", data: player)]));
        newMenu.AddItem(new(MenuItemType.Spacer));
        newMenu.AddItem(new(MenuItemType.Button, new("button", new Color().Rainbow(), data: "object?", (player, menuAction, menuValue) =>
        {
            if (menuAction == MenuAction.Select)
                Console.WriteLine($"Selected, data: {menuValue.Data}");
        })));
        newMenu.AddItem(new(MenuItemType.Button, new("button2 strobe fast: ", new Color().RainbowStrobe(254)), ["1", "2"]));
        newMenu.AddItem(new(MenuItemType.Button, values: ["1"], options: itemOptions));
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
        */
        for (var i = 0; i < 3; i++)
            newMenu.AddItem(new(MenuItemType.Button, $"button {i}"));

        //newMenu.AddItem(new(MenuItemType.Spacer));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));

        Menu.Add(player, newMenu, OnMenuAction);

        void OnMenuAction(CCSPlayerController player, MenuBase menu, MenuAction action, MenuItem? item)
        {
            if (action != MenuAction.Update)
                return;

            switch (menu.SelectedItem?.Index)
            {
                case 0:
                    menu.Options.HeaderFontSize = (MenuFontSize)item?.SelectedValue?.Value.Data;
                    break;

                case 1:
                    menu.Options.ItemFontSize = (MenuFontSize)item?.SelectedValue?.Value.Data;
                    break;

                case 2:
                    menu.Options.FooterFontSize = (MenuFontSize)item?.SelectedValue?.Value.Data;
                    break;
            }
        }
    }
}