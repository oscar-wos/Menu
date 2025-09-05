using System.Text;
using RMenu.Enums;

namespace RMenu;

public class MenuValue
{
    public static implicit operator MenuValue(List<MenuObject> menuObjects) => new(menuObjects);

    public List<MenuObject> Objects { get; set; } = [];
    public object? Data { get; set; }
    public Action<MenuBase, MenuValue, MenuAction>? Callback { get; }

    public MenuValue(
        string text,
        MenuFormat? format = null,
        object? data = null,
        Action<MenuBase, MenuValue, MenuAction>? callback = null
    )
    {
        Objects = [new MenuObject(text, format)];
        Data = data;
        Callback = callback;
    }

    public MenuValue(
        IEnumerable<MenuObject> values,
        object? data = null,
        Action<MenuBase, MenuValue, MenuAction>? callback = null
    )
    {
        Objects = [.. values];
        Data = data;
        Callback = callback;
    }

    internal void Render(StringBuilder stringBuilder, MenuFormat? highlight = null)
    {
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].Render(stringBuilder, highlight);
        }
    }

    internal int Length(MenuFormat? highlight = null)
    {
        double length = 0;

        for (int i = 0; i < Objects.Count; i++)
        {
            double objectLength = Objects[i].Text.Length;
            MenuFormat format = highlight ?? Objects[i].Format;

            if (format.Style == MenuStyle.Mono)
            {
                objectLength *= 1.2;
            }

            length += objectLength;
        }

        return (int)length;
    }
}
