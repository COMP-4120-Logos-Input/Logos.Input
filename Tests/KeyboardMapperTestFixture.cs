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

            /* FIXME: Several lines in the tests will not compile without some modifications. For
             *        this one, I suggest creating a KeyGesture that associates the A key with the
             *        key press action, and assigning the gesture to a child of the KeyControl<bool>
             *        class through the mapper's Bind() method. When overriding the KeyControl<bool>
             *        subclass' OnKeyPressed() method, you can set its State property to true by
             *        passing the value to the OnStateChanged() method. You can then check its State
             *        property via an assert to see if it was set through the event. You can imagine
             *        that you'd have to do something similar for each of the broken tests. Feel
             *        free to remove comments like this when you get the tests to pass. - Roberto
             * 
             * mapper.BindKeyPress(KeyCode.A, (_, args) => captured = args);
             */
            mapper.RouteEvents(device);

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

            /* FIXME: Please refer to my comment within the BindKeyPress_triggers_handler_on_repeat_key_event()
             *        method under the KeyboardMapperTestFixture class for guidance on how to fix
             *        this test. - Roberto
             *
             * mapper.BindKeyRepeat(KeyCode.B, (_, args) => captured = args);
             */
            mapper.RouteEvents(device);

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

            /* FIXME: Please refer to my comment within the BindKeyPress_triggers_handler_on_repeat_key_event()
             *        method under the KeyboardMapperTestFixture class for guidance on how to fix
             *        this test. - Roberto
             *
             * mapper.BindKeyRelease(KeyCode.C, (_, args) => captured = args);
             */
            mapper.RouteEvents(device);

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

            /* FIXME: Please refer to my comment within the BindKeyPress_triggers_handler_on_repeat_key_event()
             *        method under the KeyboardMapperTestFixture class for guidance on how to fix
             *        this test. - Roberto
             *
             * mapper.BindKeyPress(KeyCode.D, (_, _) => called = true);
             */

            mapper.RouteEvents(device);
            mapper.BlockEvents(device);

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
