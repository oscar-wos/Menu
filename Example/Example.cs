using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        AddCommand("css_test", "", CommandTest);
    }

    private void CommandTest(CCSPlayerController? player, CommandInfo info)
    {
        Menu.Add(player, new MenuBase(), (_, _, _) =>
        {

        });
    }
}