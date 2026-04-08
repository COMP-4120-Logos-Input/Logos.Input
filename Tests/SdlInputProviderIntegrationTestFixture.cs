using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Logos.Input.Sdl3;

namespace Logos.Input.Tests
{
    [TestFixture(TestOf = typeof(SdlInputProvider))]
    [Category("Integration")]
    [Explicit("Manual SDL integration tests requiring native SDL3 and real hardware interaction.")]
    public sealed class SdlInputProviderIntegrationTestFixture
    {
        [Test]
        public void NativeRuntime_CanConstructProviderAndPollWithoutThrowing()
        {
            Assume.That(HasVendoredNativeSdl(), Is.True, "Requires a vendored native SDL3 binary for this platform.");

            SdlInputProvider provider = new SdlInputProvider();

            Assert.DoesNotThrow(provider.DispatchEvents);
        }

        [Test]
        public void Update_WhenExternalKeyboardIsConnected_RaisesDeviceConnected()
        {
            Assume.That(HasVendoredNativeSdl(), Is.True, "Requires a vendored native SDL3 binary for this platform.");

            SdlInputProvider provider = new SdlInputProvider();
            IKeyboardListener listener = provider.GetListener<IKeyboardListener>()!;
            bool connected = false;

            listener.DeviceConnected += (_, _) => connected = true;

            TestContext.Progress.WriteLine("Connect an external keyboard within 10 seconds.");

            bool observed = WaitForEvent(
                timeout: TimeSpan.FromSeconds(10),
                onPoll: provider.DispatchEvents,
                isSatisfied: () => connected);

            Assert.That(observed, Is.True, "No keyboard connection event was observed within the timeout.");
        }

        private static bool WaitForEvent(TimeSpan timeout, Action onPoll, Func<bool> isSatisfied)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                onPoll();
                if (isSatisfied())
                {
                    return true;
                }

                Thread.Sleep(16);
            }

            return false;
        }

        private static bool HasVendoredNativeSdl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RuntimeInformation.OSArchitecture == Architecture.Arm64;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RuntimeInformation.OSArchitecture == Architecture.X64;
            }

            return false;
        }
    }
}
