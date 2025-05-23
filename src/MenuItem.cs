using RMenu.Enums;

namespace RMenu;

public class MenuItem(bool pinwheel = false)
{
    private readonly bool Pinwheel = pinwheel;
    private MenuButton LastAction { get; set; } = MenuButton.Left;
}