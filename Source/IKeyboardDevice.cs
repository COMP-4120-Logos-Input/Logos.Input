using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public interface IKeyboardDevice : IInputDevice
    {
        public HashSet<KeyCode> PressedKeys { get; }
        event EventHandler<KeyboardEventArgs> KeyPressed;

        event EventHandler<KeyboardEventArgs> KeyReleased;

        bool IsKeyDown(KeyCode key);
    }
}
