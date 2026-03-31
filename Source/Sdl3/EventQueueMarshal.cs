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

        public static void OnMouseConnected(int mouseId)
        {
            SDL_Event e = default;
            e.mdevice.type = SDL_EventType.SDL_EVENT_MOUSE_ADDED;
            e.mdevice.timestamp = GetTimestampAsNanoseconds();
            e.mdevice.which = (uint)mouseId;
            SDL_PushEvent(ref e);
        }

        public static void OnMouseDisconnected(int mouseId)
        {
            SDL_Event e = default;
            e.mdevice.type = SDL_EventType.SDL_EVENT_MOUSE_REMOVED;
            e.mdevice.timestamp = GetTimestampAsNanoseconds();
            e.mdevice.which = (uint)mouseId;
            SDL_PushEvent(ref e);
        }

        public static void OnMouseButtonPressed(int mouseId, MouseButton button)
        {
            SDL_Event e = default;
            e.button.type = SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN;
            e.button.timestamp = GetTimestampAsNanoseconds();
            e.button.which = (uint)mouseId;
            e.button.button = MouseButtonToSdlButton(button);
            e.button.down = 1;
            SDL_PushEvent(ref e);
        }

        public static void OnMouseButtonReleased(int mouseId, MouseButton button)
        {
            SDL_Event e = default;
            e.button.type = SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP;
            e.button.timestamp = GetTimestampAsNanoseconds();
            e.button.which = (uint)mouseId;
            e.button.button = MouseButtonToSdlButton(button);
            SDL_PushEvent(ref e);
        }

        public static void OnMouseMoved(int mouseId, float x, float y)
        {
            SDL_Event e = default;
            e.motion.type = SDL_EventType.SDL_EVENT_MOUSE_MOTION;
            e.motion.timestamp = GetTimestampAsNanoseconds();
            e.motion.which = (uint)mouseId;
            e.motion.x = x;
            e.motion.y = y;
            SDL_PushEvent(ref e);
        }

        public static void OnMouseWheelRolled(int mouseId, float x, float y)
        {
            SDL_Event e = default;
            e.wheel.type = SDL_EventType.SDL_EVENT_MOUSE_WHEEL;
            e.wheel.timestamp = GetTimestampAsNanoseconds();
            e.wheel.which = (uint)mouseId;
            e.wheel.x = x;
            e.wheel.y = y;
            SDL_PushEvent(ref e);
        }

        private static ulong GetTimestampAsNanoseconds()
        {
            return (ulong)Stopwatch.GetTimestamp() * TimeSpan.NanosecondsPerTick;
        }

        private static byte MouseButtonToSdlButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return 1;
                case MouseButton.Middle:
                    return 2;
                case MouseButton.Right:
                    return 3;
                case MouseButton.X1:
                    return 4;
                case MouseButton.X2:
                    return 5;
                default:
                    return 0;
            }
        }
    }
}
