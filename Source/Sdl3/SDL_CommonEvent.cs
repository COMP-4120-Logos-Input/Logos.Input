using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_CommonEvent
    {
        public SDL_EventType type;
        public uint reserved;
        public ulong timestamp;
    }
}
