using System;
using System.Collections.Generic;

namespace Logos.Input.Tests
{
    [TestFixture, TestOf(typeof(KeyboardMapper))]
    public sealed class KeyboardMapperTestFixture : MeasurableTestFixture
    {
        [Test, Category(KeyboardCategory)]
        public void BindKeyPress_triggers_handler_on_repeat_key_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            KeyEventArgs? captured = null;

            mapper.BindKeyPress(KeyCode.A, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(42), KeyCode.A);
            device.RaiseKeyPressed(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRepeat_triggers_handler_on_non_repeat_key_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            KeyEventArgs? captured = null;

            mapper.BindKeyRepeat(KeyCode.B, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(99), KeyCode.B);
            device.RaiseKeyRepeated(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRelease_triggers_handler_on_key_release_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            KeyEventArgs? captured = null;

            mapper.BindKeyRelease(KeyCode.C, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(123), KeyCode.C);
            device.RaiseKeyReleased(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(KeyboardCategory)]
        public void Disconnect_stops_receiving_key_events()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            bool called = false;

            mapper.BindKeyPress(KeyCode.D, (_, _) => called = true);
            mapper.Connect(device);
            mapper.Disconnect(device);

            device.RaiseKeyPressed(new KeyEventArgs(null!, TimeSpan.FromTicks(1), KeyCode.D));

            Assert.That(called, Is.False);
        }

        private sealed class FakeKeyboardListener : IKeyboardListener
        {
            public IEnumerable<IKeyboardDevice> ConnectedDevices
            {
                get => null!;
            }

            IEnumerable<IInputDevice> IInputListener.ConnectedDevices
            {
                get => null!;
            }

            public event EventHandler<InputEventArgs>? DeviceConnected;

            public event EventHandler<InputEventArgs>? DeviceDisconnected;

            public event EventHandler<KeyEventArgs>? KeyPressed;

            public event EventHandler<KeyEventArgs>? KeyRepeated;

            public event EventHandler<KeyEventArgs>? KeyReleased;

            public void RaiseKeyPressed(KeyEventArgs args)
            {
                KeyPressed?.Invoke(this, args);
            }

            public void RaiseKeyRepeated(KeyEventArgs args)
            {
                KeyRepeated?.Invoke(this, args);
            }

            public void RaiseKeyReleased(KeyEventArgs args)
            {
                KeyReleased?.Invoke(this, args);
            }
        }
    }
}
