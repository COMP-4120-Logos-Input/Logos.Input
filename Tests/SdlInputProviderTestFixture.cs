using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    public sealed class SdlInputProviderTestFixture
    {
        // SDL treats 0 as an invalid keyboard ID, which is perfect for our fake keyboard events.
        private const int FakeKeyboardId = 0;

        [Test]
        public void Constructor_InitializesWithNoConnectedDevices()
        {
            SdlInputProvider provider = new SdlInputProvider();
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
                Assert.That(keyboard.PressedKeys, Does.Not.Contain(KeyCode.A));
            }
        }

        [Test]
        public void Update_WhenUnknownKeyboardReceivesKeyEvent_DoesNotRaiseDeviceUpdated()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.OnKeyPressed(FakeKeyboardId, KeyCode.C, isRepeat: false);

            provider.DeviceUpdated += (_, _) => Assert.Fail();
            provider.Update();
            Assert.That(provider.ConnectedDevices, Is.Empty);
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
    }
}
