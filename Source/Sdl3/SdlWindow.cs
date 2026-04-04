using System;
using System.Diagnostics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    public sealed class SdlWindow : IDisposable
    {
        private nint _handle;
        private bool _disposed;

        public uint Id { get; }

        public bool IsCloseRequested { get; private set; }

        public SdlWindow(string title, int width, int height, SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_RESIZABLE)
        {
            _handle = SDL_CreateWindow(title, width, height, flags);
            
            if (_handle == nint.Zero)
                throw new Exception("SDL_CreateWindow failed.");

            Id = SDL_GetWindowID(_handle);
        }
        
        internal void OnWindowEvent(in SDL_WindowEvent e)
        {
            if (e.type == SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED)
                IsCloseRequested = true;
        }

        public void Dispose()
        {
            if (_disposed) return;
            SDL_DestroyWindow(_handle);
            _handle = nint.Zero;
            _disposed = true;
        }
    }
}