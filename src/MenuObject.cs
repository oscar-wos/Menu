using System.Drawing;
using System.Text;
using RMenu.Helpers;

namespace RMenu;

public class MenuObject(string text, MenuFormat? format = null)
{
    public static implicit operator MenuObject(string text) => new(text, new MenuFormat());

    internal string Display { get; set; } = text;
    public string Text { get; set; } = text;
    public MenuFormat Format { get; set; } = format ?? new MenuFormat();

    internal void Render(StringBuilder stringBuilder, MenuFormat? highlight = null)
    {
        /*
        switch (color.A)
        {
            case 0:
                color = Rainbow.CurrentColor;
                break;

            case 1:
                Rainbow.Strobe(stringBuilder, Display, color.R, color.G, color.B, false);
                return;

            case 2:
                Rainbow.Strobe(stringBuilder, Display, color.R, color.G, color.B, true);
                return;

            default:
                break;
        }

        _ = stringBuilder.Append(
            $"<font color=\"#{color.R:X2}{color.G:X2}{color.B:X2}\">{Display}</font>"
        );
        */
    }
}
