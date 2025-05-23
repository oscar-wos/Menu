using CounterStrikeSharp.API;
using RMenu.Enums;

namespace RMenu;

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
                MenuButton.Up => (ulong)PlayerButtons.Forward,
                MenuButton.Down => (ulong)PlayerButtons.Back,
                MenuButton.Left => (ulong)PlayerButtons.Moveleft,
                MenuButton.Right => (ulong)PlayerButtons.Moveright,
                MenuButton.Select => (ulong)PlayerButtons.Jump,
                MenuButton.Back => (ulong)PlayerButtons.Walk,
                MenuButton.Exit => (ulong)PlayerButtons.Scoreboard,
                MenuButton.Assist => (ulong)PlayerButtons.Inspect,
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