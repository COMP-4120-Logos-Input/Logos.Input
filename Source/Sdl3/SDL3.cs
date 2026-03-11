using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    internal static partial class SDL3
    {
        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_Init(uint flags);

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void SDL_Quit();

        [LibraryImport(nameof(SDL3))]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_PollEvent(out SDL_Event e);
    }
}
