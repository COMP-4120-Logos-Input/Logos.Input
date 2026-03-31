using System;

namespace Logos.Input
{
    public static class MouseCursorEventHandler
    {
        public static EventHandler<MouseCursorEventArgs> Create<TState>(Action<TState> action, TState state)
        {
            ArgumentNullException.ThrowIfNull(action);
            return (_, _) => action(state);
        }

        public static EventHandler<MouseCursorEventArgs>  Create<TState>(Action<TState> action, Func<object?, MouseCursorEventArgs, TState> converter)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(converter);
            return (sender, args) => action(converter(sender, args));
        }
    }
}
