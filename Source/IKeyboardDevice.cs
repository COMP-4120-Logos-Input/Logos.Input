using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public interface IKeyboardDevice : IInputDevice
    {
        IEnumerable<KeyCode> PressedKeys { get; }

        event EventHandler<KeyboardEventArgs> KeyPressed;

        event EventHandler<KeyboardEventArgs> KeyReleased;

        bool IsKeyPressed(KeyCode key);
    }
}
