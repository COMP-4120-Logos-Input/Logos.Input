using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Logos.Input
{
    /// <summary>
    /// Represents a read-only collection of input mappers whose events are routed from a common
    /// input provider.
    /// </summary>
    public class InputMappingContext : IReadOnlyCollection<IInputMapper>
    {
        private readonly IInputProvider _provider;
        private readonly IInputMapper[] _mappers;
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputMappingContext"/> class that routes
        /// events from the specified input provider to the specified collection of input mappers.
        /// </summary>
        /// <param name="provider">
        /// The input provider to route events from.
        /// </param>
        /// <param name="mappers">
        /// The collection of input mappers to route events to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>. -or-
        /// <paramref name="mappers"/> is <see langword="null"/>. -or-
        /// One or more elements of <paramref name="mappers"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// An element of <paramref name="mappers"/> failed to obtain a required input listener from
        /// <paramref name="provider"/>.
        /// </exception>
        public InputMappingContext(IInputProvider provider, IEnumerable<IInputMapper> mappers)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _provider = provider;
            _mappers = mappers.ToArray();

            foreach (IInputMapper mapper in _mappers)
            {
                ArgumentNullException.ThrowIfNull(mapper);
                mapper.RouteEvents(_provider);
            }

            _isEnabled = true;
        }

        /// <inheritdoc/>
        public int Count
        {
            get => _mappers.Length;
        }

        /// <summary>
        /// Gets an input provider that the <see cref="InputMappingContext"/> is routing events
        /// from.
        /// </summary>
        /// <returns>
        /// An input provider that the <see cref="InputMappingContext"/> is routing events from.
        /// </returns>
        public IInputProvider Provider
        {
            get => _provider;
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="InputMappingContext"/> is allowing
        /// input events to be routed to its elements.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="InputMappingContext"/> is allowing input events
        /// to be routed to its elements; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsEnabled
        {
            get => _isEnabled;
        }

        /// <summary>
        /// Routes events from <see cref="Provider"/> to all the input mappers contained by the
        /// <see cref="InputMappingContext"/>.
        /// </summary>
        public void Enable()
        {
            if (!_isEnabled)
            {
                foreach (IInputMapper mapper in _mappers)
                {
                    mapper.RouteEvents(_provider);
                }

                _isEnabled = true;
            }
        }

        /// <summary>
        /// Blocks events routed from <see cref="Provider"/> from reaching any of the input mappers
        /// contained by the <see cref="InputMappingContext"/>.
        /// </summary>
        public void Disable()
        {
            if (_isEnabled)
            {
                foreach (IInputMapper mapper in _mappers)
                {
                    mapper.BlockEvents(_provider);
                }

                _isEnabled = false;
            }
        }

        /// <summary>
        /// Returns an <see cref="Enumerator"/> that can be used to iterate across the elements of
        /// the <see cref="InputMappingContext"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="Enumerator"/> that can be used to iterate across the elements of the
        /// <see cref="InputMappingContext"/>.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator<IInputMapper> IEnumerable<IInputMapper>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Enumerates the elements of an <see cref="InputMappingContext"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<IInputMapper>
        {
            private readonly IInputMapper[] _mappers;
            private int _index;

            internal Enumerator(InputMappingContext context)
            {
                _mappers = context._mappers;
                _index = -1;
            }

            /// <inheritdoc/>
            public IInputMapper Current
            {
                get => _mappers[_index];
            }

            /// <inheritdoc/>
            object? IEnumerator.Current
            {
                get => _mappers[_index];
            }

            /// <inheritdoc/>
            public readonly void Dispose()
            {
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                return ++_index < _mappers.Length;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
