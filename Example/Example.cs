using CounterStrikeSharp.API.Core;
using Menus;

namespace example;

public class Example : BasePlugin
{
    public override string ModuleName => "example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool isReload)
    {
        AddCommand("css_test", "test", (controller, info) =>
        {
            if (controller == null || !controller.IsValid())
                return;

            Menu.Add(controller, new MenuBase(new MenuValue("test")), (buttons, @base, arg3) =>
            {

            });
        });
    }
}