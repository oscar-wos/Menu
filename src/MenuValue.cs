using System.Collections;
using System.Drawing;
using System.Text;
using RMenu.Enums;

namespace RMenu;

public class MenuValue : IEnumerable<MenuObject>
{
    public static implicit operator MenuValue(List<MenuObject> menuObjects) => new(menuObjects);

    public IEnumerator<MenuObject> GetEnumerator() => Objects.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(string text) => Objects.Add(new MenuObject(text));

    public void Add(MenuObject menuObject) => Objects.Add(menuObject);

    public List<MenuObject> Objects { get; set; } = [];
    public object? Data { get; set; }
    public Action<MenuBase, MenuValue, MenuAction>? Callback { get; }

    public MenuValue() { }

    public MenuValue(
        string text,
        Color? color = null,
        object? data = null,
        Action<MenuBase, MenuValue, MenuAction>? callback = null
    )
    {
        Objects = [new MenuObject(text, new MenuFormat(color))];
        Data = data;
        Callback = callback;
    }

    public MenuValue(
        IEnumerable<string> texts,
        object? data = null,
        Action<MenuBase, MenuValue, MenuAction>? callback = null
    )
    {
        Objects = [.. texts.Select(t => new MenuObject(t))];
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
