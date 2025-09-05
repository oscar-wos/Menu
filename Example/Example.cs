using CounterStrikeSharp.API.Core;

namespace Example;

public partial class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        AddCommand("css_example1", "", Example1Menu);
        AddCommand("css_example2", "", Example2Menu);
        AddCommand("css_example3", "", Example3Menu);
        AddCommand("css_example4", "", Example4Menu);
        AddCommand("css_example5", "", Example5Menu);
        AddCommand("css_example6", "", Example6Menu);
        AddCommand("css_example7", "", Example7Menu);
    }
}
