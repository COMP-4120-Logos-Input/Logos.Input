using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    public static class SdlInputProviderTestFixture
    {
        [Test]
        public static void ConstructorTest()
        {
            SdlInputProvider provider = new SdlInputProvider();
            Assert.That(provider.ConnectedDevices, Is.Empty);
        }

        [Test]
        public static void DeviceConnectedTest()
        {
            SdlInputProvider provider = new SdlInputProvider();
            EventQueueMarshal.PushKeyboardConnectedEvent(uint.MaxValue);
            provider.DeviceConnected += (_, args) =>
            {
                Assert.That(provider.ConnectedDevices, Has.Member(args.Device));
                Assert.Pass();
            };
            provider.Update();
            Assert.Fail();
        }
    }
}
