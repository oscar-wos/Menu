using CounterStrikeSharp.API;
using RMenu.Enums;

namespace RMenu;

public class MenuInput<T>
    where T : struct, Enum
{
    private readonly PlayerButtons[] _values = new PlayerButtons[Enum.GetValues(typeof(T)).Length];

    public MenuInput()
    {
        foreach (T button in Enum.GetValues(typeof(T)))
        {
            _values[Convert.ToUInt16(button)] = button switch
            {
                MenuButton.Up => PlayerButtons.Forward,
                MenuButton.Down => PlayerButtons.Back,
                MenuButton.Left => PlayerButtons.Moveleft,
                MenuButton.Right => PlayerButtons.Moveright,
                MenuButton.Select => PlayerButtons.Jump,
                MenuButton.Back => PlayerButtons.Walk,
                MenuButton.Exit => PlayerButtons.Scoreboard,
                MenuButton.Assist => PlayerButtons.Inspect,
                _ => 0,
            };
        }
    }

    public PlayerButtons this[T button]
    {
        get => _values[Convert.ToUInt16(button)];
        set => _values[Convert.ToUInt16(button)] = value;
    }
}
