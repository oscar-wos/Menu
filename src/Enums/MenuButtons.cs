namespace Menu.Enums;

public enum MenuButtons : ulong
{
    Jump = (1 << 1),
    Duck = (1 << 2),
    Forward = (1 << 3),
    Back = (1 << 4),
    Left = (1 << 9),
    Right = (1 << 10),
    Scoreboard = ((ulong)1 << 33)
}