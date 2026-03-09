using System;
using System.Collections.Generic;

namespace Logos.Input
{
    /// <summary>
    /// Defines methods that access connected input devices and trigger events related to them.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Returns a collection of connected input devices.
        /// </summary>
        /// <returns>
        /// A collection of connected input devices.
        /// </returns>
        IEnumerable<IInputDevice> InputDevices { get; }

        /// <summary>
        /// Occurs when a new input device is connected.
        /// </summary>
        event EventHandler<IInputDevice> InputDeviceAdded;

        /// <summary>
        /// Occurs when an input device is disconnected.
        /// </summary>
        event EventHandler<IInputDevice> InputDeviceRemoved;

        /// <summary>
        /// Occurs when the state of an input device has changed.
        /// </summary>
        event EventHandler<IInputDevice> InputDeviceUpdated;

        /// <summary>
        /// Reads input from connected input devices and triggers events related to them.
        /// </summary>
        void Update();
    }
}
