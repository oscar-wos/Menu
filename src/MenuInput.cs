using Menus.Enums;

namespace Menus;

public class MenuInput<T> where T : struct, Enum
{
    private readonly ulong[] _values;

    public MenuInput()
    {
        _values = new ulong[Enum.GetValues(typeof(T)).Length];

        foreach (T button in Enum.GetValues(typeof(T)))
        {
            _values[Convert.ToUInt16(button)] = button switch
            {
                MenuButton.Up => 1UL << 3,
                MenuButton.Down => 1UL << 4,
                MenuButton.Left => 1UL << 9,
                MenuButton.Right => 1UL << 10,
                MenuButton.Select => 1UL << 5,
                MenuButton.Back => 1UL << 17,
                MenuButton.Exit => 1UL << 33,
                MenuButton.Special => 1UL << 13,
                _ => 0
            };
        }
    }

    public ulong this[T button]
    {
        get => _values[Convert.ToUInt16(button)];
        set => _values[Convert.ToUInt16(button)] = value;
    }
}