using System.Drawing;

namespace RMenu.Extensions;

public static class ColorExtensions
{
    public static Color Rainbow(this Color _) => Color.FromArgb(0, 0, 0, 0);
    public static Color RainbowStrobe(this Color _, byte hueDelta = 1) => Color.FromArgb(Math.Max((byte)1, Math.Min((byte)254, hueDelta)), 0, 0, 0);
}