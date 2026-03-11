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
            bool success = SDL3.SDL_InitSubSystem(0x4000);
            Debug.Assert(success, "SDL3 somehow failed upon initialization.");
            SDL3.SDL_Quit();;
        }

        public IEnumerable<IInputDevice> InputDevices
        {
            get => Enumerable.Empty<IInputDevice>();
        }

        public event EventHandler<InputEventArgs>? InputDeviceAdded;

        public event EventHandler<InputEventArgs>? InputDeviceRemoved;

        public event EventHandler<InputEventArgs>? InputDeviceUpdated;

        public void Update()
        {
        }
    }
}
