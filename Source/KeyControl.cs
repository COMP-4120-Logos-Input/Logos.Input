namespace Logos.Input
{
    /// <summary>
    /// Represents a base class for classes that represent key controls whose states can be changed
    /// by key events.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the state associated with the <see cref="KeyControl{T}"/>.
    /// </typeparam>
    public abstract class KeyControl<T> : InputControl<T>, IKeyObserver
    {
        /// <summary>
        /// Notifies the <see cref="KeyControl{T}"/> of a key press event.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An object that contains key event data.
        /// </param>
        public abstract void OnKeyPressed(object? sender, KeyEventArgs e);

        /// <summary>
        /// Notifies the <see cref="KeyControl{T}"/> of a key repeat event.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An object that contains key event data.
        /// </param>
        public abstract void OnKeyRepeated(object? sender, KeyEventArgs e);

        /// <summary>
        /// Notifies the <see cref="KeyControl{T}"/> of a key release event.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An object that contains key event data.
        /// </param>
        public abstract void OnKeyReleased(object? sender, KeyEventArgs e);
    }
}
