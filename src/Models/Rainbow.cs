using System.Drawing;
using System.Text;

namespace RMenu.Models;

public static class Rainbow
{
    private const double HUE_INCREMENT = 3.233;

    private static readonly Color[] _hueLookup = new Color[360];
    private static double _rainbowHue = 0.0;
    private static Color _rainbowColor = Color.FromArgb(0, 0, 0, 0);

    public static Color Color => _rainbowColor;

    static Rainbow()
    {
        for (int i = 0; i < 360; i++)
            _hueLookup[i] = ComputeColor(i);
    }

    public static void UpdateRainbowHue()
    {
        _rainbowHue = (_rainbowHue + HUE_INCREMENT) % 360;
        _rainbowColor = ColorFromHue(_rainbowHue);
    }

    private static Color ComputeColor(double hue)
    {
        double h = hue / 60.0;
        int hi = (int)h;
        double f = h - hi;
        hi %= 6;

        int v = 255;
        int p = 0;
        int q = (int)(v * (1.0 - f) + 0.5);
        int t = (int)(v * f + 0.5);

        return hi switch
        {
            0 => Color.FromArgb(v, t, p),
            1 => Color.FromArgb(q, v, p),
            2 => Color.FromArgb(p, v, t),
            3 => Color.FromArgb(p, q, v),
            4 => Color.FromArgb(t, p, v),
            _ => Color.FromArgb(v, p, q)
        };
    }

    private static Color ColorFromHue(double hue)
    {
        int index = ((int)Math.Round(hue)) % 360;

        if (index < 0)
            index += 360;

        return _hueLookup[index];
    }

    public static string Strobe(string input, byte hueDelta)
    {
        double fixedStep = (input.Length > 1) ? hueDelta / (input.Length - 1) : hueDelta;

        var sb = new StringBuilder(input.Length * 30);
        for (int i = 0; i < input.Length; i++)
        {
            double offset = hueDelta - i * fixedStep;
            double hue = (_rainbowHue + offset) % 360;

            Color color = ColorFromHue(hue);
            sb.Append($"<font color=\"#{color.R:X2}{color.G:X2}{color.B:X2}\">{input[i]}");
        }

        return sb.ToString();
    }
}