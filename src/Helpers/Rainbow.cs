using System.Drawing;
using System.Text;
using RMenu.Extensions;

namespace RMenu.Helpers;

internal static class Rainbow
{
    internal const int HUE_MAX = 360;
    internal const double HUE_BYTE = HUE_MAX / 255.0;
    internal const double HUE_INCREMENT = 5;

    private static readonly Color[] _hue = new Color[HUE_MAX];

    private static double _currentHue;

    static Rainbow()
    {
        for (int i = 0; i < HUE_MAX; i++)
        {
            _hue[i] = ComputeColor(i);
        }
    }

    public static Color CurrentColor { get; private set; }

    public static void Update()
    {
        _currentHue = (_currentHue + HUE_INCREMENT + HUE_MAX) % HUE_MAX;
        CurrentColor = GetColorFromHue(_currentHue);
    }

    public static void Strobe(
        StringBuilder stringBuilder,
        string text,
        MenuFormat format,
        bool isReversed = false
    )
    {
        byte startHue = format.Color.R;
        byte endHue = format.Color.G;
        byte hueDelta = format.Color.B;

        int step = text.Length > 1 ? hueDelta / (text.Length - 1) : hueDelta;

        if (isReversed)
        {
            step = -step;
        }

        _ = stringBuilder.Append($"<font class=\"{format.Style.Value()}\">");

        for (int i = 0; i < text.Length; i++)
        {
            int offset = i * step;
            double strobeHue = (_currentHue + offset + HUE_MAX) % HUE_MAX;

            if (startHue != 0 || endHue != 255)
            {
                double startHue360 = startHue * HUE_BYTE;
                double endHue360 = endHue * HUE_BYTE;

                double phase = strobeHue / HUE_MAX * 2.0;
                double pingPong = phase <= 1.0 ? phase : 2.0 - phase;

                if (Math.Abs(endHue360 - startHue360) > HUE_MAX / 2)
                {
                    if (startHue360 > endHue360)
                    {
                        endHue360 += HUE_MAX;
                    }
                    else
                    {
                        startHue360 += HUE_MAX;
                    }
                }

                strobeHue = startHue360 + (pingPong * (endHue360 - startHue360));
                strobeHue %= HUE_MAX;

                if (strobeHue < 0)
                {
                    strobeHue += HUE_MAX;
                }
            }

            Color color = GetColorFromHue(strobeHue);

            _ = stringBuilder.Append(
                $"<font color=\"#{color.R:X2}{color.G:X2}{color.B:X2}\">{text[i]}</font>"
            );
        }

        _ = stringBuilder.Append($"</font>");
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
