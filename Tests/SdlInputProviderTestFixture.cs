using System.Numerics;
using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    public sealed class SdlInputProviderTestFixture : MeasurableTestFixture
    {
        // SDL treats 0 as an invalid keyboard ID, which is perfect for our fake keyboard events.
        private const int FakeKeyboardId = 0;
        private const int FakeMouseId = 42;

        [Test]
        public void Constructor_InitializesWithNoConnectedDevices()
        {
            SdlInputProvider provider = new SdlInputProvider();
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKeyboardConnected_RaisesDeviceConnectedAndTracksDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnKeyboardConnected(FakeKeyboardId);
            InputEventArgs deviceConnectedArgs = default;

            provider.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.AssignableTo<IKeyboardDevice>());
                Assert.That(deviceConnectedArgs.Device.IsConnected);
                Assert.That(provider.ConnectedDevices, Does.Contain(deviceConnectedArgs.Device));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKeyboardDisconnected_RaisesDeviceDisconnectedAndRemovesDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnKeyboardConnected(FakeKeyboardId);
            EventQueueMarshal.OnKeyboardDisconnected(FakeKeyboardId);
            InputEventArgs deviceConnectedArgs = default;
            InputEventArgs deviceDisconnectedArgs = default;

            provider.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.DeviceDisconnected += (_, args) => deviceDisconnectedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceDisconnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.AssignableTo<IKeyboardDevice>());
                Assert.That(deviceDisconnectedArgs.Device, Is.SameAs(deviceConnectedArgs.Device));
                Assert.That(deviceDisconnectedArgs.Device.IsConnected, Is.False);
                Assert.That(deviceDisconnectedArgs.Timestamp, Is.GreaterThan(deviceConnectedArgs.Timestamp));
                Assert.That(provider.ConnectedDevices, Is.Empty);
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKnownKeyboardReceivesKeyPressed_RaisesKeyboardAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeKeyboard(out IKeyboardDevice keyboard);
            EventQueueMarshal.OnKeyPressed(FakeKeyboardId, KeyCode.A, isRepeat: true);
            InputEventArgs deviceUpdatedArgs = default;
            KeyboardEventArgs keyPressedArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            keyboard.KeyPressed += (_, args) => keyPressedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(keyboard));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(keyPressedArgs.Timestamp));
                Assert.That(keyPressedArgs.Key, Is.EqualTo(KeyCode.A));
                Assert.That(keyPressedArgs.IsRepeat);
                Assert.That(keyboard.IsKeyPressed(KeyCode.A));
                Assert.That(keyboard.PressedKeys, Does.Contain(KeyCode.A));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenKnownKeyboardReceivesKeyReleased_RaisesKeyboardAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeKeyboard(out IKeyboardDevice keyboard);
            EventQueueMarshal.OnKeyPressed(FakeKeyboardId, KeyCode.B, isRepeat: false);
            EventQueueMarshal.OnKeyReleased(FakeKeyboardId, KeyCode.B);
            InputEventArgs deviceUpdatedArgs = default;
            KeyboardEventArgs keyReleasedArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            keyboard.KeyReleased += (_, args) => keyReleasedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(keyboard));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(keyReleasedArgs.Timestamp));
                Assert.That(keyReleasedArgs.Key, Is.EqualTo(KeyCode.B));
                Assert.That(keyReleasedArgs.IsRepeat, Is.False);
                Assert.That(keyboard.IsKeyPressed(KeyCode.B), Is.False);
                Assert.That(keyboard.PressedKeys, Does.Not.Contain(KeyCode.B));
            }
        }

        [Test, Category(KeyboardCategory)]
        public void Update_WhenUnknownKeyboardReceivesKeyEvent_DoesNotRaiseDeviceUpdated()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnKeyPressed(FakeKeyboardId, KeyCode.C, isRepeat: false);

            provider.DeviceUpdated += (_, _) => Assert.Fail();
            provider.Update();
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseConnected_RaisesDeviceConnectedAndTracksDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnMouseConnected(FakeMouseId);
            InputEventArgs deviceConnectedArgs = default;

            provider.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceConnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceConnectedArgs.Device, Is.AssignableTo<IMouseDevice>());
                Assert.That(deviceConnectedArgs.Device.IsConnected);
                Assert.That(provider.ConnectedDevices, Does.Contain(deviceConnectedArgs.Device));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseDisconnected_RaisesDeviceDisconnectedAndRemovesDevice()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnMouseConnected(FakeMouseId);
            EventQueueMarshal.OnMouseDisconnected(FakeMouseId);
            InputEventArgs deviceConnectedArgs = default;
            InputEventArgs deviceDisconnectedArgs = default;

            provider.DeviceConnected += (_, args) => deviceConnectedArgs = args;
            provider.DeviceDisconnected += (_, args) => deviceDisconnectedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceDisconnectedArgs.Device, Is.Not.Null);
                Assert.That(deviceDisconnectedArgs.Device, Is.AssignableTo<IMouseDevice>());
                Assert.That(deviceDisconnectedArgs.Device, Is.SameAs(deviceConnectedArgs.Device));
                Assert.That(deviceDisconnectedArgs.Device.IsConnected, Is.False);
                Assert.That(deviceDisconnectedArgs.Timestamp, Is.GreaterThan(deviceConnectedArgs.Timestamp));
                Assert.That(provider.ConnectedDevices, Is.Empty);
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesButtonPressed_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            EventQueueMarshal.OnMouseButtonPressed(FakeMouseId, MouseButton.Left);
            InputEventArgs deviceUpdatedArgs = default;
            MouseButtonEventArgs buttonPressedArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            mouse.ButtonPressed += (_, args) => buttonPressedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(mouse));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(buttonPressedArgs.Timestamp));
                Assert.That(buttonPressedArgs.Button, Is.EqualTo(MouseButton.Left));
                Assert.That(mouse.IsButtonPressed(MouseButton.Left));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesButtonReleased_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            EventQueueMarshal.OnMouseButtonPressed(FakeMouseId, MouseButton.Right);
            EventQueueMarshal.OnMouseButtonReleased(FakeMouseId, MouseButton.Right);
            InputEventArgs deviceUpdatedArgs = default;
            MouseButtonEventArgs buttonReleasedArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            mouse.ButtonReleased += (_, args) => buttonReleasedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(mouse));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(buttonReleasedArgs.Timestamp));
                Assert.That(buttonReleasedArgs.Button, Is.EqualTo(MouseButton.Right));
                Assert.That(mouse.IsButtonPressed(MouseButton.Right), Is.False);
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesCursorMoved_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            Vector2 position = new Vector2(123.5f, 456.25f);
            EventQueueMarshal.OnMouseMoved(FakeMouseId, position.X, position.Y);
            InputEventArgs deviceUpdatedArgs = default;
            MouseCursorEventArgs cursorMovedArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            mouse.CursorMoved += (_, args) => cursorMovedArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(mouse));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(cursorMovedArgs.Timestamp));
                Assert.That(cursorMovedArgs.Position, Is.EqualTo(position));
                Assert.That(mouse.CursorPosition, Is.EqualTo(position));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenKnownMouseReceivesWheelRolled_RaisesMouseAndProviderEvents()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);
            Vector2 rotation = new Vector2(-2.0f, 1.5f);
            EventQueueMarshal.OnMouseWheelRolled(FakeMouseId, rotation.X, rotation.Y);
            InputEventArgs deviceUpdatedArgs = default;
            MouseWheelEventArgs wheelRolledArgs = default;

            provider.DeviceUpdated += (_, args) => deviceUpdatedArgs = args;
            mouse.WheelRolled += (_, args) => wheelRolledArgs = args;
            provider.Update();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deviceUpdatedArgs.Device, Is.SameAs(mouse));
                Assert.That(deviceUpdatedArgs.Timestamp, Is.EqualTo(wheelRolledArgs.Timestamp));
                Assert.That(wheelRolledArgs.Rotation, Is.EqualTo(rotation));
                Assert.That(mouse.WheelRotation, Is.EqualTo(rotation));
            }
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenUnknownMouseReceivesButtonEvent_DoesNotRaiseDeviceUpdated()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnMouseButtonPressed(FakeMouseId, MouseButton.Middle);

            provider.DeviceUpdated += (_, _) => Assert.Fail();
            provider.Update();
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseReceivesMultipleWheelEvents_UsesLatestWheelRotation()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);

            EventQueueMarshal.OnMouseWheelRolled(FakeMouseId, 6.0f, 7.0f);
            EventQueueMarshal.OnMouseWheelRolled(FakeMouseId, 7.0f, 6.0f);

            provider.Update();
            Assert.That(mouse.WheelRotation, Is.EqualTo(new Vector2(7.0f, 6.0f)));
        }

        [Test, Category(MouseCategory)]
        public void Update_WhenMouseReceivesMultipleMoveEvents_UsesLatestCursorPosition()
        {
            SdlInputProvider provider = SetUpFakeMouse(out IMouseDevice mouse);

            EventQueueMarshal.OnMouseMoved(FakeMouseId, 60.0f, 70.0f);
            EventQueueMarshal.OnMouseMoved(FakeMouseId, 70.0f, 60.0f);

            provider.Update();
            Assert.That(mouse.CursorPosition, Is.EqualTo(new Vector2(70.0f, 60.0f)));
        }

        private static SdlInputProvider SetUpFakeKeyboard(out IKeyboardDevice keyboard)
        {
            SdlInputProvider provider = new SdlInputProvider();
            IInputDevice? device = null;
            EventQueueMarshal.OnKeyboardConnected(FakeKeyboardId);

            provider.DeviceConnected += (_, args) => device = args.Device;
            provider.Update();
            keyboard = (device as IKeyboardDevice)!;

            if (keyboard == null)
            {
                throw new AssertionException("Expected a keyboard connection event.");
            }

            return provider;
        }

        private static SdlInputProvider SetUpFakeMouse(out IMouseDevice mouse)
        {
            SdlInputProvider provider = new SdlInputProvider();
            IInputDevice? device = null;
            EventQueueMarshal.OnMouseConnected(FakeMouseId);

            provider.DeviceConnected += (_, args) => device = args.Device;
            provider.Update();
            mouse = (device as IMouseDevice)!;

            if (mouse == null)
            {
                throw new AssertionException("Expected a mouse connection event.");
            }

            return provider;
        }
    }
}
