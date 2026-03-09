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
            Assert.That(provider.InputDevices, Is.Empty);
        }
    }
}
