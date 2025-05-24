using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
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
        Console.WriteLine(e);
    }

    private void CommandTest(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid)
            return;

        Menu.Add(player, new MenuBase(new MenuValue("new menu", Color.Red)), (player, menu, menuAction, item) =>
        {

        });
    }
}