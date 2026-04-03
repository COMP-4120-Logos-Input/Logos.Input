using System;
using System.Numerics;

namespace Logos.Input
{
    public interface IMouseDevice : IInputDevice
    {
        Vector2 CursorPosition { get; }

        Vector2 WheelRotation { get; }

        event EventHandler<MouseButtonEventArgs> ButtonPressed;

        event EventHandler<MouseButtonEventArgs> ButtonReleased;

        event EventHandler<MouseWheelEventArgs> WheelRolled;

        event EventHandler<MouseMotionEventArgs> CursorMoved;

        bool IsButtonPressed(MouseButton button);
    }
}
