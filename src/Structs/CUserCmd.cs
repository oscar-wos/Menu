using System.Runtime.InteropServices;

namespace RMenu.Structs;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CUserCmd
{
    [FieldOffset(0x40)]
    public CBaseUserCmdPb* BaseUserCmd;

    [FieldOffset(0x58)]
    public CInButtonState ButtonState;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CInButtonState
{
    [FieldOffset(0x8)]
    public ulong PressedButtons;

    [FieldOffset(0x10)]
    public ulong ChangedButtons;

    [FieldOffset(0x18)]
    public ulong ScrollButtons;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CBaseUserCmdPb
{
    [FieldOffset(0x50)]
    public float m_flForwardMove;

    [FieldOffset(0x54)]
    public float m_flSideMove;

    [FieldOffset(0x58)]
    public float m_flUpMove;
}