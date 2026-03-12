using System;
using System.Diagnostics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    public static class EventQueueMarshal
    {
        public static void OnKeyboardConnected(int keyboardId)
        {
            SDL_Event e = default;
            e.kdevice.type = SDL_EventType.SDL_EVENT_KEYBOARD_ADDED;
            e.kdevice.timestamp = GetTimestampAsNanoseconds();
            e.kdevice.which = (uint)keyboardId;
            SDL_PushEvent(ref e);
        }

        public static void OnKeyboardDisconnected(int keyboardId)
        {
            SDL_Event e = default;
            e.kdevice.type = SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED;
            e.kdevice.timestamp = GetTimestampAsNanoseconds();
            e.kdevice.which = (uint)keyboardId;
            SDL_PushEvent(ref e);
        }

        public static void OnKeyPressed(int keyboardId, KeyCode key, bool isRepeat)
        {
            SDL_Event e = default;
            e.key.type = SDL_EventType.SDL_EVENT_KEY_DOWN;
            e.key.timestamp = GetTimestampAsNanoseconds();
            e.key.which = (uint)keyboardId;
            e.key.scancode = (SDL_Scancode)key;
            e.key.down = 1;
            e.key.repeat = (byte)(isRepeat ? 1 : 0);
            SDL_PushEvent(ref e);
        }

        public static void OnKeyReleased(int keyboardId, KeyCode key)
        {
            SDL_Event e = default;
            e.key.type = SDL_EventType.SDL_EVENT_KEY_UP;
            e.key.timestamp = GetTimestampAsNanoseconds();
            e.key.which = (uint)keyboardId;
            e.key.scancode = (SDL_Scancode)key;
            SDL_PushEvent(ref e);
        }

        private static ulong GetTimestampAsNanoseconds()
        {
            return (ulong)Stopwatch.GetTimestamp() * TimeSpan.NanosecondsPerTick;
        }
    }
}
