using System;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_MouseDeviceEvent
    {
        public SDL_EventType type;
        public UInt32 reserved;
        public UInt64 timestamp;
        public uint which;
    }
}

