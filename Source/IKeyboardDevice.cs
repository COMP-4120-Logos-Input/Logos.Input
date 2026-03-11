using System;

namespace Logos.Input
{
    public interface IKeyboardDevice : IInputDevice
    {
        event EventHandler<KeyboardEventArgs> KeyPressed;

        event EventHandler<KeyboardEventArgs> KeyReleased;

        bool IsKeyDown(KeyCode key);
    }
}
