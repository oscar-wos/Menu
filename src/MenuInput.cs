namespace Menus;

public class MenuInput<T> where T : Enum
{
    private readonly ulong[] _values;

    public MenuInput()
    {
        _values = new ulong[Enum.GetValues(typeof(T)).Length];

        foreach (T button in Enum.GetValues(typeof(T)))
            _values[Convert.ToInt32(button)] = Convert.ToUInt64(button);
    }

    public ulong this[T button]
    {
        get => _values[Convert.ToInt64(button)];
        set => _values[Convert.ToInt64(button)] = value;
    }
}