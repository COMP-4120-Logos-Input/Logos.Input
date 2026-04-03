using System;

namespace Logos.Input
{
    /// <summary>
    /// Provides data for mouse button events sent by mouse devices.
    /// </summary>
    public class MouseButtonEventArgs : InputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseButtonEventArgs"/> class that contains
        /// event data sent by the specified mouse device at the specified time, which was triggered
        /// by the specified mouse button.
        /// </summary>
        /// <param name="device">
        /// The mouse device that sent the input event.
        /// </param>
        /// <param name="button">
        /// The mouse button that triggered the input event.
        /// </param>
        /// <param name="timestamp">
        /// The time at which the input event occured.
        /// </param>
        public MouseButtonEventArgs(IMouseDevice device, TimeSpan timestamp, MouseButton button) : base(device, timestamp)
        {
            Button = button;
        }

        /// <summary>
        /// Gets the mouse device that sent the event data.
        /// </summary>
        /// <returns>
        /// The mouse device that sent the event data.
        /// </returns>
        public new IMouseDevice Device
        {
            get => (IMouseDevice)base.Device;
        }

        /// <summary>
        /// Gets the mouse button that triggered the input event.
        /// </summary>
        /// <returns>
        /// The mouse button that triggered the input event.
        /// </returns>
        public MouseButton Button { get; }
    }
}
