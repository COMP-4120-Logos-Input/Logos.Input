using System;
using System.Collections.Generic;

namespace Logos.Input
{
    /// <summary>
    /// Represents an input mapper that routes input events triggered by specific key gestures to
    /// key observers.
    /// </summary>
    public class KeyboardMapper : IInputMapper
    {
        private readonly Dictionary<KeyGesture, IKeyObserver> _bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMapper"/> class.
        /// </summary>
        public KeyboardMapper()
        {
            _bindings = new Dictionary<KeyGesture, IKeyObserver>();
        }

        /// <summary>
        /// Routes input events triggered by the specified key gesture to the specified key
        /// observer.
        /// </summary>
        /// <param name="gesture">
        /// The key gesture to bind.
        /// </param>
        /// <param name="observer">
        /// The key observer that will listen for input events triggered by
        /// <paramref name="gesture"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer"/> is <see langword="null"/>.
        /// </exception>
        public void Bind(KeyGesture gesture, IKeyObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _bindings[gesture] = observer;
        }

        /// <summary>
        /// Removes the key observer that was listening for input events triggered by the specified
        /// key gesture.
        /// </summary>
        /// <param name="gesture">
        /// The key gesture to unbind.
        /// </param>
        public void Unbind(KeyGesture gesture)
        {
            _bindings.Remove(gesture);
        }

        /// <summary>
        /// Routes events exposed by a keyboard listener contained by the specified input provider
        /// to the <see cref="KeyboardMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing a keyboard listener whose events are to be routed to the
        /// <see cref="KeyboardMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain a keyboard listener.
        /// </exception>
        public void RouteEvents(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            RouteEvents(provider.GetListener<IKeyboardListener>());
        }

        /// <summary>
        /// Routes events exposed by the specified keyboard listener to the
        /// <see cref="KeyboardMapper"/>.
        /// </summary>
        /// <param name="listener">
        /// The keyboard listener whose events are to be routed to the <see cref="KeyboardMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listener"/> is <see langword="null"/>.
        /// </exception>
        public void RouteEvents(IKeyboardListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            listener.KeyPressed += OnKeyPressed;
            listener.KeyRepeated += OnKeyRepeated;
            listener.KeyReleased += OnKeyReleased;
        }

        /// <summary>
        /// Blocks events exposed by a keyboard listener contained by the specified input provider
        /// from reaching the <see cref="KeyboardMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing a keyboard listener whose events are to be blocked from
        /// reaching the <see cref="KeyboardMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain a keyboard listener.
        /// </exception>
        public void BlockEvents(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            BlockEvents(provider.GetListener<IKeyboardListener>());
        }

        /// <summary>
        /// Blocks events exposed by the specified keyboard listener from reaching the
        /// <see cref="KeyboardMapper"/>.
        /// </summary>
        /// <param name="listener">
        /// The keyboard listener whose events are to be blocked from reaching the
        /// <see cref="KeyboardMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listener"/> is <see langword="null"/>.
        /// </exception>
        public void BlockEvents(IKeyboardListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            listener.KeyPressed -= OnKeyPressed;
            listener.KeyRepeated -= OnKeyRepeated;
            listener.KeyReleased -= OnKeyReleased;
        }

        private void OnKeyPressed(object? sender, KeyEventArgs args)
        {
            KeyGesture gesture = new KeyGesture(args.Key, KeyAction.Press);

            if (_bindings.TryGetValue(gesture, out IKeyObserver? observer))
            {
                observer.OnKeyPressed(sender, args);
            }
        }

        private void OnKeyRepeated(object? sender, KeyEventArgs args)
        {
            KeyGesture gesture = new KeyGesture(args.Key, KeyAction.Repeat);

            if (_bindings.TryGetValue(gesture, out IKeyObserver? observer))
            {
                observer.OnKeyRepeated(sender, args);
            }
        }

        private void OnKeyReleased(object? sender, KeyEventArgs args)
        {
            KeyGesture gesture = new KeyGesture(args.Key, KeyAction.Release);

            if (_bindings.TryGetValue(gesture, out IKeyObserver? observer))
            {
                observer.OnKeyReleased(sender, args);
            }
        }
    }
}
