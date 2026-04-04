using System;

namespace Logos.Input
{
    /// <summary>
    /// Exposes methods that query the state of an input control and notify event handlers when its
    /// state changes.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the state associated with the <see cref="IInputControl{T}"/>.
    /// </typeparam>
    public interface IInputControl<out T>
    {
        /// <summary>
        /// Gets the state of the <see cref="IInputControl{T}"/>.
        /// </summary>
        /// <returns>
        /// The state of the <see cref="IInputControl{T}"/>.
        /// </returns>
        T State { get; }

        /// <summary>
        /// Occurs when <see cref="State"/> is updated.
        /// </summary>
        event EventHandler<T> StateChanged;
    }
}
