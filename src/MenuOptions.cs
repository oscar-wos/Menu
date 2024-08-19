using Menus.Enums;

namespace Menus;

public class MenuOptions
{
    public MenuTitleStyle TitleStyle { get; set; } = MenuTitleStyle.Sub;
    public MenuPagination Pagination { get; set; } = MenuPagination.None;
    public MenuInput<MenuButton> Buttons { get; set; } = new();
    public int Priority { get; set; } = 0;
    public float ButtonsFirstDelay { get; set; } = 1;
    public float ButtonsContinuousDelay { get; set; } = 0.2f;
}