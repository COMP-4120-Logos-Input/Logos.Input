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
            var control = new KeyControlSpy();

            mapper.Bind(new KeyGesture(KeyCode.A, KeyAction.Press), control);
            mapper.RouteEvents(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(42), KeyCode.A);
            device.RaiseKeyPressed(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRepeat_triggers_handler_on_non_repeat_key_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            var control = new KeyControlSpy();

            mapper.Bind(new KeyGesture(KeyCode.B, KeyAction.Repeat), control);
            mapper.RouteEvents(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(99), KeyCode.B);
            device.RaiseKeyRepeated(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void BindKeyRelease_triggers_handler_on_key_release_event()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            var control = new KeyControlSpy();

            mapper.Bind(new KeyGesture(KeyCode.C, KeyAction.Release), control);
            mapper.RouteEvents(device);

            var input = new KeyEventArgs(null!, TimeSpan.FromTicks(123), KeyCode.C);
            device.RaiseKeyReleased(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Disconnect_stops_receiving_key_events()
        {
            var mapper = new KeyboardMapper();
            var device = new FakeKeyboardListener();
            var control = new KeyControlSpy();

            mapper.Bind(new KeyGesture(KeyCode.D, KeyAction.Press), control);
            mapper.RouteEvents(device);
            mapper.BlockEvents(device);

            device.RaiseKeyPressed(new KeyEventArgs(null!, TimeSpan.FromTicks(1), KeyCode.D));

            Assert.That(control.WasTriggered, Is.False);
        }

#pragma warning disable CS0067
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
#pragma warning restore CS0067

        // Simple test helper that remembers if a key event reached it.
        private sealed class KeyControlSpy : KeyControl<bool>
        {
            public KeyControlSpy()
            {
                State = false;
            }

            public bool WasTriggered => State;

            public KeyEventArgs? LastEventArgs { get; private set; }

            public override void OnKeyPressed(object? sender, KeyEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }

            public override void OnKeyRepeated(object? sender, KeyEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }

            public override void OnKeyReleased(object? sender, KeyEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }
        }
    }
}
