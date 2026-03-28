using System;

namespace Logos.Input
{
    public static class MouseWheelEventHandler
    {
        public static EventHandler<MouseWheelEventArgs> Create<TState>(Action<TState> action, TState state)
        {
            ArgumentNullException.ThrowIfNull(action);
            return (_, _) => action(state);
        }

        public static EventHandler<MouseWheelEventArgs>  Create<TState>(Action<TState> action, Func<object?, MouseWheelEventArgs, TState> converter)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(converter);
            return (sender, args) => action(converter(sender, args));
        }
    }
}
