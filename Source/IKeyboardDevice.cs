using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public interface IKeyboardDevice : IInputDevice
    {
        IEnumerable<KeyCode> PressedKeys { get; }

        event EventHandler<KeyEventArgs> KeyPressed;

        event EventHandler<KeyEventArgs> KeyReleased;

        bool IsKeyPressed(KeyCode key);
    }
}
