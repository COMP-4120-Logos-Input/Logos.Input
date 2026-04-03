using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Logos.Input.Tests
{
    [TestFixture, TestOf(typeof(KeyboardMapper))]
    public sealed class KeyboardMapperTestFixture : MeasurableTestFixture
    {
        [Test, Category(KeyboardCategory)]
        public void BindKeyPress_triggers_handler_on_repeat_key_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardDevice();
            KeyEventArgs? captured = null;

            mapper.BindKeyPress(KeyCode.A, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(KeyCode.A, isRepeat: true, timestamp: 42);
            device.RaiseKeyPressed(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRepeat_triggers_handler_on_non_repeat_key_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardDevice();
            KeyEventArgs? captured = null;

            mapper.BindKeyRepeat(KeyCode.B, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(KeyCode.B, isRepeat: false, timestamp: 99);
            device.RaiseKeyPressed(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRelease_triggers_handler_on_key_release_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardDevice();
            KeyEventArgs? captured = null;

            mapper.BindKeyRelease(KeyCode.C, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(KeyCode.C, isRepeat: false, timestamp: 123);
            device.RaiseKeyReleased(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void Disconnect_stops_receiving_key_events()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardDevice();
            bool called = false;

            mapper.BindKeyPress(KeyCode.D, (_, _) => called = true);
            mapper.Connect(device);
            mapper.Disconnect(device);

            device.RaiseKeyPressed(new KeyEventArgs(KeyCode.D, isRepeat: true, timestamp: 1));

            Assert.That(called, Is.False);
        }

        private sealed class FakeKeyboardDevice : IKeyboardDevice
        {
            public bool IsConnected { get; init; } = true;

            public event EventHandler<KeyEventArgs>? KeyPressed;

            public event EventHandler<KeyEventArgs>? KeyReleased;

            public IEnumerable<KeyCode> PressedKeys => Array.Empty<KeyCode>();

            public bool IsKeyPressed(KeyCode key) => false;

            public void RaiseKeyPressed(KeyEventArgs args) => KeyPressed?.Invoke(this, args);

            public void RaiseKeyReleased(KeyEventArgs args) => KeyReleased?.Invoke(this, args);
        }
    }
}
