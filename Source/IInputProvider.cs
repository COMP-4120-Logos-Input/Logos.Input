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
        IEnumerable<IInputDevice> ConnectedDevices { get; }

        /// <summary>
        /// Occurs when a new input device is connected.
        /// </summary>
        event EventHandler<InputEventArgs> DeviceConnected;

        /// <summary>
        /// Occurs when an input device is disconnected.
        /// </summary>
        event EventHandler<InputEventArgs> DeviceDisconnected;

        /// <summary>
        /// Occurs when the state of an input device is updated.
        /// </summary>
        event EventHandler<InputEventArgs> DeviceUpdated;

        /// <summary>
        /// Reads input from connected input devices and triggers events related to them.
        /// </summary>
        void Update();
    }
}
