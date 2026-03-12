using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Logos.Input.Sdl3.SDL3;

namespace Logos.Input.Sdl3
{
    internal interface ISdlRuntime
    {
        void Initialize();

        bool PollEvent(out SDL_Event e);
    }

    public sealed class SdlInputProvider : IInputProvider
    {
        private const SDL_InitFlags InitFlags = SDL_InitFlags.SDL_INIT_EVENTS | SDL_InitFlags.SDL_INIT_GAMEPAD;

        private sealed class NativeSdlRuntime : ISdlRuntime
        {
            private static readonly Lazy<bool> s_isInitialized = new(() => SDL_Init(InitFlags));

            public static NativeSdlRuntime Instance { get; } = new();

            private NativeSdlRuntime()
            {
            }

            public void Initialize()
            {
                Debug.Assert(s_isInitialized.Value, "SDL3 somehow failed upon initialization.");
            }

            public bool PollEvent(out SDL_Event e)
            {
                return SDL_PollEvent(out e);
            }
        }

        private readonly Dictionary<uint, KeyboardDevice> _keyboards;
        private readonly ISdlRuntime _runtime;

        public SdlInputProvider()
            : this(NativeSdlRuntime.Instance)
        {
        }

        internal SdlInputProvider(ISdlRuntime runtime)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _runtime.Initialize();
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
            while (_runtime.PollEvent(out SDL_Event e))
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
                if (_pressedKeys.Add(args.Key))
                {
                    KeyPressed?.Invoke(this, args);
                }
            }

            public void OnKeyReleased(KeyboardEventArgs args)
            {
                if (_pressedKeys.Remove(args.Key))
                {
                    KeyReleased?.Invoke(this, args);
                }
            }
        }
    }
}
