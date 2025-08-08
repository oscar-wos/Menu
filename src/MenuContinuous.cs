using RMenu.Enums;

namespace RMenu;

public class MenuContinuous<T>
    where T : struct, Enum
{
    private readonly bool[] _values = new bool[Enum.GetValues(typeof(T)).Length];

    public MenuContinuous()
    {
        foreach (T button in Enum.GetValues(typeof(T)))
        {
            _values[Convert.ToUInt16(button)] = button switch
            {
                MenuButton.Up => true,
                MenuButton.Down => true,
                MenuButton.Left => true,
                MenuButton.Right => true,
                MenuButton.Select => false,
                MenuButton.Exit => false,
                MenuButton.Assist => false,
                _ => false,
            };
        }
    }

    public bool this[T button]
    {
        get => _values[Convert.ToUInt16(button)];
        set => _values[Convert.ToUInt16(button)] = value;
    }
}
