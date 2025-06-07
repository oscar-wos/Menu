using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;
using System.Drawing;
using System.Reflection;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    private Dictionary<CCSPlayerController, MenuBase> _menus = new();

    public override void Load(bool hotReload)
    {
        AddCommand("css_test", "", CommandTest);

        AddCommand("css_test2", "", (player, info) =>
        {
            var menuOptions = new MenuOptions()
            {
                DisplayItemsInHeader = false,
                BlockMovement = true
            };

            var newMenu = new MenuBase(new MenuValue("Skins", new Color().RainbowStrobe()), new MenuValue("kzg", new Color().RainbowStrobeReversed(180)), options: menuOptions);
            newMenu.AddItem(new(MenuItemType.Choice, head: "type: ", values: ["Pistols", "Mid", "Rifles", "Knife", "Gloves", "Model"]));
            //newMenu.AddItem(new(MenuItemType.Choice, values: ))

            var items = typeof(CsItem)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .OrderBy(f => f.MetadataToken) // Preserves declaration order
            .GroupBy(f => (int)f.GetValue(null))
            .Select(g => g.First())         // Select the first declared field for each value
            .Select(f => (CsItem)f.GetValue(null))
            .ToArray();

            var pistols = items.Where(item => (int)item >= 200 && (int)item < 300).ToArray();

            // Filter mid-tier weapons: values between 300 and 312 (i.e. <400)
            var mid = items.Where(item => (int)item >= 300 && (int)item < 400).ToArray();

            // Filter rifles: values between 400 and 410 (i.e. <500)
            var rifles = items.Where(item => (int)item >= 400 && (int)item < 500).ToArray();

            newMenu.AddItem(new(MenuItemType.Choice, head: "weapon: ", values: [.. pistols.Select(p => new MenuValue(p.ToString(), data: p))]));
            newMenu.AddItem(new(MenuItemType.Spacer));


            Menu.Add(player, newMenu, (player, menu, action) =>
            {
                if (action != MenuAction.Update)
                    return;

                if (menu.SelectedItem?.Index == 0)
                {
                    if (menu.SelectedItem?.Item.SelectedValue?.Index == 0)
                        menu.Items[1].Values = [.. pistols.Select(p => new MenuValue(p.ToString(), data: p))];
                    else if (menu.SelectedItem?.Item.SelectedValue?.Index == 1)
                        menu.Items[1].Values = [.. mid.Select(m => new MenuValue(m.ToString(), data: m))];
                    else if (menu.SelectedItem?.Item.SelectedValue?.Index == 2)
                        menu.Items[1].Values = [.. rifles.Select(r => new MenuValue(r.ToString(), data: r))];
                }
            });
        });
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

        void OnMenuAction(CCSPlayerController player, MenuBase menu, MenuAction action)
        {
            if (action != MenuAction.Update)
                return;

            switch (menu.SelectedItem?.Index)
            {
                case 0:
                    menu.Options.HeaderFontSize = (MenuFontSize)menu.SelectedItem?.Item.SelectedValue?.Value.Data;
                    break;

                case 1:
                    menu.Options.ItemFontSize = (MenuFontSize)menu.SelectedItem?.Item.SelectedValue?.Value.Data;
                    break;

                case 2:
                    menu.Options.FooterFontSize = (MenuFontSize)menu.SelectedItem?.Item.SelectedValue?.Value.Data;
                    break;
            }
        }
    }
}