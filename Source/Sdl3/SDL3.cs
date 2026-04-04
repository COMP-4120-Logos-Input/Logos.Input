using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    internal static partial class SDL3
    {
        static SDL3()
        {
            // Quits SDL when the parent process is about to die.
        }

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void SDL_Quit();

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_Init(SDL_InitFlags flags);

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_PollEvent(out SDL_Event e);

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_PushEvent(ref SDL_Event e);
        
        [LibraryImport(nameof(SDL3), StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nint SDL_CreateWindow(string title, int w, int h, SDL_WindowFlags flags);

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void SDL_DestroyWindow(nint window);

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial uint SDL_GetWindowID(nint window);
    }
}
