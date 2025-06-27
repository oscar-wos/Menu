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
