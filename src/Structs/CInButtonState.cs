using System.Runtime.InteropServices;

namespace RMenu.Structs;

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
