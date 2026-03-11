namespace Logos.Input.Sdl3
{
    internal enum SDL_EventType : uint
    {
        SDL_EVENT_FIRST = 0,
        SDL_EVENT_QUIT = 0x100,
        SDL_EVENT_KEY_DOWN = 0x300,
        SDL_EVENT_KEY_UP,
        SDL_EVENT_KEYMAP_CHANGED,
        SDL_EVENT_KEYBOARD_ADDED,
        SDL_EVENT_KEYBOARD_REMOVED
    }
}
