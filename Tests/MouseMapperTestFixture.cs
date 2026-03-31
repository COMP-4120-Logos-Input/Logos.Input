using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace Logos.Input.Tests
{
    [TestFixture, TestOf(typeof(MouseMapper)), Category(MouseCategory)]
    public sealed class MouseMapperTestFixture : MeasurableTestFixture
    {
        [Test, Category(MouseCategory)]
        public void BindButtonPress_triggers_handler_on_button_press()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseDevice();
            MouseButtonEventArgs? captured = null;

            mapper.BindButtonPress(MouseButton.Left, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseButtonEventArgs(MouseButton.Left, timestamp: 7);
            device.RaiseButtonPressed(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindButtonRelease_triggers_handler_on_button_release()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseDevice();
            MouseButtonEventArgs? captured = null;

            mapper.BindButtonRelease(MouseButton.Right, (_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseButtonEventArgs(MouseButton.Right, timestamp: 13);
            device.RaiseButtonReleased(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindCursorMotion_triggers_handler_on_cursor_move()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseDevice();
            MouseCursorEventArgs? captured = null;

            mapper.BindCursorMotion((_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseCursorEventArgs(new Vector2(10.0f, 20.0f), timestamp: 21);
            device.RaiseCursorMoved(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void BindWheelRotation_triggers_handler_on_wheel_roll()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseDevice();
            MouseWheelEventArgs? captured = null;

            mapper.BindWheelRotation((_, args) => captured = args);
            mapper.Connect(device);

            var input = new MouseWheelEventArgs(new Vector2(1.0f, -1.0f), timestamp: 31);
            device.RaiseWheelRolled(input);

            Assert.That(captured, Is.EqualTo(input));
        }

        [Test, Category(MouseCategory)]
        public void UnbindCursorMotion_stops_receiving_cursor_events()
        {
            var mapper = new MouseMapper();
            var device = new FakeMouseDevice();
            bool called = false;

            mapper.BindCursorMotion((_, _) => called = true);
            mapper.Connect(device);
            mapper.UnbindCursorMotion();

            device.RaiseCursorMoved(new MouseCursorEventArgs(new Vector2(5.0f, 6.0f), timestamp: 2));

            Assert.That(called, Is.False);
        }

        private sealed class FakeMouseDevice : IMouseDevice
        {
            public bool IsConnected { get; init; } = true;

            public Vector2 CursorPosition { get; private set; }

            public Vector2 WheelRotation { get; private set; }

            public event EventHandler<MouseButtonEventArgs>? ButtonPressed;

            public event EventHandler<MouseButtonEventArgs>? ButtonReleased;

            public event EventHandler<MouseWheelEventArgs>? WheelRolled;

            public event EventHandler<MouseCursorEventArgs>? CursorMoved;

            public bool IsButtonPressed(MouseButton button) => false;

            public IEnumerable<MouseButton> PressedButtons => Array.Empty<MouseButton>();

            public void RaiseButtonPressed(MouseButtonEventArgs args) => ButtonPressed?.Invoke(this, args);

            public void RaiseButtonReleased(MouseButtonEventArgs args) => ButtonReleased?.Invoke(this, args);

            public void RaiseWheelRolled(MouseWheelEventArgs args) => WheelRolled?.Invoke(this, args);

            public void RaiseCursorMoved(MouseCursorEventArgs args) => CursorMoved?.Invoke(this, args);
        }
    }
}
