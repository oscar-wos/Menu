using System.Drawing;
using RMenu.Enums;

namespace RMenu;

public class MenuOptions
{
    public int Priority { get; set; } = 0;
    public int Timeout { get; set; } = 0;
    public bool ProcessInput { get; set; } = true;
    public bool Exitable { get; set; } = true;
    public bool BlockMovement { get; set; } = false;
    public bool BlockJump { get; set; } = false;
    public int ButtonsDelay { get; set; } = 100;
    public MenuInput<MenuButton> Buttons { get; set; } = new();

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