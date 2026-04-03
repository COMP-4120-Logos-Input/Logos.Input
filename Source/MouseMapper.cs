using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public class MouseMapper : IInputMapper<IMouseDevice>
    {
        private readonly Dictionary<MouseButtonGesture, EventHandler<MouseButtonEventArgs>> _buttonBindings;
        private EventHandler<MouseMotionEventArgs>? _cursorBinding;
        private EventHandler<MouseWheelEventArgs>? _wheelBinding;

        public MouseMapper()
        {
            _buttonBindings = new Dictionary<MouseButtonGesture, EventHandler<MouseButtonEventArgs>>();
        }

        public void BindButtonPress(MouseButton button, EventHandler<MouseButtonEventArgs> handler)
        {
            BindEventHandler(button, MouseButtonEventType.Press, handler);
        }

        public void BindButtonRelease(MouseButton button, EventHandler<MouseButtonEventArgs> handler)
        {
            BindEventHandler(button, MouseButtonEventType.Release, handler);
        }

        public void BindCursorMotion(EventHandler<MouseMotionEventArgs> handler)
        {
            _cursorBinding += handler;
        }

        public void BindWheelRotation(EventHandler<MouseWheelEventArgs> handler)
        {
            _wheelBinding += handler;
        }

        public void UnbindButtonPress(MouseButton button)
        {
            UnbindEventHandler(button, MouseButtonEventType.Press);
        }

        public void UnbindButtonRelease(MouseButton button)
        {
            UnbindEventHandler(button, MouseButtonEventType.Release);
        }

        public void UnbindCursorMotion()
        {
            _cursorBinding = null;
        }

        public void UnbindWheelRotation()
        {
            _wheelBinding = null;
        }

        public void Connect(IMouseDevice device)
        {
            ArgumentNullException.ThrowIfNull(device);
            device.ButtonPressed += OnButtonPressed;
            device.ButtonReleased += OnButtonReleased;
            device.CursorMoved += OnCursorMoved;
            device.WheelRolled += OnWheelRolled;
        }

        public void Disconnect(IMouseDevice device)
        {
            ArgumentNullException.ThrowIfNull(device);
            device.ButtonPressed -= OnButtonPressed;
            device.ButtonReleased -= OnButtonReleased;
            device.CursorMoved -= OnCursorMoved;
            device.WheelRolled -= OnWheelRolled;
        }

        private void BindEventHandler(MouseButton button, MouseButtonEventType type, EventHandler<MouseButtonEventArgs> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            MouseButtonGesture gesture = new MouseButtonGesture(button, type);

            if (_buttonBindings.TryGetValue(gesture, out EventHandler<MouseButtonEventArgs>? value))
            {
                // Append the handler to the end of the call list if an existing handler was found.
                value += handler;
            }
            else
            {
                // Otherwise, add the handler to the dictionary.
                value = handler;
            }

            _buttonBindings[gesture] = value;
        }

        private void UnbindEventHandler(MouseButton button, MouseButtonEventType type)
        {
            _buttonBindings.Remove(new MouseButtonGesture(button, type));
        }

        private void OnButtonPressed(object? sender, MouseButtonEventArgs args)
        {
            MouseButtonGesture gesture = new MouseButtonGesture(args.Button, MouseButtonEventType.Press);

            if (_buttonBindings.TryGetValue(gesture, out EventHandler<MouseButtonEventArgs>? handler))
            {
                handler(sender, args);
            }
        }

        private void OnButtonReleased(object? sender, MouseButtonEventArgs args)
        {
            MouseButtonGesture gesture = new MouseButtonGesture(args.Button, MouseButtonEventType.Release);

            if (_buttonBindings.TryGetValue(gesture, out EventHandler<MouseButtonEventArgs>? handler))
            {
                handler(sender, args);
            }
        }

        private void OnCursorMoved(object? sender, MouseMotionEventArgs args)
        {
            _cursorBinding?.Invoke(sender, args);
        }

        private void OnWheelRolled(object? sender, MouseWheelEventArgs args)
        {
            _wheelBinding?.Invoke(sender, args);
        }

        private readonly record struct MouseButtonGesture
        {
            public readonly MouseButton Button;
            public readonly MouseButtonEventType Type;

            public MouseButtonGesture(MouseButton key, MouseButtonEventType type)
            {
                Button = key;
                Type = type;
            }
        }

        private enum MouseButtonEventType
        {
            Press,
            Release
        }
    }
}
