using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Logos.Input.Sdl3
{
    internal static partial class Sdl
    {
        private const string LibraryName = "SDL3";

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_InitSubSystem(uint flags);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void SDL_Quit();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool SDL_PollEvent(out SDL_Event e);
    }
}
