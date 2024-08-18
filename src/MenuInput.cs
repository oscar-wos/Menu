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
                MenuButtons.Up => 1UL << 3,
                MenuButtons.Down => 1UL << 4,
                MenuButtons.Left => 1UL << 9,
                MenuButtons.Right => 1UL << 10,
                MenuButtons.Select => 1UL << 5,
                MenuButtons.Back => 1UL << 17,
                MenuButtons.Exit => 1UL << 33,
                MenuButtons.Special => 1UL << 13,
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