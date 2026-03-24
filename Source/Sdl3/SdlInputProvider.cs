using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    public sealed class SdlInputProvider : IInputProvider
    {
        private readonly Dictionary<uint, KeyboardDevice> _keyboards;

        static SdlInputProvider()
        {
            if (!SDL_Init(SDL_InitFlags.SDL_INIT_GAMEPAD))
            {
                // It is considered a catastrophic failure if SDL cannot initialize input events.
                throw new Exception("SDL3 somehow failed upon initialization.");
            }
        }

        public SdlInputProvider()
        {
            _keyboards = new Dictionary<uint, KeyboardDevice>();
        }

        public IEnumerable<IInputDevice> ConnectedDevices
        {
            get => _keyboards.Values;
        }

        public event EventHandler<InputEventArgs>? DeviceConnected;

        public event EventHandler<InputEventArgs>? DeviceDisconnected;

        public event EventHandler<InputEventArgs>? DeviceUpdated;

        public void Update()
        {
            while (SDL_PollEvent(out SDL_Event e))
            {
                long timestamp = (long)(e.common.timestamp / TimeSpan.NanosecondsPerTick);

                switch (e.type)
                {
                    case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
                    {
                        KeyboardDevice keyboard = new KeyboardDevice();
                        _keyboards.Add(e.kdevice.which, keyboard);
                        DeviceConnected?.Invoke(this, new InputEventArgs(keyboard, timestamp));
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
                    {
                        if (_keyboards.Remove(e.kdevice.which, out KeyboardDevice? keyboard))
                        {
                            keyboard.IsConnected = false;
                            DeviceDisconnected?.Invoke(this, new InputEventArgs(keyboard, timestamp));
                        }

                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEY_DOWN:
                    {
                        if (_keyboards.TryGetValue(e.key.which, out KeyboardDevice? keyboard))
                        {
                            keyboard.OnKeyPressed(new KeyboardEventArgs((KeyCode)e.key.scancode, e.key.repeat != 0, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(keyboard, timestamp));
                        }

                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEY_UP:
                    {
                        if (_keyboards.TryGetValue(e.key.which, out KeyboardDevice? keyboard))
                        {
                            keyboard.OnKeyReleased(new KeyboardEventArgs((KeyCode)e.key.scancode, false, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(keyboard, timestamp));
                        }

                        continue;
                    }
                    default:
                        continue;
                }
            }
        }

        private sealed class KeyboardDevice : IKeyboardDevice
        {
            private HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();

            public bool IsConnected { get; set; } = true;

            public IEnumerable<KeyCode> PressedKeys
            {
                get => _pressedKeys;
            }

            public event EventHandler<KeyboardEventArgs>? KeyPressed;

            public event EventHandler<KeyboardEventArgs>? KeyReleased;

            public bool IsKeyPressed(KeyCode key)
            {
                return _pressedKeys.Contains(key);
            }

            public void OnKeyPressed(KeyboardEventArgs args)
            {
                _pressedKeys.Add(args.Key);
                KeyPressed?.Invoke(this, args);
            }

            public void OnKeyReleased(KeyboardEventArgs args)
            {
                _pressedKeys.Remove(args.Key);
                KeyReleased?.Invoke(this, args);
            }
        }

        private sealed class MouseDevice : IMouseDevice
        {
            public bool IsConnected { get; set; } = true;

            private HashSet<MouseButton> _pressedButtons = new HashSet<MouseButton>();

            public IEnumerable<MouseButton> PressedButtons
            {
                get => _pressedButtons;
            }

            private Vector2 _cursorPosition = new Vector2(0, 0);
            private Vector2 _wheelRotation = new Vector2(0, 0);
            
            public Vector2 WheelRotation { get; }
            public event EventHandler<MouseButtonEventArgs>? ButtonPressed;
            public event EventHandler<MouseButtonEventArgs>? ButtonReleased;
            public event EventHandler<MouseWheelEventArgs>? WheelRolled;
            public event EventHandler<MouseCursorEventArgs>? CursorMoved;
            public bool IsButtonPressed(MouseButton button)
            {
                return _pressedButtons.Contains(button);
            }

            public void OnButtonPressed(MouseButtonEventArgs args)
            {
                _pressedButtons.Add(args.Button);
                ButtonPressed?.Invoke(this, args);
            }

            public void OnButtonReleased(MouseButtonEventArgs args)
            {
                _pressedButtons.Remove(args.Button);
                ButtonReleased?.Invoke(this, args);
            }

            public void OnWheelRolled(MouseWheelEventArgs args)
            {
                _wheelRotation = args.Rotation;
                WheelRolled?.Invoke(this, args);
            }

            public void OnCursorMoved(MouseCursorEventArgs args)
            {
                _cursorPosition = args.Position;
                CursorMoved?.Invoke(this, args);
            }

            Vector2 IMouseDevice.CursorPosition { get; }
        }
    }
}
