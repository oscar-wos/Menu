using System.Drawing;

namespace RMenu.Extensions;

public static class ColorExtension
{
    public static Color Rainbow(this Color _) => Color.FromArgb(0, 0, 0, 0);

    public static Color Strobe(this Color _, byte hueDelta = 60) =>
        Color.FromArgb(1, 0, 255, ClampHue(hueDelta));

    public static Color Strobe(
        this Color _,
        Color startColor,
        Color endColor,
        byte hueDelta = 60
    ) => Color.FromArgb(1, ColorToByte(startColor), ColorToByte(endColor), ClampHue(hueDelta));

    public static Color StrobeReversed(this Color _, byte hueDelta = 60) =>
        Color.FromArgb(2, 0, 255, ClampHue(hueDelta));

    public static Color StrobeReversed(
        this Color _,
        Color startColor,
        Color endColor,
        byte hueDelta = 60
    ) => Color.FromArgb(2, ColorToByte(startColor), ColorToByte(endColor), ClampHue(hueDelta));

    private static byte ClampHue(byte hueDelta) =>
        (byte)(
            hueDelta < 1 ? 1
            : hueDelta > 255 ? 255
            : hueDelta
        );

    private static byte ColorToByte(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));
        float delta = max - min;

        float hue = 0f;

        if (delta != 0f)
        {
            if (max == r)
            {
                hue = (g - b) / delta % 6f;
            }
            else if (max == g)
            {
                hue = ((b - r) / delta) + 2f;
            }
            else
            {
                hue = ((r - g) / delta) + 4f;
            }
        }

        hue *= 60f;

        if (hue < 0f)
        {
            hue += 360f;
        }

        return (byte)(hue / Helpers.Rainbow.HUE_BYTE);
    }
}
