using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Menu;
using Menu.Enums;

namespace pl1;

public class pl1 : BasePlugin
{
    public override string ModuleName => "pl1";
    public override string ModuleVersion => "1.0.0";
    public Menu.Menu Menu { get; } = new();

    public override void Load(bool isReload)
    {
        AddCommand("css_test", "", (controller, _) =>
        {
            var cursor = new MenuValue[2]
            {
                new("►") { Prefix = "<font color=\"#3399FF\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("◄") { Prefix = "<font color=\"#3399FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            };

            var selector = new MenuValue[2]
            {
                new("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
                new(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            };

            var mainMenu = new MenuBase(this, new MenuValue("Main Menu"))
            {
                Cursor = cursor,
                Selector = selector
            };

            var options = new List<MenuValue>
            {
                new("option1") { Prefix = "<font color=\"#9900FF\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("option2"),
                new("option3"),
                new("option4") { Prefix = "<font color=\"#33AA33\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("option5")
            };

            var buttons = new List<MenuValue>
            {
                new("button1") { Prefix = "<font color=\"#AA1133\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("button2"),
                new("button3"),
                new("button4") { Prefix = "<font color=\"#BB9933\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("button5")
            };

            var players = Utilities.GetPlayers().Select(player => new MenuValue(player.PlayerName)).ToList();
            players.Add(new MenuValue("FakePlayer"));

            var item = new MenuItem(MenuItemType.ChoiceBool, options);
            var itemPinwheel = new MenuItem(MenuItemType.Button, new MenuValue("h: "), buttons, new MenuValue(" :t"), true);
            var itemPlayers = new MenuItem(MenuItemType.Choice, new MenuValue("head: ") { Prefix = "<font color=\"#AA33CC\">", Suffix = "<font color=\"#FFFFFF\">" }, players, new MenuValue(" :tail") { Prefix = "<font color=\"#DDAA11\">", Suffix = "<font color=\"#FFFFFF\">" });
            var itemPlayersPinwheel = new MenuItem(MenuItemType.Choice, players, true);

            mainMenu.AddItem(item);
            mainMenu.AddItem(itemPinwheel);
            mainMenu.AddItem(itemPlayers);
            mainMenu.AddItem(itemPlayersPinwheel);
            
            Menu.SetMenu(controller!, mainMenu);
        });
    }
}