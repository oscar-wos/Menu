using System.Runtime.InteropServices;

namespace RMenu.Structs;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CUserCmd
{
    [FieldOffset(0x40)]
    public CBaseUserCmdPB* m_pBaseUserCmd;

    [FieldOffset(0x58)]
    public CInButtonStatePB m_InButtonState;
}
