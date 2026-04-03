using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public class KeyboardMapper : IInputMapper
    {
        private readonly Dictionary<KeyboardGesture, EventHandler<KeyEventArgs>> _bindings;

        public KeyboardMapper()
        {
            _bindings = new Dictionary<KeyboardGesture, EventHandler<KeyEventArgs>>();
        }

        public void BindKeyPress(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEvent(key, handler, KeyboardEventType.Press);
        }

        public void BindKeyRepeat(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEvent(key, handler, KeyboardEventType.Repeat);
        }

        public void BindKeyRelease(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEvent(key, handler, KeyboardEventType.Release);
        }

        public void UnbindKeyPress(KeyCode key)
        {
            UnbindEvent(key, KeyboardEventType.Press);
        }

        public void UnbindKeyRepeat(KeyCode key)
        {
            UnbindEvent(key, KeyboardEventType.Repeat);
        }

        public void UnbindKeyRelease(KeyCode key)
        {
            UnbindEvent(key, KeyboardEventType.Release);
        }

        public void Connect(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            RouteEvents(provider.GetListener<IKeyboardListener>());
        }

        public void Connect(IKeyboardListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            RouteEvents(listener);
        }

        public void Disconnect(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            BlockEvents(provider.GetListener<IKeyboardListener>());
        }

        public void Disconnect(IKeyboardListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            BlockEvents(listener);
        }

        private void BindEvent(KeyCode key, EventHandler<KeyEventArgs> handler, KeyboardEventType type)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _bindings[new KeyboardGesture(key, type)] = handler;
        }

        private void UnbindEvent(KeyCode key, KeyboardEventType type)
        {
            _bindings.Remove(new KeyboardGesture(key, type));
        }

        private void RouteEvents(IKeyboardListener listener)
        {
            listener.KeyPressed += OnKeyPressed;
            listener.KeyRepeated += OnKeyRepeated;
            listener.KeyReleased += OnKeyReleased;
        }

        private void BlockEvents(IKeyboardListener listener)
        {
            listener.KeyPressed -= OnKeyPressed;
            listener.KeyRepeated -= OnKeyRepeated;
            listener.KeyReleased -= OnKeyReleased;
        }

        private void OnKeyPressed(object? sender, KeyEventArgs args)
        {
            InvokeKeyEventHandler(sender, args, KeyboardEventType.Press);
        }

        private void OnKeyRepeated(object? sender, KeyEventArgs args)
        {
            InvokeKeyEventHandler(sender, args, KeyboardEventType.Repeat);
        }

        private void OnKeyReleased(object? sender, KeyEventArgs args)
        {
            InvokeKeyEventHandler(sender, args, KeyboardEventType.Release);
        }

        private void InvokeKeyEventHandler(object? sender, KeyEventArgs args, KeyboardEventType type)
        {
            KeyboardGesture gesture = new KeyboardGesture(args.Key, type);

            if (_bindings.TryGetValue(gesture, out EventHandler<KeyEventArgs>? handler))
            {
                handler(sender, args);
            }
        }

        private readonly record struct KeyboardGesture
        {
            public readonly KeyCode Key;
            public readonly KeyboardEventType Type;

            public KeyboardGesture(KeyCode key, KeyboardEventType type)
            {
                Key = key;
                Type = type;
            }
        }

        private enum KeyboardEventType
        {
            Press,
            Repeat,
            Release
        }
    }
}
