using System.Drawing;
using System.Reflection;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        AddCommand("css_test", "", CommandTest);

        AddCommand(
            "css_test2",
            "",
            (player, info) =>
            {
                if (player is null || !player.IsValid)
                {
                    return;
                }

                MenuOptions menuOptions = new()
                {
                    DisplayItemsInHeader = false,
                    BlockMovement = true,
                    Highlight = new Color().RainbowStrobe(Color.Red, Color.Blue),
                };

                MenuBase newMenu = new(
                    header: new MenuValue("Skins", new Color().RainbowStrobe()),
                    footer: new MenuValue(
                        "kzg",
                        new Color().RainbowStrobeReversed(Color.Purple, Color.Red)
                    ),
                    options: menuOptions
                );

                newMenu.Items.Add(
                    new(
                        MenuItemType.Choice,
                        head: "type: ",
                        values:
                        [
                            "Pistols",
                            new("Mid", new Color().RainbowStrobe(Color.Blue, Color.LightBlue)),
                            new("Rifles", new Color().RainbowStrobe(Color.Red, Color.Purple)),
                            new(
                                "Knife",
                                new Color().RainbowStrobeReversed(Color.Blue, Color.LightBlue)
                            ),
                            new("Gloves", new Color().RainbowStrobe(Color.Green, Color.Yellow)),
                            new("Model", new Color().RainbowStrobe(Color.Orange, Color.Red)),
                        ]
                    )
                );
                //newMenu.Items.Add(new(MenuItemType.Choice, values: ))

                CsItem[] items =
                [
                    .. typeof(CsItem)
                        .GetFields(BindingFlags.Public | BindingFlags.Static)
                        .OrderBy(f => f.MetadataToken) // Preserves declaration order
                        .GroupBy(f => (int)f.GetValue(null)!)
                        .Select(g => g.First()) // Select the first declared field for each value
                        .Select(f => (CsItem)f.GetValue(null)!),
                ];

                CsItem[] pistols = [.. items.Where(item => (int)item >= 200 && (int)item < 300)];

                // Filter mid-tier weapons: values between 300 and 312 (i.e. <400)
                CsItem[] mid = [.. items.Where(item => (int)item >= 300 && (int)item < 400)];

                // Filter rifles: values between 400 and 410 (i.e. <500)
                CsItem[] rifles = [.. items.Where(item => (int)item >= 400 && (int)item < 500)];

                newMenu.Items.Add(
                    new(
                        MenuItemType.Choice,
                        head: "weapon: ",
                        values: [.. pistols.Select(p => new MenuValue(p.ToString(), data: p))]
                    )
                );

                newMenu.Items.Add(new(MenuItemType.Spacer));

                Menu.Add(
                    player,
                    newMenu,
                    (player, menu, action) =>
                    {
                        if (action != MenuAction.Update)
                        {
                            return;
                        }

                        if (menu.SelectedItem?.Index == 0)
                        {
                            if (menu.SelectedItem?.Item.SelectedValue?.Index == 0)
                            {
                                menu.Items[1].Values =
                                [
                                    .. pistols.Select(p => new MenuValue(p.ToString(), data: p)),
                                ];
                            }
                            else if (menu.SelectedItem?.Item.SelectedValue?.Index == 1)
                            {
                                menu.Items[1].Values =
                                [
                                    .. mid.Select(m => new MenuValue(m.ToString(), data: m)),
                                ];
                            }
                            else if (menu.SelectedItem?.Item.SelectedValue?.Index == 2)
                            {
                                menu.Items[1].Values =
                                [
                                    .. rifles.Select(r => new MenuValue(r.ToString(), data: r)),
                                ];
                            }
                        }
                    }
                );
            }
        );
    }

    private void CommandTest(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuOptions options = new() { BlockMovement = true, DisplayItemsInHeader = true };

        List<MenuValue> customHeader =
        [
            new("wow", new Color().RainbowStrobe()),
            new("header", Color.Green),
        ];

        //var newMenu = new MenuBase(header: ["test"], options: options);

        MenuItemOptions itemOptions = new() { Pinwheel = false };

        List<MenuValue> menuFontSizes =
        [
            .. Enum.GetValues(typeof(MenuFontSize))
                .Cast<MenuFontSize>()
                .Select(fontSize => new MenuValue(fontSize.ToString(), data: fontSize)),
        ];

        MenuBase newMenu = new(
            header: customHeader,
            footer: ["footer: ", new("rasco", new Color().RainbowStrobeReversed(20))],
            options: options
        );

        newMenu.Items.Add(new(MenuItemType.Choice, "h: ", values: menuFontSizes));
        newMenu.Items[0].SelectedValue = (5, menuFontSizes[5]);

        newMenu.Items.Add(new(MenuItemType.Choice, "i: ", values: menuFontSizes));
        newMenu.Items[1].SelectedValue = (2, menuFontSizes[2]);

        newMenu.Items.Add(new(MenuItemType.Choice, "f: ", values: menuFontSizes));
        newMenu.Items[2].SelectedValue = (1, menuFontSizes[1]);

        newMenu.Items.Add(
            new(
                MenuItemType.Choice,
                values:
                [
                    "Dragonfire",
                    "Acid Fade",
                    "Turbo Peak",
                    "Big Iron",
                    new("Blood in the Water", new Color().RainbowStrobe()),
                ],
                options: itemOptions
            )
        );

        newMenu.Items.Add(new(MenuItemType.Spacer));

        newMenu.Items.Add(
            new(
                MenuItemType.Choice,
                head: "choice: ",
                values:
                [
                    new("ak-47", Color.Red, data: true),
                    new("m4a1", new Color().RainbowStrobe(), data: 2),
                    "3",
                    "4",
                    new("5", data: player),
                ]
            )
        );

        newMenu.Items.Add(new(MenuItemType.Spacer));

        // Value callback
        newMenu.Items.Add(
            new(
                MenuItemType.Button,
                options: new() { Pinwheel = false },
                values:
                [
                    new(
                        "button",
                        new Color().Rainbow(),
                        data: "multi button1?",
                        (player, menuValue, menuAction) =>
                        {
                            if (menuAction == MenuAction.Select)
                            {
                                Console.WriteLine($"Selected, data: {menuValue.Data}");
                            }
                        }
                    ),
                    new(
                        "button2",
                        new Color().Rainbow(),
                        data: "multi button2?",
                        (player, menuValue, menuAction) =>
                        {
                            if (menuAction == MenuAction.Select)
                            {
                                Console.WriteLine($"Selected, data: {menuValue.Data}");
                            }
                        }
                    ),
                ]
            )
        );

        // Item callback
        newMenu.Items.Add(
            new(
                MenuItemType.Button,
                new("button", new Color().Rainbow()),
                data: "single button",
                callback: (player, menuItem, menuAction) =>
                {
                    if (menuAction == MenuAction.Select)
                    {
                        Console.WriteLine($"Selected, data: {menuItem.Data}");
                    }
                }
            )
        );

        newMenu.Items.Add(
            new(
                MenuItemType.Button,
                new("button2 strobe fast: ", new Color().RainbowStrobe(254)),
                ["1", "2"]
            )
        );

        newMenu.Items.Add(new(MenuItemType.Button, values: ["1"], options: itemOptions));
        //newMenu.Items.Add(new(MenuItemType.Button, new("button4 strobe fast reversed", new Color().RainbowStrobeReversed(254))));
        //newMenu.Items.Add(new(MenuItemType.Button, new("button6 strobe slow reversed", new Color().RainbowStrobe(60))));

        /*
        //newMenu.Items.Add(new(MenuItemType.Spacer, "lol"));
        newMenu.Items.Add(new(MenuItemType.Button, new("green", Color.Green)));
        newMenu.Items.Add(new(MenuItemType.Button, new("red", Color.Red)));
        newMenu.Items.Add(new(MenuItemType.Button, "blank"));
        newMenu.Items.Add(new(MenuItemType.Button, new("blue", Color.Blue)));

        newMenu.Items.Add(new(MenuItemType.Button, new("full text", new Color().Rainbow())));
        newMenu.Items.Add(new(MenuItemType.Button, new("strobe fast", new Color().RainbowStrobe(254))));
        newMenu.Items.Add(new(MenuItemType.Button, new("strobe medium", new Color().RainbowStrobe(128))));
        newMenu.Items.Add(new(MenuItemType.Button, new("strobe slow", new Color().RainbowStrobe(30))));
        //newMenu.Items.Add(new(MenuItemType.Spacer));
        */
        for (int i = 0; i < 3; i++)
        {
            newMenu.Items.Add(new(MenuItemType.Button, $"button {i}"));
        }

        //newMenu.Items.Add(new(MenuItemType.Spacer));
        //newMenu.Items.Add(new(MenuItemType.Text, "text"));
        //newMenu.Items.Add(new(MenuItemType.Text, "text"));

        Menu.Add(player, newMenu, OnMenuAction);

        void OnMenuAction(CCSPlayerController player, MenuBase menu, MenuAction action)
        {
            if (action != MenuAction.Update)
                return;

            switch (menu.SelectedItem?.Index)
            {
                case 0:
                    menu.Options.HeaderFontSize = (MenuFontSize)
                        menu.SelectedItem?.Item.SelectedValue?.Value.Data!;
                    break;

                case 1:
                    menu.Options.ItemFontSize = (MenuFontSize)
                        menu.SelectedItem?.Item.SelectedValue?.Value.Data!;
                    break;

                case 2:
                    menu.Options.FooterFontSize = (MenuFontSize)
                        menu.SelectedItem?.Item.SelectedValue?.Value.Data!;
                    break;
            }
        }
    }
}
