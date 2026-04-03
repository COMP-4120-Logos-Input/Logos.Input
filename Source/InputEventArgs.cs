using System;

namespace Logos.Input
{
    /// <summary>
    /// Represents a base class for classes that contain event data emitted by connected input
    /// devices.
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputEventArgs"/> class that contains event
        /// data sent by the specified input device at the specified time.
        /// </summary>
        /// <param name="device">
        /// The input device that triggered the input event.
        /// </param>
        /// <param name="timestamp">
        /// The time at which the input event occured.
        /// </param>
        public InputEventArgs(IInputDevice device, TimeSpan timestamp)
        {
            Device = device;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets the input device that triggered the input event.
        /// </summary>
        /// <returns>
        /// The input device that triggered the input event.
        /// </returns>
        public IInputDevice Device { get; }

        /// <summary>
        /// Gets the time at which the input event occured.
        /// </summary>
        /// <returns>
        /// The time at which the input event occured.
        /// </returns>
        public TimeSpan Timestamp { get; }
    }
}
