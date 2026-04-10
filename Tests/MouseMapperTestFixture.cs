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
            var listener = new FakeMouseListener();
            var mapper = new MouseMapper(listener);
            var control = new MouseButtonControlSpy();

            mapper.Bind(new MouseButtonGesture(MouseButton.Left, MouseButtonAction.Press), control);
            mapper.IsEnabled = true;

            var input = new MouseButtonEventArgs(null!, TimeSpan.FromTicks(7), MouseButton.Left);
            listener.RaiseButtonPressed(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(MouseCategory)]
        public void BindButtonRelease_triggers_handler_on_button_release()
        {
            var listener = new FakeMouseListener();
            var mapper = new MouseMapper(listener);
            var control = new MouseButtonControlSpy();

            mapper.Bind(new MouseButtonGesture(MouseButton.Right, MouseButtonAction.Release), control);
            mapper.IsEnabled = true;

            var input = new MouseButtonEventArgs(null!, TimeSpan.FromTicks(13), MouseButton.Right);
            listener.RaiseButtonReleased(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(MouseCategory)]
        public void BindCursorMotion_triggers_handler_on_cursor_move()
        {
            var listener = new FakeMouseListener();
            var mapper = new MouseMapper(listener);
            var control = new MouseMotionControlSpy();

            mapper.Bind(MouseMotionDirection.Any, control);
            mapper.IsEnabled = true;

            var input = new MouseMotionEventArgs(null!, TimeSpan.FromTicks(21), new Vector2(10.0f, 20.0f));
            listener.RaiseMouseMoved(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(MouseCategory)]
        public void BindWheelRotation_triggers_handler_on_wheel_roll()
        {
            var listener = new FakeMouseListener();
            var mapper = new MouseMapper(listener);
            var control = new MouseWheelControlSpy();

            mapper.Bind(MouseWheelDirection.Any, control);
            mapper.IsEnabled = true;

            var input = new MouseWheelEventArgs(null!, TimeSpan.FromTicks(31), new Vector2(1.0f, -1.0f));
            listener.RaiseWheelMoved(input);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(control.WasTriggered, Is.True);
                Assert.That(control.LastEventArgs, Is.EqualTo(input));
            }
        }

        [Test, Category(MouseCategory)]
        public void UnbindCursorMotion_stops_receiving_cursor_events()
        {
            var listener = new FakeMouseListener();
            var mapper = new MouseMapper(listener);
            var control = new MouseMotionControlSpy();

            mapper.Bind(MouseMotionDirection.Any, control);
            mapper.IsEnabled = true;
            mapper.IsEnabled = false;

            listener.RaiseMouseMoved(new MouseMotionEventArgs(null!, TimeSpan.FromTicks(2), new Vector2(5.0f, 6.0f)));

            Assert.That(control.WasTriggered, Is.False);
        }

#pragma warning disable CS0067
        private sealed class FakeMouseListener : IMouseListener
        {
            public IEnumerable<IMouseDevice> Devices
            {
                get => null!;
            }

            IEnumerable<IInputDevice> IInputListener.Devices
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
#pragma warning restore CS0067

        // Simple test helpers that remember if a mouse event reached them.
        private sealed class MouseButtonControlSpy : MouseButtonControl<bool>
        {
            public MouseButtonControlSpy()
            {
                State = false;
            }

            public bool WasTriggered => State;

            public MouseButtonEventArgs? LastEventArgs { get; private set; }

            public override void OnButtonPressed(object? sender, MouseButtonEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }

            public override void OnButtonReleased(object? sender, MouseButtonEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }
        }

        private sealed class MouseMotionControlSpy : MouseMotionControl<bool>
        {
            public MouseMotionControlSpy()
            {
                State = false;
            }

            public bool WasTriggered => State;

            public MouseMotionEventArgs? LastEventArgs { get; private set; }

            public override void OnMouseMoved(object? sender, MouseMotionEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }
        }

        private sealed class MouseWheelControlSpy : MouseWheelControl<bool>
        {
            public MouseWheelControlSpy()
            {
                State = false;
            }

            public bool WasTriggered => State;

            public MouseWheelEventArgs? LastEventArgs { get; private set; }

            public override void OnWheelMoved(object? sender, MouseWheelEventArgs e)
            {
                LastEventArgs = e;
                State = true;
            }
        }
    }
}
