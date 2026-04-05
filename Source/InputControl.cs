using System;

namespace Logos.Input
{
    /// <summary>
    /// Represents a base class for classes that implement input controls whose states can be
    /// changed by input events.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the state associated with the <see cref="InputControl{T}"/>.
    /// </typeparam>
    public abstract class InputControl<T>
    {
        private T? _state;

        /// <summary>
        /// Gets the state of the <see cref="InputControl{T}"/>.
        /// </summary>
        /// <returns>
        /// The state of the <see cref="InputControl{T}"/>.
        /// </returns>
        public T State
        {
            get => _state!;
        }

        /// <summary>
        /// Occurs when the state of the <see cref="InputControl{T}"/> is changed.
        /// </summary>
        public event EventHandler<T>? StateChanged;

        /// <summary>
        /// Notifies the <see cref="InputControl{T}"/> of a change to its state.
        /// </summary>
        /// <param name="state">
        /// The new state of the <see cref="InputControl{T}"/>.
        /// </param>
        protected void OnStateChanged(T state)
        {
            _state = state;
            StateChanged?.Invoke(this, state);
        }
    }
}
