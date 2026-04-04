using System;
using System.Collections.Generic;
using System.Numerics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    public sealed class SdlInputProvider : IInputProvider, IDisposable
    {
        private readonly ObservableKeyboardDeviceCollection _keyboards;
        private readonly ObservableMouseDeviceCollection _mice;
        private readonly Dictionary<uint, SdlWindow> _windows;
        
        public void RegisterWindow(SdlWindow window) => _windows[window.Id] = window;
        public void UnregisterWindow(SdlWindow window) => _windows.Remove(window.Id);

        static SdlInputProvider()
        {
            if (!SDL_Init(SDL_InitFlags.SDL_INIT_GAMEPAD | SDL_InitFlags.SDL_INIT_VIDEO))
            {
                // It is considered a catastrophic failure if SDL cannot initialize input events.
                throw new Exception("SDL3 somehow failed upon initialization.");
            }
        }

        public SdlInputProvider()
        {
            _keyboards = new ObservableKeyboardDeviceCollection();
            _mice = new ObservableMouseDeviceCollection();
            _windows = new Dictionary<uint, SdlWindow>();
        }

        public IEnumerable<IInputListener> Listeners
        {
            get
            {
                yield return _keyboards;
                yield return _mice;
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            SDL_Quit();

        }
        
        public T GetListener<T>() where T : IInputListener
        {
            if (_keyboards is T keyboardListener)
            {
                return keyboardListener;
            }

            if (_mice is T mouseListener)
            {
                return mouseListener;
            }

            throw new NotSupportedException(
                "The SdlInputProvider does not contain the specified input listener.");
        }

        public void DispatchEvents()
        {
            while (SDL_PollEvent(out SDL_Event e))
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
                        _keyboards.OnKeyboardAdded(in e.kdevice);
                        continue;
                    case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
                        _keyboards.OnKeyboardRemoved(in e.kdevice);
                        continue;
                    case SDL_EventType.SDL_EVENT_KEY_DOWN:
                        _keyboards.OnKeyDown(in e.key);
                        continue;
                    case SDL_EventType.SDL_EVENT_KEY_UP:
                        _keyboards.OnKeyUp(in e.key);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_ADDED:
                        _mice.OnMouseAdded(in e.mdevice);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_REMOVED:
                        _mice.OnMouseRemoved(in e.mdevice);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
                        _mice.OnMouseButtonDown(in e.button);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                        _mice.OnMouseButtonUp(in e.button);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                        _mice.OnMouseMotion(in e.motion);
                        continue;
                    case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                        _mice.OnMouseWheel(in e.wheel);
                        continue;
                    case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                        if (_windows.TryGetValue(e.window.windowID, out SdlWindow? w))
                            w.OnWindowEvent(in e.window);
                        continue;
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
            private readonly HashSet<KeyCode> _pressedKeys;

            public KeyboardDevice()
            {
                _pressedKeys = new HashSet<KeyCode>();
            }

            public bool IsConnected { get; set; }

            public IEnumerable<KeyCode> PressedKeys
            {
                get => _pressedKeys;
            }

            public bool IsKeyPressed(KeyCode key)
            {
                return _pressedKeys.Contains(key);
            }

            public void OnKeyDown(in SDL_KeyboardEvent e)
            {
                _pressedKeys.Add((KeyCode)e.scancode);
            }

            public void OnKeyUp(in SDL_KeyboardEvent e)
            {
                _pressedKeys.Remove((KeyCode)e.scancode);
            }
        }

        private sealed class MouseDevice : IMouseDevice
        {
            private readonly HashSet<MouseButton> _pressedButtons;

            public MouseDevice()
            {
                _pressedButtons = new HashSet<MouseButton>();
            }

            public bool IsConnected { get; set; }

            public Vector2 Position { get; private set; }

            public Vector2 ScrollWheel { get; private set; }

            public IEnumerable<MouseButton> PressedButtons
            {
                get => _pressedButtons;
            }

            public bool IsButtonPressed(MouseButton button)
            {
                return _pressedButtons.Contains(button);
            }

            public void OnMouseButtonDown(in SDL_MouseButtonEvent e)
            {
                _pressedButtons.Add((MouseButton)e.button);
            }

            public void OnMouseButtonUp(in SDL_MouseButtonEvent e)
            {
                _pressedButtons.Remove((MouseButton)e.button);
            }

            public void OnMouseMotion(in SDL_MouseMotionEvent e)
            {
                Position = new Vector2(e.x, e.y);
            }

            public void OnMouseWheel(in SDL_MouseWheelEvent e)
            {
                ScrollWheel = new Vector2(e.x, e.y);
            }
        }

        private sealed class ObservableKeyboardDeviceCollection : Dictionary<uint, KeyboardDevice>, IKeyboardListener
        {
            public IEnumerable<IKeyboardDevice> ConnectedDevices
            {
                get => Values;
            }

            IEnumerable<IInputDevice> IInputListener.ConnectedDevices
            {
                get => Values;
            }

            public event EventHandler<InputEventArgs>? DeviceConnected;

            public event EventHandler<InputEventArgs>? DeviceDisconnected;

            public event EventHandler<KeyEventArgs>? KeyPressed;

            public event EventHandler<KeyEventArgs>? KeyRepeated;

            public event EventHandler<KeyEventArgs>? KeyReleased;

            public void OnKeyboardAdded(ref readonly SDL_KeyboardDeviceEvent e)
            {
                KeyboardDevice device = new KeyboardDevice();

                if (TryAdd(e.which, device))
                {
                    device.IsConnected = true;
                    DeviceConnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnKeyboardRemoved(ref readonly SDL_KeyboardDeviceEvent e)
            {
                if (Remove(e.which, out KeyboardDevice? device))
                {
                    device.IsConnected = false;
                    DeviceDisconnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnKeyDown(ref readonly SDL_KeyboardEvent e)
            {
                if (!TryGetValue(e.which, out KeyboardDevice? device))
                {
                    device = new KeyboardDevice { IsConnected = true };
                    Add(e.which, device);
                    DeviceConnected?.Invoke(this, new InputEventArgs(device, ToTimeSpan(e.timestamp)));
                }

                device.OnKeyDown(in e);
                EventHandler<KeyEventArgs>? handler = e.repeat == 0 ? KeyPressed : KeyRepeated;
                handler?.Invoke(this, CreateEventArgs(device, in e));
            }

            public void OnKeyUp(ref readonly SDL_KeyboardEvent e)
            {
                if (!TryGetValue(e.which, out KeyboardDevice? device))
                {
                    device = new KeyboardDevice { IsConnected = true };
                    Add(e.which, device);
                    DeviceConnected?.Invoke(this, new InputEventArgs(device, ToTimeSpan(e.timestamp)));
                }

                device.OnKeyUp(in e);
                KeyReleased?.Invoke(this, CreateEventArgs(device, in e));
            }

            private static InputEventArgs CreateEventArgs(KeyboardDevice device, ref readonly SDL_KeyboardDeviceEvent e)
            {
                return new InputEventArgs(device, ToTimeSpan(e.timestamp));
            }

            private static KeyEventArgs CreateEventArgs(KeyboardDevice device, ref readonly SDL_KeyboardEvent e)
            {
                return new KeyEventArgs(device, ToTimeSpan(e.timestamp), (KeyCode)e.scancode);
            }
        }

        private sealed class ObservableMouseDeviceCollection : Dictionary<uint, MouseDevice>, IMouseListener
        {
            public IEnumerable<IMouseDevice> ConnectedDevices
            {
                get => Values;
            }

            IEnumerable<IInputDevice> IInputListener.ConnectedDevices
            {
                get => Values;
            }

            public event EventHandler<InputEventArgs>? DeviceConnected;
            
            public event EventHandler<InputEventArgs>? DeviceDisconnected;

            public event EventHandler<MouseButtonEventArgs>? ButtonPressed;
            
            public event EventHandler<MouseButtonEventArgs>? ButtonReleased;
            
            public event EventHandler<MouseMotionEventArgs>? MouseMoved;
            
            public event EventHandler<MouseWheelEventArgs>? WheelMoved;

            public void OnMouseAdded(ref readonly SDL_MouseDeviceEvent e)
            {
                MouseDevice device = new MouseDevice();

                if (TryAdd(e.which, device))
                {
                    device.IsConnected = true;
                    DeviceConnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnMouseRemoved(ref readonly SDL_MouseDeviceEvent e)
            {
                if (Remove(e.which, out MouseDevice? device))
                {
                    device.IsConnected = false;
                    DeviceDisconnected?.Invoke(this, CreateEventArgs(device, in e));
                }
            }

            public void OnMouseButtonDown(ref readonly SDL_MouseButtonEvent e)
            {
                if (!TryGetValue(e.which, out MouseDevice? mouse))
                {
                    mouse = new MouseDevice { IsConnected = true };
                    Add(e.which, mouse);
                    DeviceConnected?.Invoke(this, new InputEventArgs(mouse, ToTimeSpan(e.timestamp)));
                }

                mouse.OnMouseButtonDown(in e);
                ButtonPressed?.Invoke(this, CreateEventArgs(mouse, in e));
            }

            public void OnMouseButtonUp(ref readonly SDL_MouseButtonEvent e)
            {
                if (!TryGetValue(e.which, out MouseDevice? mouse))
                {
                    mouse = new MouseDevice { IsConnected = true };
                    Add(e.which, mouse);
                    DeviceConnected?.Invoke(this, new InputEventArgs(mouse, ToTimeSpan(e.timestamp)));
                }

                mouse.OnMouseButtonUp(in e);
                ButtonReleased?.Invoke(this, CreateEventArgs(mouse, in e));
            }

            public void OnMouseMotion(ref readonly SDL_MouseMotionEvent e)
            {
                if (!TryGetValue(e.which, out MouseDevice? mouse))
                {
                    mouse = new MouseDevice { IsConnected = true };
                    Add(e.which, mouse);
                    DeviceConnected?.Invoke(this, new InputEventArgs(mouse, ToTimeSpan(e.timestamp)));
                }

                mouse.OnMouseMotion(in e);
                MouseMoved?.Invoke(this, CreateEventArgs(mouse, in e));
            }

            public void OnMouseWheel(ref readonly SDL_MouseWheelEvent e)
            {
                if (!TryGetValue(e.which, out MouseDevice? mouse))
                {
                    mouse = new MouseDevice { IsConnected = true };
                    Add(e.which, mouse);
                    DeviceConnected?.Invoke(this, new InputEventArgs(mouse, ToTimeSpan(e.timestamp)));
                }

                mouse.OnMouseWheel(in e);
                WheelMoved?.Invoke(this, CreateEventArgs(mouse, in e));
            }

            private static InputEventArgs CreateEventArgs(MouseDevice device, ref readonly SDL_MouseDeviceEvent e)
            {
                return new InputEventArgs(device, ToTimeSpan(e.timestamp));
            }

            private static MouseButtonEventArgs CreateEventArgs(MouseDevice device, ref readonly SDL_MouseButtonEvent e)
            {
                return new MouseButtonEventArgs(device, ToTimeSpan(e.timestamp), (MouseButton)e.button);
            }

            private static MouseMotionEventArgs CreateEventArgs(MouseDevice device, ref readonly SDL_MouseMotionEvent e)
            {
                return new MouseMotionEventArgs(device, ToTimeSpan(e.timestamp), new Vector2(e.x, e.y));
            }

            private static MouseWheelEventArgs CreateEventArgs(MouseDevice device, ref readonly SDL_MouseWheelEvent e)
            {
                return new MouseWheelEventArgs(device, ToTimeSpan(e.timestamp), new Vector2(e.x, e.y));
            }
        }
    }
}
