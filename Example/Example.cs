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
            new("wow", new Color().Rainbow()),
            "new menu",
        };

        //var newMenu = new MenuBase("simple header");
        var newMenu = new MenuBase(customHeader, options: options);
        newMenu.AddItem(new(MenuItemType.Button, "button"));
        newMenu.AddItem(new(MenuItemType.Button, new("button2", new Color().Rainbow(), data: "object?", (player, menuAction, menuValue) =>
        {
            if (menuAction == MenuAction.Select)
                Console.WriteLine($"Selected, data: {menuValue.Data}");
        })));
        newMenu.AddItem(new(MenuItemType.Button, new("button3 strobe fast", new Color().RainbowStrobe(254))));
        newMenu.AddItem(new(MenuItemType.Button, new("button4 strobe slow", new Color().RainbowStrobe(30))));
        newMenu.AddItem(new(MenuItemType.Button, new("button6", Color.Green)));

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

        //newMenu.AddItem(new(MenuItemType.Spacer));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));
        //newMenu.AddItem(new(MenuItemType.Text, "text"));

        Menu.Add(player, newMenu, (player, menu, menuAction, item) =>
        {
            Console.WriteLine(menuAction);
        });
    }
}