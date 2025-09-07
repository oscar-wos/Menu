using RMenu.Enums;

namespace RMenu;

public class MenuContinuous<T>
    where T : struct, Enum
{
    private readonly int[] _values = new int[Enum.GetValues(typeof(T)).Length];

    public MenuContinuous()
    {
        foreach (T button in Enum.GetValues(typeof(T)))
        {
            _values[Convert.ToUInt16(button)] = button switch
            {
                MenuButton.Up => 150,
                MenuButton.Down => 150,
                MenuButton.Left => 150,
                MenuButton.Right => 150,
                MenuButton.Select => 0,
                MenuButton.Back => 0,
                MenuButton.Exit => 0,
                MenuButton.Assist => 0,
                _ => 0,
            };
        }
    }

    public int this[T button]
    {
        get => _values[Convert.ToUInt16(button)];
        set => _values[Convert.ToUInt16(button)] = value;
    }
}
