using System.Drawing;

namespace RMenu.Extensions;

public static class ColorExtensions
{
    public static Color Rainbow(this Color _) => Color.FromArgb(0, 0, 0, 0);

    public static Color RainbowStrobe(this Color _, byte hueDelta = 60) =>
        Color.FromArgb(1, ClampHue(hueDelta), 0, 0);

    public static Color RainbowStrobeReversed(this Color _, byte hueDelta = 60) =>
        Color.FromArgb(2, ClampHue(hueDelta), 0, 0);

    private static byte ClampHue(byte hueDelta) =>
        (byte)(
            hueDelta < 1 ? 1
            : hueDelta > 255 ? 255
            : hueDelta
        );
}
