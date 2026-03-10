using System;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3;

using SDL_KeyboardID = System.UInt32;
using SDL_WindowID = System.UInt32;
using SDL_Keycode = System.UInt32;
using SDL_Keymod = System.UInt16;
using SDL_Scancode = System.UInt32;

[StructLayout(LayoutKind.Explicit, Size = 128)]
public struct SDL_Event
{
    [FieldOffset(0)]
    private UInt32 type;                            /**< Event type, shared with all events, Uint32 to cover user events which are not in the SDL_EventType enumeration */
    
    [FieldOffset(0)]
    SDL_CommonEvent common;                 /**< Common event data */
    
    [FieldOffset(0)]
    SDL_KeyboardDeviceEvent kdevice;        /**< Keyboard device change event data */
    
    [FieldOffset(0)]
    SDL_KeyboardEvent key;                  /**< Keyboard event data */
    
    [FieldOffset(0)]
    SDL_QuitEvent quit;                     /**< Quit request event data */
}


[StructLayout(LayoutKind.Sequential)]
public struct SDL_CommonEvent
{
    private UInt32 type;
    private UInt32 reserved;
    private UInt64 timestamp;
}

[StructLayout(LayoutKind.Sequential)]
public struct SDL_KeyboardDeviceEvent
{
    private SDL_EventType type;
    private UInt32 reserved;
    private UInt64 timestamp;
    private SDL_KeyboardID which;
}

public enum SDL_EventType : UInt32
{
    SDL_EVENT_FIRST     = 0,     /**< Unused (do not remove) */

    /* Application events */
    SDL_EVENT_QUIT           = 0x100, /**< User-requested quit */

    /* Keyboard events */
    SDL_EVENT_KEY_DOWN        = 0x300, /**< Key pressed */
    SDL_EVENT_KEY_UP,                  /**< Key released */
    SDL_EVENT_KEYMAP_CHANGED,          /**< Keymap changed due to a system event such as an
                                            input language or keyboard layout change. */
    SDL_EVENT_KEYBOARD_ADDED,          /**< A new keyboard has been inserted into the system */
    SDL_EVENT_KEYBOARD_REMOVED,        /**< A keyboard has been removed */
}

[StructLayout(LayoutKind.Sequential)]
public struct SDL_KeyboardEvent
{
    SDL_EventType type;
    UInt32 reserved;
    UInt64 timestamp;
    private SDL_WindowID windowID;
    private SDL_KeyboardID which;
    private UInt32 scancode;
    private SDL_Keycode key;
    private SDL_Keymod mod;
    private UInt16 raw;
    private byte down;  // This needs to be a byte for it to work with SDL_PollEvent for some reason
    private byte repeat; // Same here.. All bools need to be bytes
}

[StructLayout(LayoutKind.Sequential)]
public struct SDL_QuitEvent
{
    SDL_EventType type;
    UInt32 reserved;
    UInt64 timestamp;
}