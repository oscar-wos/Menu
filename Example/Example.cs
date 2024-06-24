using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Menu;
using Menu.Enums;

namespace example;

public class Example : BasePlugin
{
    public override string ModuleName => "example";
    public override string ModuleVersion => "1.0.0";
    public Menu.Menu Menu { get; } = new();

    private readonly Dictionary<CCSPlayerController, MenuBase> _dynamicMenu = new();

    public Example()
    {
        global::Menu.Menu.OnDrawMenu += (_, menuEvent) =>
        {
            var controller = menuEvent.Controller;

            if (!_dynamicMenu.TryGetValue(controller, out var dynamicMenu))
                return;

            if (menuEvent.Menu != dynamicMenu)
                return;

            var dynamicValue = (DynamicValue)dynamicMenu.Items[0].Head!;
            dynamicValue.Position = controller.PlayerPawn.Value!.AbsOrigin!;
        };
    }

    public override void Load(bool isReload)
    {
        AddCommand("css_test", "", (controller, _) =>
        {
            if (controller == null || !controller.IsValid)
                return;

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

            var mainMenu = new MenuBase(new MenuValue("Main Menu") { Prefix = "<font class=\"fontSize-L\">", Suffix = "<font class=\"fontSize-sm\">" })
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

            var choices = new List<MenuValue>
            {
                new("choice1") { Prefix = "<font color=\"#AA1133\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("choice2"),
                new("choice3"),
                new("choice4") { Prefix = "<font color=\"#BB9933\">", Suffix = "<font color=\"#FFFFFF\">" },
                new("choice5")
            };

            var players = Utilities.GetPlayers().Select(player => (MenuValue)new PlayerValue(player.PlayerName, player.UserId)).ToList();
            players.Add(new PlayerValue("p1", 1) { Prefix = "<font color=\"#AA1133\">", Suffix = "<font color=\"#FFFFFF\">" });
            players.Add(new PlayerValue("p2", 2));

            var itemOptions = new MenuItem(MenuItemType.ChoiceBool, options);
            var itemPinwheel = new MenuItem(MenuItemType.Choice, new MenuValue("h: "), choices, new MenuValue(" :t"), true);
            var itemPlayers = new MenuItem(MenuItemType.Button, new MenuValue("button: ") { Prefix = "<font color=\"#AA33CC\">", Suffix = "<font color=\"#FFFFFF\">" }, players, new MenuValue(" :tail") { Prefix = "<font color=\"#DDAA11\">", Suffix = "<font color=\"#FFFFFF\">" });
            var itemSlider = new MenuItem(MenuItemType.Input, new MenuValue("input: "));

            mainMenu.AddItem(itemOptions);
            mainMenu.AddItem(itemPinwheel);
            mainMenu.AddItem(itemPlayers);
            mainMenu.AddItem(itemSlider);
            mainMenu.AddItem(new MenuItem(MenuItemType.Spacer));

            var customButtons = new List<MenuValue>
            {
                new ButtonValue("Search", ButtonType.Search) { Prefix = "<font color=\"#AA1133\">", Suffix = "<font color=\"#FFFFFF\">" },
                new ButtonValue("Find", ButtonType.Find),
                new ButtonValue("Select", ButtonType.Select) { Prefix = "<font color=\"#AA1133\">", Suffix = "<font color=\"#FFFFFF\">" }
            };

            var itemCustomButtons = new MenuItem(MenuItemType.Button, customButtons);
            mainMenu.AddItem(itemCustomButtons);

            // Menu.SetMenu() to clear the stack and add
            Menu.SetMenu(controller, mainMenu, (buttons, menu, selectedItem) =>
            {
                // MenuButtons.Up, MenuButtons.Down, MenuButtons.Left, MenuButtons.Right are not used in this example
                if (buttons != MenuButtons.Select)
                    return;

                // mainMenu.AddItem(itemCustomButtons) is at index 4, since MenuItemType.Spacer and MenuItemType.Text are not counted (not selectable)
                if (menu.Option == 4)
                {
                    // var customButtons[]
                    var menuValues = selectedItem.Values!;

                    // MenuValue
                    var selectedValue = menuValues[selectedItem.Option];

                    // Cast MenuValue to ButtonValue
                    var buttonValue = (ButtonValue)selectedValue;

                    switch (buttonValue.Button)
                    {
                        case ButtonType.Search:
                            Console.WriteLine("Search button clicked");
                            break;

                        case ButtonType.Find:
                            Console.WriteLine("Find button clicked");
                            break;

                        case ButtonType.Select:
                            CustomSelect(controller, new Vector(0, 0, 0));
                            break;
                    }

                    // Without custom ButtonValue class
                    switch (selectedItem.Option)
                    {
                        case 0:
                            Console.WriteLine("Search button clicked");
                            break;

                        case 1:
                            Console.WriteLine("Find button clicked");
                            break;
                    }

                    // Accomplishes the same as above due to "hard coded" MenuValue.Value, this will not work if using Localized or other dynamic values you don't know ahead of time
                    switch (buttonValue.Value)
                    {
                        case "Search":
                            Console.WriteLine("Search button clicked");
                            break;

                        case "Find":
                            Console.WriteLine("Find button clicked");
                            break;
                    }
                }
            });
        });

        AddCommand("css_test1", "", (controller, _) =>
        {
            if (controller == null || !controller.IsValid)
                return;

            var dynamicMenu = new MenuBase(new MenuValue("Dynamic Menu") { Prefix = "<font class=\"fontSize-L\">", Suffix = "<font class=\"fontSize-sm\">" });

            var dynamicItem = new MenuItem(MenuItemType.Text, new DynamicValue(""));
            dynamicMenu.AddItem(dynamicItem);

            var saveButton = new MenuItem(MenuItemType.Button, [new MenuValue("Save")]);
            dynamicMenu.AddItem(saveButton);

            _dynamicMenu[controller] = dynamicMenu;

            Menu.SetMenu(controller, dynamicMenu, (buttons, menu, _) =>
            {
                if (buttons != MenuButtons.Select)
                    return;

                if (menu.Option != 0)
                    return;

                if (menu.Option == 0)
                {
                    var dynamicValue = (DynamicValue)menu.Items[0].Head!;
                    Console.WriteLine($"{dynamicValue}");

                    CustomSelect(controller, dynamicValue.Position);
                }
            });
        });
    }

    private void CustomSelect(CCSPlayerController controller, Vector pos)
    {
        // Since it's a sub menu setting Prefix will not affect the title as it inherits from Menu[0], Suffix will still work
        var subMenu = new MenuBase(new MenuValue("Sub Menu") { Prefix = "<font class=\"fontSize-XXXL\">", Suffix = "<font class=\"fontSize-m\">" }) ;

        var options = new List<MenuValue>
        {
            new("o1") { Prefix = "<font color=\"#9900FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new("o2"),
            new("o3"),
            new("o4") { Prefix = "<font color=\"#33AA33\">", Suffix = "<font color=\"#FFFFFF\">" },
            new("o5")
        };

        // MenuItemType.ChoiceBool overrides prefix colors to be Menu.Bool[MenuBool.False] and Menu.Bool[MenuBool.True] which can be set the same way as Selector or Cursor above
        var itemOptions = new MenuItem(MenuItemType.ChoiceBool, new MenuValue("options: "), options);
        subMenu.AddItem(itemOptions);
        subMenu.AddItem(new MenuItem(MenuItemType.Bool));

        subMenu.AddItem(new MenuItem(MenuItemType.Spacer));
        subMenu.AddItem(new MenuItem(MenuItemType.Text, new MenuValue($"Saving: {pos.X} {pos.Y} {pos.Z}")));

        // Menu.AddMenu() to add to the stack (sub-menu)
        Menu.AddMenu(controller, subMenu, (buttons, menu, item) =>
        {
            if (buttons != MenuButtons.Select)
                return;

            if (menu.Option == 0)
            {
                var valueItem = item.Values![item.Option];
                Console.WriteLine($"Selected: {valueItem.Value} [{item.Option}]");
            }
            
            if (menu.Option == 1)
                Console.WriteLine($"Bool: {item.Data[0]}");
        });
    }

    private void BuildMenu(CCSPlayerController controller)
    {
        // Create a Menu object which holds all data
        var mainMenu = new MenuBase(new MenuValue("Main Menu") { Prefix = "<font class=\"fontSize-L\">", Suffix = "<font class=\"fontSize-sm\">" });

        // Can add custom formatting, MenuValue[2] Cursor, MenuValue[2] Selector, MenuValue[2] Bool, MenuValue[4] Slider, MenuValue[1] Input
        var cursor = new MenuValue[2]
        {
            // MenuValue is the fundamental building block of everything in the Menu - MenuValue.Value, MenuValue.Prefix, MenuValue.Suffix
            new("--> ") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new(" <--") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }
        };
        
        mainMenu.Cursor = cursor;

        // Let's add a simple text field to the menu, each (row) is a MenuItem which holds data for that (row)
        // Again MenuValue is the fundamental building block of everything in the Menu - MenuValue.Value, MenuValue.Prefix, MenuValue.Suffix

        var textItem = new MenuValue("Welcome to the new menu!");

        // Let's modify the prefix and suffix of the textItem

        textItem.Prefix = "<font color=\"#FF0000\">";
        textItem.Suffix = "<font color=\"#FFFFFF\">";

        // Simplified

        textItem = new MenuValue("Welcome to the new menu!")
        {
            Prefix = "<font color=\"#FF0000\">",
            Suffix = "<font color=\"#FFFFFF\">"
        };

        var simpleTextItem = new MenuItem(MenuItemType.Text, textItem);

        // Now let's add the textItem to the menu
        mainMenu.AddItem(simpleTextItem);

        // And let's add to the global stack to print to the player
        Menu.SetMenu(controller, mainMenu, (buttons, menu, item) => { });

        // The library automatically handles the deposition of the menu
        // Using Tab (Scoreboard) exists the menu, and Ctrl (Duck) will go back to the previous menu
    }
}

public class PlayerValue(string value, int? id) : MenuValue(value)
{
    public int? Id { get; set; } = id;
    //public Player? Player { get; set; }
    //public CCSPlayerController? Controller { get; set; }
}

public class ButtonValue(string value, ButtonType button) : MenuValue(value)
{
    public ButtonType Button { get; set; } = button;
}

public enum ButtonType
{
    Search,
    Find,
    Select
}

public class DynamicValue(string value) : MenuValue(value)
{
    public Vector Position { get; set; } = new(0, 0, 0);

    public override string ToString()
    {
        return $"{Prefix}x: {Position.X} y: {Position.Y} z: {Position.Z}{Suffix}";
    }
}