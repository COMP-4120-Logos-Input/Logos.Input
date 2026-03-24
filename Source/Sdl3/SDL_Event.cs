using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    internal struct SDL_Event
    {
        [FieldOffset(0)]
        public SDL_EventType type;

        [FieldOffset(0)]
        public SDL_CommonEvent common;

        [FieldOffset(0)]
        public SDL_KeyboardDeviceEvent kdevice;

        [FieldOffset(0)]
        public SDL_KeyboardEvent key;

        [FieldOffset(0)]
        public SDL_QuitEvent quit;

        [FieldOffset(0)] 
        public SDL_MouseDeviceEvent mdevice;
        
        [FieldOffset(0)]
        public SDL_MouseMotionEvent motion;
        
        [FieldOffset(0)]
        public SDL_MouseButtonEvent button;
        
        [FieldOffset(0)]
        public SDL_MouseWheelEvent wheel;
    }
}
