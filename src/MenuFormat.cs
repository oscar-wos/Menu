using System.Drawing;
using RMenu.Enums;

namespace RMenu;

public class MenuFormat(
    Color? color = null,
    MenuStyle style = MenuStyle.None,
    bool canHighlight = true
)
{
    public Color Color { get; set; } = color ?? Color.White;
    public MenuStyle Style { get; set; } = style;
    public bool CanHighlight { get; set; } = canHighlight;
}
