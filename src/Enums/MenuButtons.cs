namespace Menu.Enums;

public enum MenuButtons : ulong
{
    Select = (1 << 1),
    Back = (1 << 2),
    Up = (1 << 3),
    Down = (1 << 4),
    Left = (1 << 9),
    Right = (1 << 10),
    Exit = ((ulong)1 << 33),
    Input = ((ulong)1 << 63)
}