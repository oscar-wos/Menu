using System.Drawing;
using RMenu.Enums;

namespace RMenu;

public class MenuOptions
{
    public MenuFontSize TitleFontSize { get; set; } = MenuFontSize.M;
    public MenuFontSize ItemFontSize { get; set; } = MenuFontSize.Sm;
    public MenuInput<MenuButton> Buttons { get; set; } = new();
    public bool BlockMovement { get; set; } = false;
    public bool ProcessInput { get; set; } = true;
    public int ButtonsDelay { get; set; } = 200;

    //TODO
    public bool Exitable { get; set; } = true;
    public bool BlockJump { get; set; } = false;
    public int Priority { get; set; } = 0;
    public int Timeout { get; set; } = 0;

    public MenuValue[] Cursor =
    [
        new("►", Color.White),
        new("◄", Color.White)
    ];

    public MenuValue[] Selector =
    [
        new("[ ", Color.White),
        new(" ]", Color.White)
    ];

    public MenuValue[] Bool =
    [
        new("✘", Color.Red),
        new("✔", Color.Green)
    ];

    public MenuValue[] Slider =
    [
        new("(", Color.White),
        new(")", Color.White),
        new("-", Color.White),
        new("|", Color.White)
    ];

    public MenuValue Input = new("________", Color.White);
    public MenuValue Seperator = new(" - ", Color.White);
}