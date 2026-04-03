using System;
using System.Collections.Generic;

namespace Logos.Input
{
    public class KeyboardMapper : IInputMapper<IKeyboardDevice>
    {
        private readonly Dictionary<KeyboardGesture, EventHandler<KeyEventArgs>> _bindings;

        public KeyboardMapper()
        {
            _bindings = new Dictionary<KeyboardGesture, EventHandler<KeyEventArgs>>();
        }

        public void BindKeyPress(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEventHandler(key, KeyboardEventType.Press, handler);
        }

        public void BindKeyRepeat(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEventHandler(key, KeyboardEventType.Repeat, handler);
        }

        public void BindKeyRelease(KeyCode key, EventHandler<KeyEventArgs> handler)
        {
            BindEventHandler(key, KeyboardEventType.Release, handler);
        }

        public void UnbindKeyPress(KeyCode key)
        {
            UnbindEventHandler(key, KeyboardEventType.Press);
        }

        public void UnbindKeyRepeat(KeyCode key)
        {
            UnbindEventHandler(key, KeyboardEventType.Repeat);
        }

        public void UnbindKeyRelease(KeyCode key)
        {
            UnbindEventHandler(key, KeyboardEventType.Release);
        }

        public void Connect(IKeyboardDevice device)
        {
            ArgumentNullException.ThrowIfNull(device);
            device.KeyPressed += OnKeyPressed;
            device.KeyReleased += OnKeyReleased;
        }

        public void Disconnect(IKeyboardDevice device)
        {
            ArgumentNullException.ThrowIfNull(device);
            device.KeyPressed -= OnKeyPressed;
            device.KeyReleased -= OnKeyReleased;
        }

        private void BindEventHandler(KeyCode key, KeyboardEventType type, EventHandler<KeyEventArgs> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            KeyboardGesture gesture = new KeyboardGesture(key, type);

            if (_bindings.TryGetValue(gesture, out EventHandler<KeyEventArgs>? value))
            {
                // Append the handler to the end of the call list if an existing handler was found.
                value += handler;
            }
            else
            {
                // Otherwise, add the handler to the dictionary.
                value = handler;
            }

            _bindings[gesture] = value;
        }

        private void UnbindEventHandler(KeyCode key, KeyboardEventType type)
        {
            _bindings.Remove(new KeyboardGesture(key, type));
        }

        private void OnKeyPressed(object? sender, KeyEventArgs args)
        {
            KeyboardEventType type = args.IsRepeat ? KeyboardEventType.Press : KeyboardEventType.Repeat;
            KeyboardGesture gesture = new KeyboardGesture(args.Key, type);

            if (_bindings.TryGetValue(gesture, out EventHandler<KeyEventArgs>? handler))
            {
                handler(sender, args);
            }
        }

        private void OnKeyReleased(object? sender, KeyEventArgs args)
        {
            KeyboardGesture gesture = new KeyboardGesture(args.Key, KeyboardEventType.Release);

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
