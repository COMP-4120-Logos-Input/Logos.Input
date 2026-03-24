using System;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3;

[StructLayout(LayoutKind.Sequential)]
internal struct SDL_MouseWheelEvent
{
    public SDL_EventType type;
    public UInt32 reserved;
    public UInt64 timestamp;
    public UInt32 windowID;  // SDL_WindowID
    public uint which;  // SDL_MouseID
    public float x;
    public float y;
    public SDL_MouseWheelDirection direction;
    public float mouse_x;
    public float mouse_y;
    public int integer_x;
    public int integer_y;
}
