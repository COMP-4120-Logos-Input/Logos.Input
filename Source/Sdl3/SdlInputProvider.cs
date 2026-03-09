using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Logos.Input.Sdl3
{
    public sealed class SdlInputProvider : IInputProvider
    {
        public SdlInputProvider()
        {
            bool success = Sdl.SDL_InitSubSystem(0x4000);
            Debug.Assert(success, "SDL3 somehow failed upon initialization.");
            Sdl.SDL_Quit();;
        }

        public IEnumerable<IInputDevice> InputDevices
        {
            get => Enumerable.Empty<IInputDevice>();
        }

        public event EventHandler<IInputDevice>? InputDeviceAdded;

        public event EventHandler<IInputDevice>? InputDeviceRemoved;

        public event EventHandler<IInputDevice>? InputDeviceUpdated;

        public void Update()
        {
        }
    }
}
