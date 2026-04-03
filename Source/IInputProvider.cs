using System;
using System.Collections.Generic;

namespace Logos.Input
{
    /// <summary>
    /// Defines methods that expose input listeners and dispatch input events.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Returns a collection of supported input listeners.
        /// </summary>
        /// <returns>
        /// A collection of supported input listeners.
        /// </returns>
        IEnumerable<IInputListener> Listeners { get; }

        /// <summary>
        /// Returns an input listener of type <typeparamref name="T"/> that exposes events for a
        /// specific kind of input device.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the input listener to return.
        /// </typeparam>
        /// <returns>
        /// An input listener of type <typeparamref name="T"/> that exposes events for a specific
        /// kind of input device.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The <see cref="IInputProvider"/> does not contain an input listener of type
        /// <typeparamref name="T"/>.
        /// </exception>
        T GetListener<T>() where T : IInputListener;

        /// <summary>
        /// Processes data from connected input devices and dispatches their emitted events.
        /// </summary>
        void DispatchEvents();
    }
}
