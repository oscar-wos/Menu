using Menus.Enums;

namespace Menus;

public interface IMenuOptions
{
    MenuTitleStyle TitleStyle { get; set; }
    MenuPagination Pagination { get; set; }
}