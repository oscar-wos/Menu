using System.Runtime.InteropServices;

namespace RMenu.Structs;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CBaseUserCmdPB
{
    [FieldOffset(0x50)]
    public float m_flForwardMove;

    [FieldOffset(0x54)]
    public float m_flSideMove;

    [FieldOffset(0x58)]
    public float m_flUpMove;
}
