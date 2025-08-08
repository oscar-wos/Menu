namespace RMenu;

public class MenuEvent(MenuBase menu, string html)
{
    public MenuBase Menu { get; } = menu;
    public string String { get; set; } = html;
}
