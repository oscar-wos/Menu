namespace RMenu.Models;

public class MenuEvent(MenuBase menu, string html)
{
    public MenuBase Menu { get; } = menu;
    public string Html { get; set; } = html;
}
