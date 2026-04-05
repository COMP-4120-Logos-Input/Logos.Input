using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3;

[StructLayout(LayoutKind.Sequential)]
internal struct SDL_WindowEvent
{
    public SDL_EventType type;
    public uint reserved;
    public ulong timestamp;
    public uint windowID;
    public int data1;
    public int data2;
}