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
        AddCommand("css_test", "", (controller, info) =>
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

            var mainMenu = new MenuBase(this, new MenuValue("Main Menu") { Prefix = "", Suffix = "" }, cursor, selector);

            var options = new List<MenuValue>
            {
                new("option1") { Prefix = "<font color=\"#9900FF\">" },
                new("option2"),
                new("option3"),
                new("option4")  { Prefix = "<font color=\"#33AA33\">", Suffix = "<font color=\"#FFFFFF\">"},
                new("option5"),
                new("option6"),
            };

            var item = new MenuItem(MenuItemType.Button, options);
            var itemPinwheel = new MenuItem(MenuItemType.Button, new MenuValue("h: "), options, new MenuValue(" :t"), true);

            mainMenu.AddItem(item);
            mainMenu.AddItem(itemPinwheel);
            
            Menu.SetMenu(controller!, mainMenu);
        });
    }
}