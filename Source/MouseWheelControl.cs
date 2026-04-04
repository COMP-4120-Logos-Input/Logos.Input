namespace Logos.Input
{
    /// <summary>
    /// Represents a base class for classes that represent mouse wheel controls whose states can be
    /// changed by mouse wheel events.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the state associated with the <see cref="MouseWheelControl{T}"/>.
    /// </typeparam>
    public abstract class MouseWheelControl<T> : InputControl<T>, IMouseWheelObserver
    {
        /// <summary>
        /// Notifies the <see cref="MouseWheelControl{T}"/> of a mouse wheel move event.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An object that contains mouse wheel event data.
        /// </param>
        public abstract void OnWheelMoved(object? sender, MouseWheelEventArgs e);
    }
}
