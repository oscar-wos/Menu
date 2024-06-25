# Menu

First we need to import .dll into the .csproj
```xml
<ItemGroup>
  <Reference Include="Menu">
    <HintPath>path\to\Menu.dll</HintPath>
  </Reference>
</ItemGroup>
```

> [!WARNING]
> Make sure to delete `Menu.dll` from `yourPlugin.dll` since the library is in `/shared/Menu/Menu.dll` available from Releases

Next we have to create a new Menu object
```cs
using Menu;
using Menu.Enums;

public class Plugin : BasePlugin
{
    public Menu.Menu Menu { get; } = new();
}
```

Now we can have a entry point such as a Command
```cs
public class Plugin : BasePlugin
{
    public Menu.Menu Menu { get; } = new();

    public override void Load(bool isReload)
    {
        AddCommand("css_test", "", (controller, _) =>
        {
            if (controller == null || !controller.IsValid)
                return;
    
            BuildMenu(controller);
        }
    }
}
```

```cs
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

    // If you want to create a sub-menu use the following, this nests the menu ontop of the current menu if there's any, otherwise it follows the same logic as SetMenu
    Menu.AddMenu(controller, mainMenu, (buttons, menu, item) => { });

    // The library automatically handles the deposition of the menu
    // Using Tab (Scoreboard) exists the menu, and Ctrl (Duck) will go back to the previous menu
}
```
