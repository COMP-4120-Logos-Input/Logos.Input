using System;

namespace Logos.Input
{
    /// <summary>
    /// Represents an event router that notifies input observers based on mapped input gestures.
    /// </summary>
    public interface IInputMapper
    {
        /// <summary>
        /// Routes events exposed by compatible input listeners contained by the specified input
        /// provider to the <see cref="IInputMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing input listeners whose events are to be routed to the
        /// <see cref="IInputMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain an input listener requested by the
        /// <see cref="IInputMapper"/>.
        /// </exception>
        void RouteEvents(IInputProvider provider);

        /// <summary>
        /// Blocks events exposed by compatible input listeners contained by the specified input
        /// provider from reaching the <see cref="IInputMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing input listeners whose events are to be blocked from
        /// reaching the <see cref="IInputMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain an input listener requested by the
        /// <see cref="IInputMapper"/>.
        /// </exception>
        void BlockEvents(IInputProvider provider);
    }
}
