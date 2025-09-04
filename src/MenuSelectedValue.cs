namespace RMenu;

public class MenuSelectedValue(int index, MenuValue value)
{
    public int Index { get; set; } = index;
    public MenuValue Value { get; set; } = value;
}
