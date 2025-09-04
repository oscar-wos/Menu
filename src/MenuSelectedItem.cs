namespace RMenu;

public class MenuSelectedItem(int index, MenuItem item)
{
    public int Index { get; set; } = index;
    public MenuItem Item { get; set; } = item;
}
