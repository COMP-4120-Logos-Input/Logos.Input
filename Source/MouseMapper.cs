using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public class MouseMapper : IInputMapper
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
            BindEvent(button, handler, MouseButtonEventType.Press);
        }

        public void BindButtonRelease(MouseButton button, EventHandler<MouseButtonEventArgs> handler)
        {
            BindEvent(button, handler, MouseButtonEventType.Release);
        }

        public void BindMouseMove(EventHandler<MouseMotionEventArgs> handler)
        {
            _cursorBinding += handler;
        }

        public void BindWheelMove(EventHandler<MouseWheelEventArgs> handler)
        {
            _wheelBinding += handler;
        }

        public void UnbindButtonPress(MouseButton button)
        {
            UnbindEvent(button, MouseButtonEventType.Press);
        }

        public void UnbindButtonRelease(MouseButton button)
        {
            UnbindEvent(button, MouseButtonEventType.Release);
        }

        public void UnbindCursorMotion()
        {
            _cursorBinding = null;
        }

        public void UnbindWheelRotation()
        {
            _wheelBinding = null;
        }

        public void Connect(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            RouteEvents(provider.GetListener<IMouseListener>());
        }

        public void Connect(IMouseListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            RouteEvents(listener);
        }

        public void Disconnect(IMouseListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            BlockEvents(listener);
        }

        public void Disconnect(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            BlockEvents(provider.GetListener<IMouseListener>());
        }

        private void BindEvent(MouseButton button, EventHandler<MouseButtonEventArgs> handler, MouseButtonEventType type)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _buttonBindings[new MouseButtonGesture(button, type)] = handler;
        }

        private void UnbindEvent(MouseButton button, MouseButtonEventType type)
        {
            _buttonBindings.Remove(new MouseButtonGesture(button, type));
        }

        private void RouteEvents(IMouseListener listener)
        {
            listener.ButtonPressed += OnButtonPressed;
            listener.ButtonReleased += OnButtonReleased;
            listener.MouseMoved += OnMouseMoved;
            listener.WheelMoved += OnWheelMoved;
        }

        private void BlockEvents(IMouseListener listener)
        {
            listener.ButtonPressed -= OnButtonPressed;
            listener.ButtonReleased -= OnButtonReleased;
            listener.MouseMoved -= OnMouseMoved;
            listener.WheelMoved -= OnWheelMoved;
        }

        private void OnButtonPressed(object? sender, MouseButtonEventArgs args)
        {
            OnButtonEvent(sender, args, MouseButtonEventType.Press);
        }

        private void OnButtonReleased(object? sender, MouseButtonEventArgs args)
        {
            OnButtonEvent(sender, args, MouseButtonEventType.Release);
        }

        private void OnButtonEvent(object? sender, MouseButtonEventArgs args, MouseButtonEventType type)
        {
            MouseButtonGesture gesture = new MouseButtonGesture(args.Button, type);

            if (_buttonBindings.TryGetValue(gesture, out EventHandler<MouseButtonEventArgs>? handler))
            {
                handler(sender, args);
            }
        }

        private void OnMouseMoved(object? sender, MouseMotionEventArgs args)
        {
            _cursorBinding?.Invoke(sender, args);
        }

        private void OnWheelMoved(object? sender, MouseWheelEventArgs args)
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
