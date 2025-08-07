using System.Drawing;
using RMenu.Enums;

namespace RMenu;

public class MenuFormat(
    Color? color = null,
    bool bold = false,
    MenuFont font = MenuFont.Default,
    bool canHighlight = true
)
{
    public Color Color { get; set; } = color ?? Color.White;
    public bool Bold { get; set; } = bold;
    public MenuFont Font { get; set; } = font;
    public bool CanHighlight { get; set; } = canHighlight;
}
