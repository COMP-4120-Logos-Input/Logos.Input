using System;

namespace Logos.Input
{
    /// <summary>
    /// Represents an event router that invokes event handlers based on mapped input data.
    /// </summary>
    public interface IInputMapper
    {
        /// <summary>
        /// Routes events exposed by compatible input listeners contained by the specified input
        /// provider to the <see cref="IInputMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing input listeners whose events can be routed to the
        /// <see cref="IInputMapper"/>.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain an input listener requested by the
        /// <see cref="IInputMapper"/>.
        /// </exception>
        void Connect(IInputProvider provider);

        /// <summary>
        /// Blocks events exposed by compatible input listeners contained by the specified input
        /// provider from reaching the <see cref="IInputMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing input listeners whose events are to be blocked from the
        /// <see cref="IInputMapper"/>.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain an input listener requested by the
        /// <see cref="IInputMapper"/>.
        /// </exception>
        void Disconnect(IInputProvider provider);
    }
}
