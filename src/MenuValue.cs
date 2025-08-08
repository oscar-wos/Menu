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

    internal int Length()
    {
        int length = 0;

        for (int i = 0; i < Objects.Count; i++)
        {
            length += Objects[i].Text.Length;
        }

        return length;
    }
}
