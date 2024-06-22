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

    public MenuBase(BasePlugin plugin, MenuValue title, MenuValue[] cursor, MenuValue[] selector)
    {
        Plugin = plugin;
        Title = title;

        Cursor = cursor;
        Selector = selector;
    }

    public MenuBase(BasePlugin plugin, MenuValue title)
    {
        Plugin = plugin;
        Title = title;

        Cursor[(int)MenuCursor.Left] = new MenuValue("►") { Prefix = "<font color=\"#FFFFFF\">" };
        Cursor[(int)MenuCursor.Right] = new MenuValue("◄") { Prefix = "<font color=\"#FFFFFF\">" };

        Selector[(int)MenuCursor.Left] = new MenuValue("[ ") { Prefix = "<font color=\"#FFFFFF\">" };
        Selector[(int)MenuCursor.Right] = new MenuValue(" ]") { Prefix = "<font color=\"#FFFFFF\">" };
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
}