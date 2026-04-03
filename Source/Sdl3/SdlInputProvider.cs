using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    public sealed class SdlInputProvider : IInputProvider
    {
        private readonly ObservableKeyboardCollection _keyboards;
        private readonly Dictionary<uint, MouseDevice> _mice;

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
            _keyboards = new ObservableKeyboardCollection();
            _mice = new Dictionary<uint, MouseDevice>();
        }

        public IEnumerable<IInputListener> Listeners
        {
            get
            {
                yield return _keyboards;
            }
        }

        public T GetListener<T>() where T : IInputListener
        {
            if (_keyboards is not T listener)
            {
                throw new NotSupportedException(
                    "The SdlInputProvider does not contain the specified input listener.");
            }

            return listener;
        }

        public void DispatchEvents()
        {
            while (SDL_PollEvent(out SDL_Event e))
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
                    {
                        _keyboards.OnKeyboardAdded(in e.kdevice);
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
                    {
                        _keyboards.OnKeyboardRemoved(in e.kdevice);
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEY_DOWN:
                    {
                        _keyboards.OnKeyDown(in e.key);
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_KEY_UP:
                    {
                        _keyboards.OnKeyUp(in e.key);
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_MOUSE_ADDED:
                    {
                        MouseDevice mouse = new MouseDevice();
                        _mice.Add(e.mdevice.which, mouse);
                        DeviceConnected?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_MOUSE_REMOVED:
                    {
                        if (_mice.Remove(e.mdevice.which, out MouseDevice? mouse))
                        {
                            mouse.IsConnected = false;
                            DeviceDisconnected?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        }
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
                    {
                        if (_mice.TryGetValue(e.button.which, out MouseDevice? mouse))
                        {
                            MouseButton button = SDLButtonToMouseButton(e.button.button);
                            mouse.OnButtonPressed(new MouseButtonEventArgs(button, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        }
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                    {
                        if (_mice.TryGetValue(e.button.which, out MouseDevice? mouse))
                        {
                            MouseButton button = SDLButtonToMouseButton(e.button.button);
                            mouse.OnButtonReleased(new MouseButtonEventArgs(button, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        }
                        continue;
                    }
                    case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                    {
                        if (_mice.TryGetValue(e.motion.which, out MouseDevice? mouse))
                        {
                            Vector2 pos = new Vector2(e.motion.x, e.motion.y);
                            mouse.OnCursorMoved(new MouseMotionEventArgs(pos, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        }
                        continue;
                    }
                    /* THis event below doesnt handle flipped wheels which is a property of SDL_MouseWheelEvent
                        Although it should be trivial to handle it.
                     */
                    case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                    {
                        if (_mice.TryGetValue(e.wheel.which, out MouseDevice? mouse))
                        {
                            Vector2 rotation = new Vector2(e.wheel.x, e.wheel.y);
                            mouse.OnWheelRolled(new MouseWheelEventArgs(rotation, timestamp));
                            DeviceUpdated?.Invoke(this, new InputEventArgs(mouse, timestamp));
                        }
                        continue;
                    }
                    default:
                        continue;
                }
            }
        }

        private static TimeSpan ToTimeSpan(ulong timestamp)
        {
            return new TimeSpan((long)(timestamp / TimeSpan.NanosecondsPerTick));
        }

        private sealed class KeyboardDevice : IKeyboardDevice
        {
            private readonly HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();

            public bool IsConnected { get; set; } = true;

            public IEnumerable<KeyCode> PressedKeys
            {
                get => _pressedKeys;
            }

            public bool IsKeyPressed(KeyCode key)
            {
                return _pressedKeys.Contains(key);
            }

            public void OnKeyDown(SDL_Scancode key)
            {
                _pressedKeys.Add((KeyCode)key);
            }

            public void OnKeyUp(SDL_Scancode key)
            {
                _pressedKeys.Remove((KeyCode)key);
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
            
            public Vector2 WheelRotation
            {
                get => _wheelRotation;
            }
            public event EventHandler<MouseButtonEventArgs>? ButtonPressed;
            public event EventHandler<MouseButtonEventArgs>? ButtonReleased;
            public event EventHandler<MouseWheelEventArgs>? WheelRolled;
            public event EventHandler<MouseMotionEventArgs>? CursorMoved;
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
                _wheelRotation = args.Delta;
                WheelRolled?.Invoke(this, args);
            }

            public void OnCursorMoved(MouseMotionEventArgs args)
            {
                _cursorPosition = args.Translation;
                CursorMoved?.Invoke(this, args);
            }

            Vector2 IMouseDevice.CursorPosition
            {
                get => _cursorPosition;
            }
        }

        private sealed class ObservableKeyboardCollection : Dictionary<uint, KeyboardDevice>, IKeyboardListener
        {
            IEnumerable<IInputDevice> IInputListener.ConnectedDevices
            {
                get => Values;
            }

            public event EventHandler<InputEventArgs>? DeviceConnected;

            public event EventHandler<InputEventArgs>? DeviceDisconnected;

            public event EventHandler<KeyEventArgs>? KeyPressed;

            public event EventHandler<KeyEventArgs>? KeyRepeated;

            public event EventHandler<KeyEventArgs>? KeyReleased;

            public IEnumerable<IKeyboardDevice> ConnectedDevices
            {
                get => Values;
            }

            public void OnKeyboardAdded(ref readonly SDL_KeyboardDeviceEvent e)
            {
                KeyboardDevice device = new KeyboardDevice();

                if (TryAdd(e.which, device))
                {
                    DeviceConnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnKeyboardRemoved(ref readonly SDL_KeyboardDeviceEvent e)
            {
                if (Remove(e.which, out KeyboardDevice? device))
                {
                    DeviceDisconnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnKeyDown(ref readonly SDL_KeyboardEvent e)
            {
                if (TryGetValue(e.which, out KeyboardDevice? device))
                {
                    device.OnKeyDown(e.scancode);
                    EventHandler<KeyEventArgs>? handler = e.repeat == 0 ? KeyPressed : KeyReleased;
                    handler?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnKeyUp(ref readonly SDL_KeyboardEvent e)
            {
                if (TryGetValue(e.which, out KeyboardDevice? device))
                {
                    device.OnKeyUp(e.scancode);
                    KeyReleased?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            private static InputEventArgs CreateEventArgs(KeyboardDevice device, ref readonly SDL_KeyboardDeviceEvent e)
            {
                return new InputEventArgs(device, ToTimeSpan(e.timestamp));
            }

            private static KeyEventArgs CreateEventArgs(KeyboardDevice device, ref readonly SDL_KeyboardEvent e)
            {
                return new KeyEventArgs(device, ToTimeSpan(e.timestamp), (KeyCode)e.key);
            }
        }
    }
}
