using System.Runtime.InteropServices;

namespace RMenu.Structs;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CInButtonStatePB
{
    [FieldOffset(0x8)]
    public ulong m_nPressedButtons;

    [FieldOffset(0x10)]
    public ulong m_nChangedButtons;

    [FieldOffset(0x18)]
    public ulong m_nScrollButtons;
}
