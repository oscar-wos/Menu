using CounterStrikeSharp.API.Core;
using Menu.Enums;

namespace Menu;

public class MenuBase
{
    public BasePlugin Plugin { get; init; }
    public MenuValue Title { get; set; }
    public List<MenuItem> Items { get; set; } = [];
    public int Option { get; set; } = 0;

    public bool AcceptButtons { get; set; } = false;
    public bool AcceptInput { get; set; } = false;

    public MenuValue[] Cursor = new MenuValue[Enum.GetValues(typeof(MenuCursor)).Length];
    public MenuValue[] Selector = new MenuValue[Enum.GetValues(typeof(MenuCursor)).Length];

    public MenuBase(BasePlugin plugin, MenuValue title)
    {
        Plugin = plugin;
        Title = title;

        Cursor[(int)MenuCursor.Left] = new MenuValue("►");
        Cursor[(int)MenuCursor.Right] = new MenuValue("◄");

        Selector[(int)MenuCursor.Left] = new MenuValue("[ ");
        Selector[(int)MenuCursor.Right] = new MenuValue(" ]");
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
}