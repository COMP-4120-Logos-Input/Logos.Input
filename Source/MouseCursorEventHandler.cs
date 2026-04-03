using System;

namespace Logos.Input
{
    public static class MouseCursorEventHandler
    {
        public static EventHandler<MouseMotionEventArgs> Create<TState>(Action<TState> action, TState state)
        {
            ArgumentNullException.ThrowIfNull(action);
            return (_, _) => action(state);
        }

        public static EventHandler<MouseMotionEventArgs>  Create<TState>(Action<TState> action, Func<object?, MouseMotionEventArgs, TState> converter)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(converter);
            return (sender, args) => action(converter(sender, args));
        }
    }
}
