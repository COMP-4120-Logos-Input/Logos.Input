using System;
using System.Numerics;

namespace Logos.Input
{
    /// <summary>
    /// Provides data for mouse motion events sent by mouse devices.
    /// </summary>
    public class MouseMotionEventArgs : InputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseMotionEventArgs"/> class that contains
        /// event data sent by the specified mouse device at the specified time, which was triggered
        /// by the specified change in the mouse device's position.
        /// </summary>
        /// <param name="device">
        /// The mouse device that sent the input event.
        /// </param>
        /// <param name="timestamp">
        /// The time at which the input event occured.
        /// </param>
        /// <param name="translation">
        /// The change in the position of <paramref name="device"/>.
        /// </param>
        public MouseMotionEventArgs(IMouseDevice device, TimeSpan timestamp, Vector2 translation) : base(device, timestamp)
        {
            Translation = translation;
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
        /// The change in the position of <see cref="Device"/> when the input event occured.
        /// </summary>
        /// <returns>
        /// The change in the position of <see cref="Device"/> when the input event occured.
        /// </returns>
        public Vector2 Translation { get; }
    }
}
