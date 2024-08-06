using Menus.Enums;

namespace Menus.Interfaces;

public interface IMenuOptions
{
    MenuTitleStyle TitleStyle { get; set; }
    MenuPagination Pagination { get; set; }
    MenuInput<MenuButtons> Buttons { get; set; }

    double ButtonDelayFirst { get; set; }
    double ButtonDelayContinuous { get; set; }
}