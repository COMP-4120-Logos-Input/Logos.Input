using System;

namespace Logos.Input
{
    public class MouseButtonEventArgs : InputEventArgs
    {
        public MouseButtonEventArgs(IMouseDevice device, MouseButton button, TimeSpan timestamp) : base(device, timestamp)
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
