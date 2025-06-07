using System.Drawing;
using System.Text;

namespace RMenu.Helpers;

internal static class Rainbow
{
    private const double HUE_INCREMENT = 3.233;
    private const int HUE_MAX = 360;

    private static readonly Color[] _hue;
    private static double _currentHue;
    private static Color _currentColor;

    static Rainbow()
    {
        _hue = new Color[HUE_MAX];

        for (int i = 0; i < HUE_MAX; i++)
            _hue[i] = ComputeColor(i);
    }

    public static Color CurrentColor => _currentColor;

    public static void UpdateRainbowHue()
    {
        _currentHue = (_currentHue + HUE_INCREMENT + HUE_MAX) % HUE_MAX;
        _currentColor = GetColorFromHue(_currentHue);
    }

    public static string ApplyStrobeEffect(string input, byte hueDelta, bool isReversed = false)
    {
        var step = input.Length > 1 ? hueDelta / (input.Length - 1) : hueDelta;
        var sb = new StringBuilder(input.Length * 37);

        if (isReversed)
            step = -step;
        
        for (int i = 0; i < input.Length; i++)
        {
            var offset = hueDelta - i * step;
            var hue = (_currentHue + offset + HUE_MAX) % HUE_MAX;

            var color = GetColorFromHue(hue);
            sb.Append($"<font color=\"#{color.R:X2}{color.G:X2}{color.B:X2}\">{input[i]}</font>");
        }

        return sb.ToString();
    }

    private static Color ComputeColor(double hue)
    {
        var h = hue / 60.0;
        var segment = (int)h;
        var fraction = h - segment;
        segment %= 6;

        var v = 255;
        var p = 0;
        var q = (int)(v * (1.0 - fraction) + 0.5);
        var t = (int)(v * fraction + 0.5);

        return segment switch
        {
            0 => Color.FromArgb(v, t, p),
            1 => Color.FromArgb(q, v, p),
            2 => Color.FromArgb(p, v, t),
            3 => Color.FromArgb(p, q, v),
            4 => Color.FromArgb(t, p, v),
            _ => Color.FromArgb(v, p, q)
        };
    }

    private static Color GetColorFromHue(double hue)
    {
        return _hue[((int)hue + HUE_MAX) % HUE_MAX];
    }
}