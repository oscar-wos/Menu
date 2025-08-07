using System.Drawing;

namespace RMenu;

public class MenuFormat(
    Color? color = null,
    bool bold = false,
    bool italic = false,
    bool canHighlight = true
)
{
    public Color Color { get; set; } = color ?? Color.White;
    public bool Bold { get; set; } = bold;
    public bool Italic { get; set; } = italic;
    public bool CanHighlight { get; set; } = canHighlight;
}
