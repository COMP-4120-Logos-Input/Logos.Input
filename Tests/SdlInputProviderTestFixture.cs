using System;
using System.Collections.Generic;
using System.Linq;
using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    public sealed class SdlInputProviderTestFixture
    {
        private const uint KeyboardId = 1234;

        [Test]
        public void Constructor_InitializesWithNoConnectedDevices()
        {
            SdlInputProvider provider = new SdlInputProvider(new FakeSdlRuntime());

            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test]
        public void Update_WhenKeyboardConnected_RaisesDeviceConnectedAndTracksDevice()
        {
            FakeSdlRuntime runtime = new FakeSdlRuntime();
            SdlInputProvider provider = new SdlInputProvider(runtime);
            InputEventArgs? eventArgs = null;

            provider.DeviceConnected += (_, args) => eventArgs = args;

            runtime.Push(CreateKeyboardConnectedEvent(KeyboardId));
            provider.Update();

            Assert.That(eventArgs.HasValue, Is.True);
            Assert.That(eventArgs.Value.Device, Is.AssignableTo<IKeyboardDevice>());
            Assert.That(eventArgs.Value.Device.IsConnected, Is.True);
            Assert.That(eventArgs.Value.Timestamp, Is.GreaterThan(0));
            Assert.That(provider.ConnectedDevices.Single(), Is.SameAs(eventArgs.Value.Device));
        }

        [Test]
        public void Update_WhenKeyboardDisconnected_RaisesDeviceDisconnectedAndRemovesDevice()
        {
            FakeSdlRuntime runtime = new FakeSdlRuntime();
            SdlInputProvider provider = new SdlInputProvider(runtime);
            IInputDevice? connectedDevice = null;
            InputEventArgs? disconnectedArgs = null;

            provider.DeviceConnected += (_, args) => connectedDevice = args.Device;
            runtime.Push(CreateKeyboardConnectedEvent(KeyboardId));
            provider.Update();

            provider.DeviceDisconnected += (_, args) => disconnectedArgs = args;
            runtime.Push(CreateKeyboardDisconnectedEvent(KeyboardId));
            provider.Update();

            Assert.That(connectedDevice, Is.Not.Null);
            Assert.That(disconnectedArgs.HasValue, Is.True);
            Assert.That(disconnectedArgs.Value.Device, Is.SameAs(connectedDevice));
            Assert.That(disconnectedArgs.Value.Device.IsConnected, Is.False);
            Assert.That(disconnectedArgs.Value.Timestamp, Is.GreaterThan(0));
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test]
        public void Update_WhenKnownKeyboardReceivesKeyPressed_RaisesKeyboardAndProviderEvents()
        {
            FakeSdlRuntime runtime = new FakeSdlRuntime();
            SdlInputProvider provider = new SdlInputProvider(runtime);
            IKeyboardDevice keyboard = ConnectKeyboard(provider, runtime);
            KeyboardEventArgs? keyPressedArgs = null;
            InputEventArgs? deviceUpdatedArgs = null;

            keyboard.KeyPressed += (_, args) => keyPressedArgs = args;
            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;

            runtime.Push(CreateKeyPressedEvent(KeyboardId, KeyCode.A, isRepeat: true));
            provider.Update();

            Assert.That(keyPressedArgs.HasValue, Is.True);
            Assert.That(keyPressedArgs.Value.Key, Is.EqualTo(KeyCode.A));
            Assert.That(keyPressedArgs.Value.IsRepeat, Is.True);
            Assert.That(keyPressedArgs.Value.Timestamp, Is.GreaterThan(0));

            Assert.That(deviceUpdatedArgs.HasValue, Is.True);
            Assert.That(deviceUpdatedArgs.Value.Device, Is.SameAs(keyboard));
            Assert.That(deviceUpdatedArgs.Value.Timestamp, Is.GreaterThan(0));
        }

        [Test]
        public void Update_WhenKnownKeyboardReceivesKeyReleased_RaisesKeyboardAndProviderEvents()
        {
            FakeSdlRuntime runtime = new FakeSdlRuntime();
            SdlInputProvider provider = new SdlInputProvider(runtime);
            IKeyboardDevice keyboard = ConnectKeyboard(provider, runtime);
            KeyboardEventArgs? keyReleasedArgs = null;
            InputEventArgs? deviceUpdatedArgs = null;

            keyboard.KeyReleased += (_, args) => keyReleasedArgs = args;
            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;

            runtime.Push(CreateKeyReleasedEvent(KeyboardId, KeyCode.B));
            provider.Update();

            Assert.That(keyReleasedArgs.HasValue, Is.True);
            Assert.That(keyReleasedArgs.Value.Key, Is.EqualTo(KeyCode.B));
            Assert.That(keyReleasedArgs.Value.IsRepeat, Is.False);
            Assert.That(keyReleasedArgs.Value.Timestamp, Is.GreaterThan(0));

            Assert.That(deviceUpdatedArgs.HasValue, Is.True);
            Assert.That(deviceUpdatedArgs.Value.Device, Is.SameAs(keyboard));
            Assert.That(deviceUpdatedArgs.Value.Timestamp, Is.GreaterThan(0));
        }

        [Test]
        public void Update_WhenUnknownKeyboardReceivesKeyEvent_DoesNotRaiseDeviceUpdated()
        {
            FakeSdlRuntime runtime = new FakeSdlRuntime();
            SdlInputProvider provider = new SdlInputProvider(runtime);
            int deviceUpdatedCount = 0;

            provider.DeviceUpdated += (_, _) => deviceUpdatedCount++;

            runtime.Push(CreateKeyPressedEvent(KeyboardId, KeyCode.C, isRepeat: false));
            provider.Update();

            Assert.That(deviceUpdatedCount, Is.Zero);
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        private static IKeyboardDevice ConnectKeyboard(SdlInputProvider provider, FakeSdlRuntime runtime)
        {
            IKeyboardDevice? keyboard = null;
            provider.DeviceConnected += (_, args) => keyboard = (IKeyboardDevice)args.Device;

            runtime.Push(CreateKeyboardConnectedEvent(KeyboardId));
            provider.Update();

            return keyboard ?? throw new AssertionException("Expected a keyboard connection event.");
        }

        private static SDL_Event CreateKeyboardConnectedEvent(uint keyboardId)
        {
            SDL_Event e = default;
            e.kdevice.type = SDL_EventType.SDL_EVENT_KEYBOARD_ADDED;
            e.kdevice.timestamp = GetTimestamp();
            e.kdevice.which = keyboardId;
            return e;
        }

        private static SDL_Event CreateKeyboardDisconnectedEvent(uint keyboardId)
        {
            SDL_Event e = default;
            e.kdevice.type = SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED;
            e.kdevice.timestamp = GetTimestamp();
            e.kdevice.which = keyboardId;
            return e;
        }

        private static SDL_Event CreateKeyPressedEvent(uint keyboardId, KeyCode key, bool isRepeat)
        {
            SDL_Event e = default;
            e.key.type = SDL_EventType.SDL_EVENT_KEY_DOWN;
            e.key.timestamp = GetTimestamp();
            e.key.which = keyboardId;
            e.key.scancode = (SDL_Scancode)key;
            e.key.down = 1;
            e.key.repeat = (byte)(isRepeat ? 1 : 0);
            return e;
        }

        private static SDL_Event CreateKeyReleasedEvent(uint keyboardId, KeyCode key)
        {
            SDL_Event e = default;
            e.key.type = SDL_EventType.SDL_EVENT_KEY_UP;
            e.key.timestamp = GetTimestamp();
            e.key.which = keyboardId;
            e.key.scancode = (SDL_Scancode)key;
            return e;
        }

        private static ulong GetTimestamp()
        {
            return (ulong)(TimeSpan.TicksPerMillisecond * 100 * 1_000);
        }

        private sealed class FakeSdlRuntime : ISdlRuntime
        {
            private readonly Queue<SDL_Event> _events = new();

            public void Initialize()
            {
            }

            public bool PollEvent(out SDL_Event e)
            {
                if (_events.Count > 0)
                {
                    e = _events.Dequeue();
                    return true;
                }

                e = default;
                return false;
            }

            public void Push(SDL_Event e)
            {
                _events.Enqueue(e);
            }
        }
    }
}
