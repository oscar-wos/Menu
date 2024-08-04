using Menus.Enums;

namespace Menus;

public class MenuOptions : IMenuOptions
{
    public MenuTitleStyle TitleStyle { get; set; } = MenuTitleStyle.Sub;
    public MenuPagination Pagination { get; set; } = MenuPagination.None;
}