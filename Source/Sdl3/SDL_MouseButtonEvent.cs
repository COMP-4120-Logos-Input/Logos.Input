using System;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_MouseButtonEvent
    {
        public SDL_EventType type;
        public UInt32 reserved;
        public UInt64 timestamp;
        public UInt32 windowID;  // SDL_WindowID
        public UInt32 which;  // SDL_MouseID
        public byte button;
        public bool down;
        public byte clicks;
        public byte padding;
        public float x;
        public float y;
    }
}