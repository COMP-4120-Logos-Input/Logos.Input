using System.Linq;
using System.Numerics;
using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    public sealed class SdlInputProviderTestFixture : MeasurableTestFixture
    {
        // SDL treats 0 as an invalid device ID, which is perfect for our fake input events.
        private const int FakeDeviceId = 0;

        [Test]
        public void Constructor_InitializesWithNoConnectedDevices()
        {
            SdlInputProvider provider = new SdlInputProvider();

            foreach (IInputListener listener in provider.Listeners)
            {
                Assert.That(listener.ConnectedDevices, Is.Empty);
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKeyboardConnected_RaisesDeviceConnectedAndTracksDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyboardConnected(FakeDeviceId);
            InputEventArgs? deviceConnectedArgs = null;

            listener.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.AssignableTo<IKeyboardDevice>());
                Assert.That(deviceConnectedArgs.Device.IsConnected);
                Assert.That(listener.ConnectedDevices, Does.Contain(deviceConnectedArgs.Device));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKeyboardDisconnected_RaisesDeviceDisconnectedAndRemovesDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyboardConnected(FakeDeviceId);
            EventQueueMarshal.OnKeyboardDisconnected(FakeDeviceId);
            InputEventArgs? deviceConnectedArgs = null;
            InputEventArgs? deviceDisconnectedArgs = null;

            listener.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            listener.DeviceDisconnected += (_, args) => deviceDisconnectedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.AssignableTo<IKeyboardDevice>());
                Assert.That(deviceDisconnectedArgs.Device, Is.SameAs(deviceConnectedArgs.Device));
                Assert.That(deviceDisconnectedArgs.Device.IsConnected, Is.False);
                Assert.That(deviceDisconnectedArgs.Timestamp, Is.GreaterThan(deviceConnectedArgs.Timestamp));
                Assert.That(listener.ConnectedDevices, Is.Empty);
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKnownKeyboardReceivesKeyPressed_RaisesKeyboardAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeKeyboard(out IKeyboardDevice keyboard);
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyPressed(FakeDeviceId, KeyCode.A, isRepeat: true);
            KeyEventArgs? keyPressedArgs = null;

            listener.KeyRepeated += (_, args) => keyPressedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(keyPressedArgs, Is.Not.Null);
                Assert.That(keyPressedArgs.Device, Is.SameAs(keyboard));
                Assert.That(keyPressedArgs.Key, Is.EqualTo(KeyCode.A));
                Assert.That(keyboard.IsKeyPressed(KeyCode.A));
                Assert.That(keyboard.PressedKeys, Does.Contain(KeyCode.A));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKnownKeyboardReceivesKeyReleased_RaisesKeyboardAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeKeyboard(out IKeyboardDevice keyboard);
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyPressed(FakeDeviceId, KeyCode.B, isRepeat: false);
            EventQueueMarshal.OnKeyReleased(FakeDeviceId, KeyCode.B);
            KeyEventArgs? keyReleasedArgs = null;

            listener.KeyReleased += (_, args) => keyReleasedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(keyReleasedArgs, Is.Not.Null);
                Assert.That(keyReleasedArgs.Device, Is.SameAs(keyboard));
                Assert.That(keyReleasedArgs.Key, Is.EqualTo(KeyCode.B));
                Assert.That(keyboard.IsKeyPressed(KeyCode.B), Is.False);
                Assert.That(keyboard.PressedKeys, Does.Not.Contain(KeyCode.B));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenUnknownKeyboardReceivesKeyEvent_DoesNotRaiseDeviceUpdated()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyPressed(FakeDeviceId, KeyCode.C, isRepeat: false);

            listener.KeyPressed += (_, _) => Assert.Fail();
            provider.DispatchEvents();
            Assert.That(listener.ConnectedDevices, Is.Empty);
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseConnected_RaisesDeviceConnectedAndTracksDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseConnected(FakeDeviceId);
            InputEventArgs? deviceConnectedArgs = null;

            listener.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.AssignableTo<IMouseDevice>());
                Assert.That(deviceConnectedArgs.Device.IsConnected);
                Assert.That(listener.ConnectedDevices, Does.Contain(deviceConnectedArgs.Device));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseDisconnected_RaisesDeviceDisconnectedAndRemovesDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseConnected(FakeDeviceId);
            EventQueueMarshal.OnMouseDisconnected(FakeDeviceId);
            InputEventArgs? deviceConnectedArgs = null;
            InputEventArgs? deviceDisconnectedArgs = null;

            listener.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            listener.DeviceDisconnected += (_, args) => deviceDisconnectedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.AssignableTo<IMouseDevice>());
                Assert.That(deviceDisconnectedArgs.Device, Is.SameAs(deviceConnectedArgs.Device));
                Assert.That(deviceDisconnectedArgs.Device.IsConnected, Is.False);
                Assert.That(deviceDisconnectedArgs.Timestamp, Is.GreaterThan(deviceConnectedArgs.Timestamp));
                Assert.That(listener.ConnectedDevices, Is.Empty);
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesButtonPressed_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseButtonPressed(FakeDeviceId, MouseButton.Left);
            MouseButtonEventArgs? buttonPressedArgs = null;

            listener.ButtonPressed += (_, args) => buttonPressedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(buttonPressedArgs, Is.Not.Null);
                Assert.That(buttonPressedArgs.Device, Is.SameAs(mouse));
                Assert.That(buttonPressedArgs.Button, Is.EqualTo(MouseButton.Left));
                Assert.That(mouse.IsButtonPressed(MouseButton.Left));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesButtonReleased_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseButtonPressed(FakeDeviceId, MouseButton.Right);
            EventQueueMarshal.OnMouseButtonReleased(FakeDeviceId, MouseButton.Right);
            MouseButtonEventArgs buttonReleasedArgs = default;

            listener.ButtonReleased += (_, args) => buttonReleasedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(buttonReleasedArgs, Is.Not.Null);
                Assert.That(buttonReleasedArgs.Device, Is.SameAs(mouse));
                Assert.That(buttonReleasedArgs.Button, Is.EqualTo(MouseButton.Right));
                Assert.That(mouse.IsButtonPressed(MouseButton.Right), Is.False);
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesCursorMoved_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            IMouseListener listener = provider.GetListener<IMouseListener>();
            Vector2 position = new Vector2(123.5f, 456.25f);
            EventQueueMarshal.OnMouseMoved(FakeDeviceId, position.X, position.Y);
            MouseMotionEventArgs? cursorMovedArgs = null;

            listener.MouseMoved += (_, args) => cursorMovedArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(cursorMovedArgs, Is.Not.Null);
                Assert.That(cursorMovedArgs.Device, Is.SameAs(mouse));
                Assert.That(cursorMovedArgs.Translation, Is.EqualTo(position));
                Assert.That(mouse.Position, Is.EqualTo(position));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesWheelRolled_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            IMouseListener listener = provider.GetListener<IMouseListener>();
            Vector2 rotation = new Vector2(-2.0f, 1.5f);
            EventQueueMarshal.OnMouseWheelRolled(FakeDeviceId, rotation.X, rotation.Y);
            MouseWheelEventArgs wheelRolledArgs = default;

            listener.WheelMoved += (_, args) => wheelRolledArgs = args;
            provider.DispatchEvents();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(wheelRolledArgs, Is.Not.Null);
                Assert.That(wheelRolledArgs.Device, Is.SameAs(mouse));
                Assert.That(wheelRolledArgs.Timestamp, Is.EqualTo(wheelRolledArgs.Timestamp));
                Assert.That(wheelRolledArgs.Delta, Is.EqualTo(rotation));
                Assert.That(mouse.ScrollWheel, Is.EqualTo(rotation));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenUnknownMouseReceivesButtonEvent_DoesNotRaiseDeviceUpdated()
        {
            SdlInputProvider provider = new SdlInputProvider();
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseButtonPressed(FakeDeviceId, MouseButton.Middle);

            listener.ButtonPressed += (_, _) => Assert.Fail();
            provider.DispatchEvents();
            Assert.That(listener.ConnectedDevices, Is.Empty);
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseReceivesMultipleWheelEvents_UsesLatestWheelRotation()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);

            EventQueueMarshal.OnMouseWheelRolled(FakeDeviceId, 6.0f, 7.0f);
            EventQueueMarshal.OnMouseWheelRolled(FakeDeviceId, 7.0f, 6.0f);

            provider.DispatchEvents();
            Assert.That(mouse.ScrollWheel, Is.EqualTo(new Vector2(7.0f, 6.0f)));
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseReceivesMultipleMoveEvents_UsesLatestCursorPosition()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);

            EventQueueMarshal.OnMouseMoved(FakeDeviceId, 60.0f, 70.0f);
            EventQueueMarshal.OnMouseMoved(FakeDeviceId, 70.0f, 60.0f);

            provider.DispatchEvents();
            Assert.That(mouse.Position, Is.EqualTo(new Vector2(70.0f, 60.0f)));
        }

        private static SdlInputProvider SetUpFakeKeyboard(out IKeyboardDevice keyboard)
        {
            SdlInputProvider provider = new SdlInputProvider();
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>();
            EventQueueMarshal.OnKeyboardConnected(FakeDeviceId);

            provider.DispatchEvents();
            keyboard = listener.ConnectedDevices.FirstOrDefault()!;

            if (keyboard == null)
            {
                throw new AssertionException("Expected a keyboard connection event.");
            }

            return provider;
        }

        private static SdlInputProvider SetUpFakeMouse(out IMouseDevice mouse)
        {
            SdlInputProvider provider = new SdlInputProvider();
            IMouseListener listener = provider.GetListener<IMouseListener>();
            EventQueueMarshal.OnMouseConnected(FakeDeviceId);

            provider.DispatchEvents();
            mouse = listener.ConnectedDevices.FirstOrDefault()!;

            if (mouse == null)
            {
                throw new AssertionException("Expected a mouse connection event.");
            }

            return provider;
        }
    }
}
