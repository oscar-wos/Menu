using CounterStrikeSharp.API.Core;
using Menus;
using Menus.Enums;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool isReload)
    {
        AddCommand("css_test", "test", (controller, _) =>
        {
            if (controller == null || !controller.IsValid())
                return;

            var test = new MenuValue("test");

            Menu.Add(controller, new MenuBase(), (_, _, _) =>
            {

            });
        });
    }
}