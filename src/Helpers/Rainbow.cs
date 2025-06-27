using System.Drawing;
using System.Text;

namespace RMenu.Helpers;

internal static class Rainbow
{
    private const double HUE_INCREMENT = 3.233;
    private const int HUE_MAX = 360;

    private static readonly Color[] _hue;
    private static double _currentHue;

    public static Color CurrentColor { get; private set; }

    static Rainbow()
    {
        _hue = new Color[HUE_MAX];

        for (int i = 0; i < HUE_MAX; i++)
        {
            _hue[i] = ComputeColor(i);
        }
    }

    public static void UpdateRainbowHue()
    {
        _currentHue = (_currentHue + HUE_INCREMENT + HUE_MAX) % HUE_MAX;
        CurrentColor = GetColorFromHue(_currentHue);
    }

    public static string ApplyStrobeEffect(string input, byte hueDelta, bool isReversed = false)
    {
        int step = input.Length > 1 ? hueDelta / (input.Length - 1) : hueDelta;
        StringBuilder sb = new(input.Length * 37);

        if (isReversed)
        {
            step = -step;
        }

        for (int i = 0; i < input.Length; i++)
        {
            int offset = hueDelta - (i * step);
            double hue = (_currentHue + offset + HUE_MAX) % HUE_MAX;

            Color color = GetColorFromHue(hue);
            _ = sb.Append(
                $"<font color=\"#{color.R:X2}{color.G:X2}{color.B:X2}\">{input[i]}</font>"
            );
        }

        return sb.ToString();
    }

    private static Color ComputeColor(double hue)
    {
        double h = hue / 60.0;
        int segment = (int)h;
        double fraction = h - segment;
        segment %= 6;

        int v = 255;
        int p = 0;
        int q = (int)((v * (1.0 - fraction)) + 0.5);
        int t = (int)((v * fraction) + 0.5);

        return segment switch
        {
            0 => Color.FromArgb(v, t, p),
            1 => Color.FromArgb(q, v, p),
            2 => Color.FromArgb(p, v, t),
            3 => Color.FromArgb(p, q, v),
            4 => Color.FromArgb(t, p, v),
            _ => Color.FromArgb(v, p, q),
        };
    }

    private static Color GetColorFromHue(double hue) => _hue[((int)hue + HUE_MAX) % HUE_MAX];
}
