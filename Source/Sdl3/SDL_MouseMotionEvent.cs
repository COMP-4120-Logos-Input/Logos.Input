using System;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_MouseMotionEvent
    {
        public SDL_EventType type;
        public UInt32 reserved;
        public UInt64 timestamp;
        public UInt32 windowID;  // SDL_WindowID
        public uint which;
        public UInt32 state;  // SDL_MouseButtonFlags
        public float x;
        public float y;
        public float xrel;
        public float yrel;
    }
}