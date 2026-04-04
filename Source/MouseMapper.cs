using System;
using System.Collections.Generic;
using System.Numerics;

namespace Logos.Input
{
    /// <summary>
    /// Represents an input mapper that routes input events triggered by specific mouse gestures to
    /// mouse observers.
    /// </summary>
    public class MouseMapper : IInputMapper
    {
        private const int UpDirectionFlag = 1 << 0;
        private const int DownDirectionFlag = 1 << 1;
        private const int LeftDirectionFlag = 1 << 2;
        private const int RightDirectionFlag = 1 << 3;

        private readonly Dictionary<MouseButtonGesture, IMouseButtonObserver> _buttonBindings;
        private readonly Dictionary<MouseMotionDirection, IMouseMotionObserver> _motionBindings;
        private readonly Dictionary<MouseWheelDirection, IMouseWheelObserver> _wheelBindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseMapper"/> class.
        /// </summary>
        public MouseMapper()
        {
            _buttonBindings = new Dictionary<MouseButtonGesture, IMouseButtonObserver>();
            _motionBindings = new Dictionary<MouseMotionDirection, IMouseMotionObserver>();
            _wheelBindings = new Dictionary<MouseWheelDirection, IMouseWheelObserver>();
        }

        /// <summary>
        /// Routes input events triggered by the specified mouse button gesture to the specified
        /// mouse button observer.
        /// </summary>
        /// <param name="gesture">
        /// The mouse button gesture to bind.
        /// </param>
        /// <param name="observer">
        /// The mouse button observer that will listen for input events triggered by
        /// <paramref name="gesture"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer"/> is <see langword="null"/>.
        /// </exception>
        public void Bind(MouseButtonGesture gesture, IMouseButtonObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _buttonBindings.Add(gesture, observer);
        }

        /// <summary>
        /// Routes input events triggered by the specified mouse motion direction to the specified
        /// mouse motion observer.
        /// </summary>
        /// <param name="direction">
        /// The mouse motion direction to bind.
        /// </param>
        /// <param name="observer">
        /// The mouse motion observer that will listen for input events triggered by mouse motion
        /// in <paramref name="direction"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer"/> is <see langword="null"/>.
        /// </exception>
        public void Bind(MouseMotionDirection direction, IMouseMotionObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _motionBindings.Add(direction, observer);
        }

        /// <summary>
        /// Routes input events triggered by the specified mouse wheel scroll direction to the
        /// specified mouse wheel observer.
        /// </summary>
        /// <param name="direction">
        /// The mouse wheel scroll direction to bind.
        /// </param>
        /// <param name="observer">
        /// The mouse wheel observer that will listen for input events triggered by mouse wheel
        /// scrolling in <paramref name="direction"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer"/> is <see langword="null"/>.
        /// </exception>
        public void Bind(MouseWheelDirection direction, IMouseWheelObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _wheelBindings.Add(direction, observer);
        }

        /// <summary>
        /// Removes the mouse button observer that was listening for input events triggered by the
        /// specified mouse button gesture.
        /// </summary>
        /// <param name="gesture">
        /// The mouse button gesture to unbind.
        /// </param>
        public void Unbind(MouseButtonGesture gesture)
        {
            _buttonBindings.Remove(gesture);
        }

        /// <summary>
        /// Removes the mouse motion observer that was listening for input events triggered by mouse
        /// motion in the specified direction.
        /// </summary>
        /// <param name="direction">
        /// The mouse wheel motion direction to unbind.
        /// </param>
        public void Unbind(MouseMotionDirection direction)
        {
            _motionBindings.Remove(direction);
        }

        /// <summary>
        /// Removes the mouse wheel observer that was listening for input events triggered by mouse
        /// wheel scrolling in the specified direction.
        /// </summary>
        /// <param name="direction">
        /// The mouse wheel scroll direction to unbind.
        /// </param>
        public void Unbind(MouseWheelDirection direction)
        {
            _wheelBindings.Remove(direction);
        }

        /// <summary>
        /// Routes events exposed by a mouse listener contained by the specified input provider to
        /// the <see cref="MouseMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing a mouse listener whose events are to be routed to the
        /// <see cref="MouseMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain a mouse listener.
        /// </exception>
        public void RouteEvents(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            RouteEvents(provider.GetListener<IMouseListener>());
        }

        /// <summary>
        /// Routes events exposed by the specified mouse listener to the <see cref="MouseMapper"/>.
        /// </summary>
        /// <param name="listener">
        /// The mouse listener whose events are to be routed to the <see cref="MouseMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listener"/> is <see langword="null"/>.
        /// </exception>
        public void RouteEvents(IMouseListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            listener.ButtonPressed += OnButtonPressed;
            listener.ButtonReleased += OnButtonReleased;
            listener.MouseMoved += OnMouseMoved;
            listener.WheelMoved += OnWheelMoved;
        }

        /// <summary>
        /// Blocks events exposed by a mouse listener contained by the specified input provider
        /// from reaching the <see cref="MouseMapper"/>.
        /// </summary>
        /// <param name="provider">
        /// The input provider containing a mouse listener whose events are to be blocked from
        /// reaching the <see cref="MouseMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="provider"/> does not contain a mouse listener.
        /// </exception>
        public void BlockEvents(IInputProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            BlockEvents(provider.GetListener<IMouseListener>());
        }

        /// <summary>
        /// Blocks events exposed by the specified mouse listener from reaching the
        /// <see cref="MouseMapper"/>.
        /// </summary>
        /// <param name="listener">
        /// The mouse listener whose events are to be blocked from reaching the
        /// <see cref="MouseMapper"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listener"/> is <see langword="null"/>.
        /// </exception>
        public void BlockEvents(IMouseListener listener)
        {
            ArgumentNullException.ThrowIfNull(listener);
            listener.ButtonPressed -= OnButtonPressed;
            listener.ButtonReleased -= OnButtonReleased;
            listener.MouseMoved -= OnMouseMoved;
            listener.WheelMoved -= OnWheelMoved;
        }

        private void OnButtonPressed(object? sender, MouseButtonEventArgs args)
        {
            MouseButtonGesture gesture = new MouseButtonGesture(args.Button, MouseButtonAction.Press);

            if (_buttonBindings.TryGetValue(gesture, out IMouseButtonObserver? observer))
            {
                observer.OnButtonPressed(sender, args);
            }
        }

        private void OnButtonReleased(object? sender, MouseButtonEventArgs args)
        {
            MouseButtonGesture gesture = new MouseButtonGesture(args.Button, MouseButtonAction.Release);

            if (_buttonBindings.TryGetValue(gesture, out IMouseButtonObserver? observer))
            {
                observer.OnButtonReleased(sender, args);
            }
        }

        private void OnMouseMoved(object? sender, MouseMotionEventArgs args)
        {
            if (_motionBindings.TryGetValue(MouseMotionDirection.Any, out IMouseMotionObserver? observer))
            {
                observer.OnMouseMoved(sender, args);
            }

            MouseMotionDirection direction = (MouseMotionDirection)ToDirectionFlags(args.Translation);

            if (direction != MouseMotionDirection.Any && _motionBindings.TryGetValue(direction, out observer))
            {
                observer.OnMouseMoved(sender, args);
            }
        }

        private void OnWheelMoved(object? sender, MouseWheelEventArgs args)
        {
            if (_wheelBindings.TryGetValue(MouseWheelDirection.Any, out IMouseWheelObserver? observer))
            {
                observer.OnWheelMoved(sender, args);
            }

            MouseWheelDirection direction = (MouseWheelDirection)ToDirectionFlags(args.Delta);

            if (direction != MouseWheelDirection.Any && _wheelBindings.TryGetValue(direction, out observer))
            {
                observer.OnWheelMoved(sender, args);
            }
        }

        private static int ToDirectionFlags(Vector2 direction)
        {
            int flags = 0;

            if (direction.Y > 0.0f)
            {
                flags |= UpDirectionFlag;
            }
            else if (direction.Y < 0.0f)
            {
                flags |= DownDirectionFlag;
            }

            if (direction.X > 0.0f)
            {
                flags |= LeftDirectionFlag;
            }
            else if (direction.X < 0.0f)
            {
                flags |= RightDirectionFlag;
            }

            return flags;
        }
    }
}
