using System;
using System.Collections.Generic;
using System.Numerics;

namespace Logos.Input.Tests
{
    [TestFixture, TestOf(typeof(MouseMapper))]
    public sealed class MouseMapperTestFixture : MeasurableTestFixture
    {
        [Test, Category(MouseCategory)]
        public void BindButtonPress_triggers_handler_on_button_press()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseListener();
            MouseButtonEventArgs? captured = null;

            mapper.BindButtonPress(MouseButton.Left, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseButtonEventArgs(null!, TimeSpan.FromTicks(7), MouseButton.Left);
            device.RaiseButtonPressed(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindButtonRelease_triggers_handler_on_button_release()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseListener();
            MouseButtonEventArgs? captured = null;

            mapper.BindButtonRelease(MouseButton.Right, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseButtonEventArgs(null!, TimeSpan.FromTicks(13), MouseButton.Right);
            device.RaiseButtonReleased(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindCursorMotion_triggers_handler_on_cursor_move()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseListener();
            MouseMotionEventArgs? captured = null;

            mapper.BindMouseMove((_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseMotionEventArgs(null!, TimeSpan.FromTicks(21), new Vector2(10.0f, 20.0f));
            device.RaiseMouseMoved(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindWheelRotation_triggers_handler_on_wheel_roll()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseListener();
            MouseWheelEventArgs? captured = null;

            mapper.BindWheelMove((_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseWheelEventArgs(null!, TimeSpan.FromTicks(31), new Vector2(1.0f, -1.0f));
            device.RaiseWheelMoved(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void UnbindCursorMotion_stops_receiving_cursor_events()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseListener();
            bool called = false;

            mapper.BindMouseMove((_, _) => called = true);
            mapper.Connect(device);
            mapper.UnbindCursorMotion();

            device.RaiseMouseMoved(new MouseMotionEventArgs(null!, TimeSpan.FromTicks(2), new Vector2(5.0f, 6.0f)));

            Assert.That(called, Is.False);
        }

        private sealed class FakeMouseListener : IMouseListener
        {
            public IEnumerable<IMouseDevice> ConnectedDevices
            {
                get => null!;
            }

            IEnumerable<IInputDevice> IInputListener.ConnectedDevices
            {
                get => null!;
            }

            public event EventHandler<InputEventArgs>? DeviceConnected;

            public event EventHandler<InputEventArgs>? DeviceDisconnected;

            public event EventHandler<MouseButtonEventArgs>? ButtonPressed;

            public event EventHandler<MouseButtonEventArgs>? ButtonReleased;

            public event EventHandler<MouseMotionEventArgs>? MouseMoved;

            public event EventHandler<MouseWheelEventArgs>? WheelMoved;

            public void RaiseButtonPressed(MouseButtonEventArgs args)
            {
                ButtonPressed?.Invoke(this, args);
            }

            public void RaiseButtonReleased(MouseButtonEventArgs args)
            {
                ButtonReleased?.Invoke(this, args);
            }

            public void RaiseWheelMoved(MouseWheelEventArgs args)
            {
                WheelMoved?.Invoke(this, args);
            }

            public void RaiseMouseMoved(MouseMotionEventArgs args)
            {
                MouseMoved?.Invoke(this, args);
            }
        }
    }
}
