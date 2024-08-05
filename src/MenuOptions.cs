using Menus.Enums;
using Menus.Interfaces;

namespace Menus;

public class MenuOptions : IMenuOptions
{
    public MenuTitleStyle TitleStyle { get; set; } = MenuTitleStyle.Sub;
    public MenuPagination Pagination { get; set; } = MenuPagination.None;
    public MenuInput<MenuButtons> Buttons { get; set; } = new();


    public double ButtonDelayFirst { get; set; } = 1.0;
    public double ButtonDelayContinuous { get; set; } = 0.1;
}